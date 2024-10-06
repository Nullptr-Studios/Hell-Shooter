using TMPro;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI goldText;

    private int score;
    private int gold;
    private int gainedGold;
    
    // Start is called before the first frame update
    void Start()
    {
        score = DataSerializer.Load<int>(SaveDataKeywords.score);
        gold = DataSerializer.Load<int>(SaveDataKeywords.goldCoins);
        gainedGold = DataSerializer.Load<int>(SaveDataKeywords.goldCoinsDelta);
        
        scoreText.text = score.ToString("000000000");
        goldText.text = gold + " (+" + gainedGold + ")";
    }

    public void Exit() => Application.Quit();

    public void Menu() => SceneManager.LoadScene("MainMenu");
}
