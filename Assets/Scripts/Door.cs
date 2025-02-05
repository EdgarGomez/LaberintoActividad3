using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isopen = false;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OpenDoor()
    {
        if (!isopen)
        {
            isopen = true;
            Debug.Log("Puerta abierta");
            animator.SetTrigger("Open");
        }

    }
}
