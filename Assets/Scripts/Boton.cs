using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Boton : MonoBehaviour
{
    public Transform puerta;
    private AudioSource audioSource;
    private Vector3 puerAbierta;
    private Vector3 puerCerrada;
    private bool puertaAbierta = false;
    public Transform respawn;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!puertaAbierta)
            {
                audioSource.PlayOneShot(audioSource.clip);
                CharacterController characterController = other.GetComponent<CharacterController>();
                characterController.enabled = false;
                other.transform.position = respawn.transform.position;
                characterController.enabled = true;
                StartCoroutine(MoverPuerta(puerCerrada, puerAbierta));
                puertaAbierta = true;
            }
        }
    }
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        puerCerrada = puerta.position;
        puerAbierta = puerta.position + new Vector3(0,-3,0);
    }
    
    IEnumerator MoverPuerta(Vector3 inicio, Vector3 fin)
    {
        float duracion = 1f;
        float tiempo = 0;

        while (tiempo < duracion)
        {
            puerta.position = Vector3.Lerp(inicio, fin, tiempo/duracion);
            tiempo += Time.deltaTime;
            yield return null;
        }

        puerta.position = fin;
    }

}
