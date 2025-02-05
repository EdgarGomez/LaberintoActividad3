using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KeyLoot : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip recollect;
    public Transform teleport;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                controller.enabled = false;
            }

            other.transform.position = teleport.position;

            if (controller != null)
            {
                controller.enabled = true;
            }
            audioSource.PlayOneShot(recollect);
            GameManager.instance.AddKey();
            Destroy(this.gameObject, 0.3f);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  
    }
}

