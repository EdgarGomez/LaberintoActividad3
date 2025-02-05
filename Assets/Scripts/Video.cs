using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoUrl = "https://drive.google.com/uc?export=download&id=1PxMb2-8GarPLJq5HONaauqtfJqMNBxk5";

    void Start()
    {
        videoPlayer.url = videoUrl;  
        videoPlayer.Play();  
    }
}
