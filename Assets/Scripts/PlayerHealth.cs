using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    CharacterController characterController;
    public AudioSource audioSource;
    public AudioClip hit;

    public Slider healthBar;
    public Transform respawnPoint;
    void Start()
    {
        if (audioSource == null) { audioSource = GetComponent<AudioSource>();}
        currentHealth = maxHealth;
        UpdateHealthUI();
        characterController = GetComponent<CharacterController>();
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = (float)currentHealth / maxHealth;
        }
    }


    public void Respawn()
    {
        StartCoroutine(HandleRespawn());
    }

    private IEnumerator HandleRespawn()
    {
        Debug.Log("Corutina");
        yield return new WaitForSeconds(0.1f);
        characterController.enabled = false;
        transform.position = respawnPoint.position;
        characterController.enabled = true;

        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0; 
        UpdateHealthUI();
        audioSource.PlayOneShot(hit);

        if (currentHealth == 0)
        {
            Respawn();
            Debug.Log("Respawn ejecutado");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
