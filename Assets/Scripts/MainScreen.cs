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

    public TextMeshProUGUI highscoreText;
    private int highscore;
    private int score;
    
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
        
        score = DataSerializer.Load<int>(SaveDataKeywords.score);
        highscore = DataSerializer.Load<int>(SaveDataKeywords.highScore);

        if (score > highscore)
        {
            highscore = score;
            DataSerializer.Save(SaveDataKeywords.highScore, highscore);
        }
        
        highscoreText.text = highscore.ToString("000000000");
        
        // Set saves for new game
        DataSerializer.Save(SaveDataKeywords.statBullet, 1);
        DataSerializer.Save(SaveDataKeywords.statCrit, 1);
        DataSerializer.Save(SaveDataKeywords.statDamage, 1);
        DataSerializer.Save(SaveDataKeywords.statFire, 1);
        DataSerializer.Save(SaveDataKeywords.statSpeed, 1);
        
        DataSerializer.Save(SaveDataKeywords.score, 0);
        DataSerializer.Save(SaveDataKeywords.levelPoints, 0);
        DataSerializer.Save(SaveDataKeywords.xp, 0);
        DataSerializer.Save(SaveDataKeywords.playerPosition, Vector3.zero);
    }
}
