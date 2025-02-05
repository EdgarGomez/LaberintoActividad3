using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaSueloNoTp : MonoBehaviour
{
    public int damage = 30;

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {

            PlayerHealth player = other.GetComponent<PlayerHealth>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }

            
        }
           
    }
}
