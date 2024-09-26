using System;
using TMPro;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public String openScene;
    public TextMeshProUGUI gold;
    public TextMeshProUGUI version;
    
    public void ExitGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(openScene);
    }

    public void OpenStore()
    {
        SceneManager.LoadScene("Store");
    }

    private void Awake() 
    {
        version.text = Application.version;
        gold.text = DataSerializer.Load<int>(SaveDataKeywords.goldCoins).ToString();
    }
}
