using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float runSpeed = 10f;
    [SerializeField] private float jumpHeigh = 2f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookUpAngle = 80f;
    [SerializeField] private float maxLookDownAngle = -60f;
    private float rotationX = 0f;
    [SerializeField] private Transform playerBody;


    public float walkStepRate = 0.5f;
    public float runStepRate = 0.3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isRunning;
    private bool isMoving;
    private float nextStepTime = 0f;

    public AudioSource audioSource;
    public AudioClip footstepSound;
    public AudioClip jumpSound;

    private Animator animator;
    private bool isAttacking;
    private bool isJumping = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
       audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cameraTransform.localRotation = Quaternion.identity;

    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation();
        MovePlayer();
        AplicarSalto();
        HandleAttack();
        PlayFootstepSound();
        UpdateAnimator();
    }

    private void PlayFootstepSound()
    {
        if (isGrounded && isMoving && Time.time > nextStepTime)
        {
            float stepRate = isRunning ? runStepRate : walkStepRate;
            if (Time.time > nextStepTime)
            {
                audioSource.PlayOneShot(footstepSound);
                nextStepTime = Time.time + stepRate;
            }
        }
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        isMoving = (horizontal != 0 || vertical != 0);
        isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 direction = (forward * vertical + right * horizontal).normalized;

        float currentSpeed = isRunning ? runSpeed : speed;
        characterController.Move(direction * currentSpeed * Time.deltaTime);
    }

    void AplicarSalto()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeigh * -2f * gravity);
            audioSource.PlayOneShot(jumpSound);
            isJumping = true;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleAttack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isAttacking = true;
        }
    }

    void UpdateAnimator()
    {
        animator.SetBool("IsWalking", isMoving && !isRunning);
        animator.SetBool("IsRunning", isMoving && isRunning);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsJumping", !isGrounded);
        animator.SetBool("IsAttacking", isAttacking);

        if (isAttacking) isAttacking = false;
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + mouseX, 0);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, maxLookDownAngle, maxLookUpAngle);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }

}
