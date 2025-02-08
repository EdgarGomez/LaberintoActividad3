using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    // Start is called before the first frame update
    private int keysCollected = 0;
    public int totalKeys = 5;
    public int score;
    public TMP_Text keysText;
    public TMP_Text scoreText;
    public Door finalDoor;
    public AudioSource audioSource;
    public AudioClip recollectScore;
    private void Awake()
    { 
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    public void AddKey() 
    {
        keysCollected++;
        keysText.text = keysCollected.ToString() + " / 5 ";
        if (keysCollected >= totalKeys)
        {
            Debug.Log("Se han recogido todas las llaves");
            audioSource.PlayOneShot(audioSource.clip);
            finalDoor.OpenDoor(); 
        }
    }

    public void AddCollectible(Transform playerTransform)
    {
        audioSource.transform.position = playerTransform.position;
        audioSource.PlayOneShot(recollectScore);
        score += 10;
        scoreText.text = "Score: " + score.ToString();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
