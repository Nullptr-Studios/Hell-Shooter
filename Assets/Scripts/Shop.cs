using System;
using TMPro;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Shop : MonoBehaviour
{
    public Color activeColor;
    public Color inactiveColor;

    public TextMeshProUGUI goldText;
    [NonSerialized] public int gold = 10;

    // Object Classes
    [SerializeField] private ShopItem dash;
    [SerializeField] private ShopItem shield;
    [SerializeField] private ShopItem health;
    
    [SerializeField] private ShopItem defaultWeapon;
    [SerializeField] private ShopItem tripleWeapon;
    [SerializeField] private ShopItem burstWeapon;

    public int equippedWeapon;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        defaultWeapon.isBought = true;
        gold = DataSerializer.Load<int>(SaveDataKeywords.goldCoins);
        UpdateGold();

        dash.isBought = DataSerializer.Load<bool>(SaveDataKeywords.dashBought);
        dash.isEquiped = DataSerializer.Load<bool>(SaveDataKeywords.dashEquipped);
        shield.isBought = DataSerializer.Load<bool>(SaveDataKeywords.shieldBought);
        shield.isEquiped = DataSerializer.Load<bool>(SaveDataKeywords.shieldEquipped);
        health.isBought = DataSerializer.Load<bool>(SaveDataKeywords.healthBought);
        health.isEquiped = DataSerializer.Load<bool>(SaveDataKeywords.healthBought);
        health.level = DataSerializer.Load<int>(SaveDataKeywords.healthLevel);
        
        tripleWeapon.isBought = DataSerializer.Load<bool>(SaveDataKeywords.tripleBought);
        burstWeapon.isBought = DataSerializer.Load<bool>(SaveDataKeywords.burstBought);
        equippedWeapon = DataSerializer.Load<int>(SaveDataKeywords.weaponEquiped);
        
        dash.Awake(activeColor, inactiveColor, _player, 0);
        shield.Awake(activeColor, inactiveColor, _player, 1);
        health.AwakeHealth(_player);
        
        tripleWeapon.AwakeWeapon(activeColor,  _player, equippedWeapon, 1);
        burstWeapon.AwakeWeapon(activeColor,  _player, equippedWeapon, 2);
        defaultWeapon.AwakeWeapon(activeColor,  _player, equippedWeapon, 0);
    }

    public void ExitShop()
    {
        DataSerializer.Save(SaveDataKeywords.goldCoins, gold);
        
        DataSerializer.Save(SaveDataKeywords.dashBought, dash.isBought);
        DataSerializer.Save(SaveDataKeywords.dashEquipped, dash.isEquiped);
        DataSerializer.Save(SaveDataKeywords.shieldBought, shield.isBought);
        DataSerializer.Save(SaveDataKeywords.shieldEquipped, shield.isEquiped);
        DataSerializer.Save(SaveDataKeywords.healthBought, health.isBought);
        DataSerializer.Save(SaveDataKeywords.healthLevel, health.level);
        
        DataSerializer.Save(SaveDataKeywords.tripleBought, tripleWeapon.isBought);
        DataSerializer.Save(SaveDataKeywords.burstBought, burstWeapon.isBought);
        DataSerializer.Save(SaveDataKeywords.weaponEquiped, equippedWeapon);

        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateGold() => goldText.text = gold.ToString();

    // Equip functions
    // This functions handle the equip button for each item
    public void DashEquip() => dash.Equip(activeColor, inactiveColor, _player, 0);
    public void ShieldEquip() => shield.Equip(activeColor, inactiveColor, _player, 1);
    
    public void TripleWEquip() => tripleWeapon.EquipWeapon(_player, 1, this, defaultWeapon, burstWeapon);
    public void BurstWEquip() => burstWeapon.EquipWeapon(_player, 2, this, defaultWeapon, tripleWeapon);
    public void DefaultWEquip() => defaultWeapon.EquipWeapon(_player, 0, this, tripleWeapon, burstWeapon);
    
    // Buy functions
    public void BuyDash() => dash.Buy(this);
    public void BuyShield() => shield.Buy(this);
    public void BuyHealth() => health.BuyHealth(this, _player);
    public void BuyTriple() => tripleWeapon.Buy(this);
    public void BuyBurst() => burstWeapon.Buy(this);
}

