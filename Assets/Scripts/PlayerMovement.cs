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

    void Start()
    {
        characterController = GetComponent<CharacterController>();
       audioSource = GetComponent<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        AplicarSalto();
        cameraTransform = Camera.main.transform;

        PlayFootstepSound();
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

        Vector3 direccion = cameraTransform.right * horizontal + cameraTransform.forward * vertical;

        direccion.y = 0;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : speed;

        characterController.Move(direccion.normalized * currentSpeed * Time.deltaTime);
    }

    void AplicarSalto()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeigh * -2f * gravity);
            audioSource.PlayOneShot(jumpSound);
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
