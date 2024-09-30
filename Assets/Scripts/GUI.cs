using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Health")]
    public Image[] healthIcons;
    public Sprite healthEmpty;
    public Sprite healthFull;

    private int _maxHealth;
    private int _currentHealth;

    [Header("XP")]
    public float xpBarLength = 394;
    public Image xpBar;
    public TextMeshProUGUI xpNumber;
    public Image xpBackground;

    private PlayerStats _stats;
    private PlayerHealthSystem _health;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _stats = player.GetComponent<PlayerStats>();
        _health = player.GetComponent<PlayerHealthSystem>();

        _maxHealth = (int)_health.maxHealth;

        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < _maxHealth)
            {
                healthIcons[i].sprite = healthFull;
            }
            else
            {
                healthIcons[i].enabled = false;
            }
        }

        _currentHealth = _maxHealth;
    }

    private void DecreaseHealth()
    {
        // Best code ever written
        _currentHealth--;
        healthIcons[_currentHealth].sprite = healthEmpty;
    }

    private void SetXpBar(float value)
    {
        if (value <= 0 && xpBar.enabled != false)
            xpBar.enabled = false;
        else if (xpBar.enabled == false)
            xpBar.enabled = true;

        xpBar.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value*xpBarLength); 
    }

    private void SetLevelPoints(int value)
    {
        if (value <= 0 && xpNumber.enabled)
        { 
            xpNumber.enabled = false; 
            xpBackground.enabled = false;
            return;
        }
        else if (xpNumber.enabled == false)
        { 
            xpNumber.enabled = true;
            xpBackground.enabled = true;
        }

        xpNumber.text = value.ToString();
    }

}
