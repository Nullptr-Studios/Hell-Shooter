using System;
using TMPro;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreen : MonoBehaviour
{
    public String openScene;
    public TextMeshProUGUI version;
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(openScene);
    }

    private void Awake() 
    {
        version.text = DataSerializer.Load<float>("timeAlive").ToString();
    }
}
