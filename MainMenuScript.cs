using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void ChangeScript(string scenename)
    {
        SceneManager.LoadScene(scenename);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
