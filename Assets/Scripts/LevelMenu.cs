using UnityEngine;
using UnityEngine.InputSystem;

public class LevelMenu : MonoBehaviour
{
    private Canvas canvas;
    // private bool _isMenuOpen = false;

    public void Open()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.enabled = true;
        // _isMenuOpen = true;
        Time.timeScale = 0f;
    }

    public void Close()
    {
        // _isMenuOpen = false;
        Time.timeScale = 1f;
        canvas.enabled = false;
    }

    public void MovementUpgrade()
    {
        GameObject.Find("Player").GetComponent<PlayerStats>().StatLevelUp(StatID.speedMultiplier, false);
    }

    public void MovementDowngrade()
    {
        GameObject.Find("Player").GetComponent<PlayerStats>().StatLevelUp(StatID.speedMultiplier, true);
    }
}
