using System.Collections;
using System.Collections.Generic;
using TMPro;
using ToolBox.Serialization;
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
    
    public TextMeshProUGUI Score;
    public TextMeshProUGUI HighScore;

    [Header("Abilities")] 
    public Image dashIcon;
    public Image shieldIcon;
    public Material abilityMaterialPrefab;
    private Material _dashMaterial;
    private Material _shieldMaterial;

    private PlayerStats _stats;
    private PlayerHealthSystem _health;
    private Dash _dash;
    private Shield _shield;
    private bool _dNullCheck;
    private bool _sNullCheck;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        _stats = player.GetComponent<PlayerStats>();
        _health = player.GetComponent<PlayerHealthSystem>();
        _dash = player.GetComponentInChildren<Dash>();
        _dNullCheck = _dash == null;
        _shield = player.GetComponentInChildren<Shield>();
        _sNullCheck = _shield == null;
        
        HighScore.text = DataSerializer.Load<int>(SaveDataKeywords.highScore).ToString("000000000");

        // Health Setup
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
        
        // Abilities setup
        if (!_dNullCheck)
        {
            _dashMaterial = Instantiate(abilityMaterialPrefab);
            dashIcon.material = _dashMaterial;
            _dashMaterial.SetFloat("_Cooldown", 1f);
        }
        else
        {
            dashIcon.enabled = false;
            if (!_sNullCheck)
                shieldIcon.rectTransform.position = new Vector3(
                    shieldIcon.rectTransform.position.x - 48f, 
                    shieldIcon.rectTransform.position.y, 
                    0);
        }

        if (!_sNullCheck)
        {
            _shieldMaterial = Instantiate(abilityMaterialPrefab);
            shieldIcon.material = _shieldMaterial;
            _shieldMaterial.SetFloat("_Cooldown", 1f);
        }
        else
        {
            shieldIcon.enabled = false;
        }

    }

    void Update()
    {
        if (!_dNullCheck)
            _dashMaterial.SetFloat("_Cooldown", _dash.cProgress);
        if (!_sNullCheck)
            _shieldMaterial.SetFloat("_Cooldown", _shield.cProgress);
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

    private void UpdateScore()
    {
        Score.text = _stats.Score.ToString("000000000");
    }

}
