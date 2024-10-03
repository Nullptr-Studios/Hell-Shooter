using System;
using TMPro;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class Shop : MonoBehaviour
{
    public Color activeColor;
    public Color inactiveColor;

    public TextMeshProUGUI goldText;
    private int _gold = 99;

    // Object Classes
    [SerializeField] private ShopItem dash;
    [SerializeField] private ShopItem shield;
    //[SerializeField] private ShopItem dash;
    
    [SerializeField] private ShopItem defaultWeapon;
    [SerializeField] private ShopItem tripleWeapon;
    [SerializeField] private ShopItem burstWeapon;

    public int equippedWeapon;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        defaultWeapon.isBought = true;

        dash.isBought = DataSerializer.Load<bool>(SaveDataKeywords.dashBought);
        dash.isEquiped = DataSerializer.Load<bool>(SaveDataKeywords.dashEquipped);
        shield.isBought = DataSerializer.Load<bool>(SaveDataKeywords.shieldBought);
        shield.isEquiped = DataSerializer.Load<bool>(SaveDataKeywords.shieldEquipped);
        
        tripleWeapon.isBought = DataSerializer.Load<bool>(SaveDataKeywords.tripleBought);
        burstWeapon.isBought = DataSerializer.Load<bool>(SaveDataKeywords.burstBought);
        equippedWeapon = DataSerializer.Load<int>(SaveDataKeywords.weaponEquiped);
        
        dash.Awake(activeColor, inactiveColor, _player, 0);
        shield.Awake(activeColor, inactiveColor, _player, 1);
        
        tripleWeapon.AwakeWeapon(activeColor, inactiveColor, _player, equippedWeapon, 1);
        burstWeapon.AwakeWeapon(activeColor, inactiveColor, _player, equippedWeapon, 2);
        defaultWeapon.AwakeWeapon(activeColor, inactiveColor, _player, equippedWeapon, 0);
    }

    // Equip functions
    // This functions handle the equip button for each item
    
    public void DashEquip() => dash.Equip(activeColor, inactiveColor, _player, 0);
    public void ShieldEquip() => shield.Equip(activeColor, inactiveColor, _player, 1);

    public void TripleWEquip() => tripleWeapon.EquipWeapon(activeColor, inactiveColor, _player, 1, this, defaultWeapon, burstWeapon);
    public void BurstWEquip() => tripleWeapon.EquipWeapon(activeColor, inactiveColor, _player, 2, this, defaultWeapon, tripleWeapon);
    public void DefaultWEquip() => tripleWeapon.EquipWeapon(activeColor, inactiveColor, _player, 0, this, tripleWeapon, burstWeapon);
}

[Serializable] internal class ShopItem
{
    public int price = 1;
    [NonSerialized] public bool isBought = false;
    [NonSerialized] public bool isEquiped = false;
    
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
    
    public void AwakeWeapon(Color activeColor, Color inactiveColor, GameObject player, int equippedWeapon, int weaponID)
    {
        if (!isBought)
        {
            equipButton.interactable = false;
        }
        else
        {
            equipButton.interactable = true;
            Object.Destroy(pucharseButton.gameObject);
            Object.Destroy(disabledImage.gameObject);
            
            equipButton.GetComponent<Image>().color = equippedWeapon == 1? activeColor : inactiveColor;
            if (equippedWeapon == weaponID) Create(player, -1);
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
    
    public void EquipWeapon(Color activeColor, Color inactiveColor, GameObject player, int weaponID, Shop shop, ShopItem w1, ShopItem w2)
    {
        if (shop.equippedWeapon == weaponID) return;
        shop.equippedWeapon = weaponID;
            
        equipButton.GetComponent<Image>().color = activeColor;
        w1.equipButton.GetComponent<Image>().color = inactiveColor;
        w2.equipButton.GetComponent<Image>().color = inactiveColor;
            
        Object.Destroy(w1.obj);
        Object.Destroy(w2.obj);
            
        Create(player, -1);
    }

    private void Create(GameObject player, int id)
    {
        obj = Object.Instantiate(prefab, player.transform, true);
        if (id == 0)
            obj.GetComponent<Dash>().enabled = false;
        else if (id == 1)
            obj.GetComponent<Shield>().enabled = false;
        var tr = obj.transform;
        obj.transform.localPosition = Vector3.zero + tr.position;
        obj.transform.localScale = tr.lossyScale;
        obj.transform.localRotation = tr.rotation;
    }
}
