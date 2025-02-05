using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampaPequeña : MonoBehaviour
{
    public int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        

        if (other.CompareTag("Player"))
        {
            Debug.Log("Dañado");

            PlayerHealth player = other.GetComponent<PlayerHealth>();
            CharacterController controller = other.GetComponent<CharacterController>();

            if (player != null)
            {
                player.TakeDamage(damage);
            }  
        }
           
    }
}
