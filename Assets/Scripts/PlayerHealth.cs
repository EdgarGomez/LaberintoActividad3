using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private int vidas = 3;
    CharacterController characterController;
    public AudioSource audioSource;
    public AudioClip hit;
    public AudioClip death;

    public Slider healthBar;
    public Transform respawnPoint;
    public TMP_Text muerte;
    public TMP_Text vidasText;
    public Image gameOver;
    
    void Start()
    {
        if (audioSource == null) { audioSource = GetComponent<AudioSource>();}
        currentHealth = maxHealth;
        UpdateHealthUI();
        characterController = GetComponent<CharacterController>();
        muerte.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
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

        GameManager.instance.invokeRespawn();

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
            vidas -= 1;
            vidasText.text = "x " + vidas.ToString();
            if (vidas > 0)
            {
                Respawn();
                Debug.Log("Respawn ejecutado");
            }
            else {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        audioSource.PlayOneShot(death);
        muerte.gameObject.SetActive(true);
        // gameOver.gameObject.SetActive(true);
        if (characterController != null)
        {
            characterController.enabled = false;
        }
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }

        StartCoroutine(ReturnToMenu());
    }

    private IEnumerator ReturnToMenu()
    {
        yield return new WaitForSeconds(3f);
        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = true;
            }
        }
        SceneManager.LoadScene(0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
