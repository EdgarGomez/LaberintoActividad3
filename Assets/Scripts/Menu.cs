using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private AudioMixer mainMixer;
    private Resolution[] resolutions;
    private List<string> resolutionOptions = new List<string>();    
    [SerializeField] private TMP_Dropdown resolutionDropdowns;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;

        resolutions = Screen.resolutions;
        foreach (var resolution in resolutions)
        {
            resolutionOptions.Add(resolution.width + "x" + resolution.height);
        }
        resolutionDropdowns.AddOptions(resolutionOptions);
    }
    public void OnOptionsButtonClicked()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainMenu.SetActive(true );
            optionsMenu.SetActive(false);
        }

    }

    public void SetNewVolumetoMusic(float volume)
    {
        mainMixer.SetFloat("musicVolume", volume);
    }
    public void SetNewVolumetoSFX(float volume)
    {
        mainMixer.SetFloat("sfxVolume", volume);
    }

    public void setFscreen(bool fscreen)
    {
        Screen.fullScreen = fscreen;
    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void SetNewRes(int resolution)
    {
        Resolution chosenResolution = resolutions[resolution];
        Screen.SetResolution(chosenResolution.width, chosenResolution.height, Screen.fullScreen);
    }
}
