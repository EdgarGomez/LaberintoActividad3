using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float rangoInteraccion = 3f;
    public LayerMask interactableLayer;
    private Camera playerCamera;
    private bool noclip = false;
    private CharacterController characterController;

    public float speed = 5f;
    public float noclipspeed = 10;
    void Start()
    {
        playerCamera = Camera.main;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        if (noclip)
        {
            NoClipMovement();
        }
    }

    private void NoClipMovement()
    {
        float moveSpeed = noclipspeed;
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), Input.GetKey(KeyCode.Space) ? 1 : (Input.GetKey(KeyCode.LeftControl) ? -1 : 0), Input.GetAxis("Vertical"));
        transform.position += playerCamera.transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime;
    }

    private void TryInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, rangoInteraccion, interactableLayer))
        {
            Debug.Log("se ha detectado " + hit.collider.name);
            if (hit.collider.CompareTag("NoClip"))
            {
                Debug.Log("noclip detectado");
                ToggleNoClip();
            }
        }
    }

    void ToggleNoClip()
    {
        noclip = !noclip;

        if (noclip)
        {
           characterController.enabled = false;
            Debug.Log("Modo Noclip Activado");
        }
        else
        {
            characterController.enabled = true;
            Debug.Log("Modo Noclip Desactivado");
        }

        Debug.Log("Modo Noclip: " + (noclip ? "Activado" : "Desactivado"));
    }
}
