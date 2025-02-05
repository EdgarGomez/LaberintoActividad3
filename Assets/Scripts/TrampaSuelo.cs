using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaSuelo : MonoBehaviour
{
    public int damage = 15;
    public Transform spawn;

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {

            PlayerHealth player = other.GetComponent<PlayerHealth>();
            CharacterController controller = other.GetComponent<CharacterController>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            if (spawn != null)
            {   
                controller.enabled = false; 
                other.transform.position = spawn.position; 
                controller.enabled = true; 
            
            }
            
        }
           
    }
}