[Serializable] internal class ShopItem
{
    public int price = 1;
    public int maxLevel = 0;
    [NonSerialized] public bool isBought = false;
    [NonSerialized] public bool isEquiped = false;
    [NonSerialized] public int level;
    
    [Header("UI Elements")]
    public Button pucharseButton;
    public Button equipButton;
    public Image disabledImage;
    public TextMeshProUGUI priceText;
    public Slider levelSlider;

    [Header("Game Objects")]
    public GameObject prefab;
    [NonSerialized] public GameObject obj;

    public void Awake(Color activeColor, Color inactiveColor, GameObject player, int id)
    {
        priceText.text = price.ToString();
        
        // Check if Dash bought
        if (!isBought)
        {
            equipButton.interactable = false;
        }
        else
        {
            equipButton.interactable = true;
            Object.Destroy(pucharseButton.gameObject);
            Object.Destroy(disabledImage.gameObject);
            
            equipButton.GetComponent<Image>().color = isEquiped? activeColor : inactiveColor;
            if (isEquiped) Create(player, id);
        }
    }

    public void AwakeHealth(GameObject player)
    {
        priceText.text = price.ToString();
        
        if (isBought)
        {
            Object.Destroy(disabledImage.gameObject);
            for (int i = 1; i <= level; i++)
            {
                price *= level+1;   
            }
            priceText.text = price.ToString();
            
            if (level >= maxLevel)
                Object.Destroy(pucharseButton.gameObject);
            
            Create(player, -1);
        }
        else
        {
            level = 0;
        }
        
        levelSlider.value = (float)level/maxLevel;
    }

    public void AwakeWeapon(Color activeColor, GameObject player, int equippedWeapon, int weaponID)
    {
        if (priceText != null) priceText.text = price.ToString();
        
        if (!isBought)
        {
            equipButton.interactable = false;
        }
        else
        {
            equipButton.GetComponent<Image>().color = equippedWeapon == weaponID? activeColor : Color.white;
            if (equippedWeapon == weaponID) Create(player, -1);
            
            equipButton.interactable = true;
            if (pucharseButton && disabledImage)
            {
                Object.Destroy(pucharseButton.gameObject);
                Object.Destroy(disabledImage.gameObject);
            }
        }
    }
    
    public void Equip(Color activeColor, Color inactiveColor, GameObject player, int id)
    {
        if (!isEquiped)
        {
            isEquiped = true;
            equipButton.GetComponent<Image>().color = activeColor;
        
            Create(player, id);
        }
        else
        {
            isEquiped = false;
            equipButton.GetComponent<Image>().color = inactiveColor;
            Object.Destroy(obj);
        }
    }
    
    public void EquipWeapon(GameObject player, int weaponID, Shop shop, ShopItem w1, ShopItem w2)
    {
        if (shop.equippedWeapon == weaponID) return;
        shop.equippedWeapon = weaponID;
            
        equipButton.GetComponent<Image>().color = shop.activeColor;
        w1.equipButton.GetComponent<Image>().color = Color.white;
        w2.equipButton.GetComponent<Image>().color = Color.white;
            
        Object.Destroy(w1.obj);
        Object.Destroy(w2.obj);
            
        Create(player, -1);
    }

    public void Buy(Shop shop)
    {
        if (shop.gold - price < 0) return;
        
        shop.gold -= price;
        shop.UpdateGold();
        isBought = true;
        
        equipButton.interactable = true;
        Object.Destroy(pucharseButton.gameObject);
        Object.Destroy(disabledImage.gameObject);
    }

    public void BuyHealth(Shop shop, GameObject player)
    {
        if (shop.gold - price < 0) return;
        
        shop.gold -= price;
        
        if (level == 0)
        {
            Object.Destroy(disabledImage.gameObject);
            isBought = true;
        }
        
        if (++level >= maxLevel)
            Object.Destroy(pucharseButton.gameObject);
        
        price *= level + 1;
        if (priceText.text!=null) priceText.text = price.ToString();

        levelSlider.value = (float)level/maxLevel;
        
        DataSerializer.Save(SaveDataKeywords.healthLevel, level);
        
        if (obj != null) Object.Destroy(obj.gameObject);
        Create(player, -1);
    }

    private void Create(GameObject player, int id)
    {
        obj = Object.Instantiate(prefab, player.transform, true);
        switch (id)
        {
            case 0:
                obj.GetComponent<Dash>().enabled = false;
                break;
            case 1:
                obj.GetComponent<Shield>().enabled = false;
                break;
            default:
                break;
        }
        var tr = obj.transform;
        obj.transform.localPosition = Vector3.zero + tr.position;
        obj.transform.localScale = tr.lossyScale;
        obj.transform.localRotation = tr.rotation;
    }
}
