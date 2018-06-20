using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    private KeyCode keybind = KeyCode.Escape;

    public AudioMixer music;

    public AudioMixer soundfx;

    public void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "testMode")
        {
            callPauseMenu(keybind);
        }
    }

    public void ChangeScript(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void callPauseMenu(KeyCode key)
    {
        if (Input.GetKey(key))
        {
            SceneManager.LoadScene("pauseMenu");
        }
    }

    public void SetMusic(float element)
    {
        music.SetFloat("music", element);
    }

    public void SetSoundfx(float element)
    {
        soundfx.SetFloat("soundfx", element);
    }

    public void SetQuality(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, isFullscreen);
    }

    public void SetReso(int reso)
    {
        Resolution[] resolution = Screen.resolutions;
        Resolution setvalue = resolution[reso];
        Screen.SetResolution(setvalue.width, setvalue.height, Screen.fullScreen);
    }
}
