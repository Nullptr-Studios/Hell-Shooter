using TMPro;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Color activeColor;
    public Color inactiveColor;

    public TextMeshProUGUI goldText;
    private int _gold = 99;

    [Header("Dash")] 
    public GameObject dPrefab;
    public Button dPucharse;
    public Button dEquip;
    public Image dDisabled;
    public TextMeshProUGUI dGoldText;
    public int dPrice = 1;
    private GameObject _dObject;
    private bool _dBought;
    private bool _dEquipped;
    
    [Header("Shield")] 
    public GameObject sPrefab;
    public Button sPucharse;
    public Button sEquip;
    public Image sDisabled;
    public TextMeshProUGUI sGoldText;
    public int sPrice = 1;
    private GameObject _sObject;
    private bool _sBought;
    private bool _sEquipped;
    
    [Header("Health +1")]
    public GameObject hPrefab;
    public Button hPucharse;
    public Slider hSlider;
    public Image hDisabled;
    public TextMeshProUGUI hGoldText;
    public int hPrice = 1;
    private GameObject _hObject;
    private bool _hBought;
    private int _hLevel;

    [Header("Triple Weapon")] 
    public GameObject tPrefab;
    public Button tPucharse;
    public Button tEquip;
    public Image tDisabled;
    public TextMeshProUGUI tGoldText;
    public int tPrice = 1;
    private bool _tBought;
    private GameObject _tObject;
    
    [Header("Burst Weapon")]
    public GameObject bPrefab;
    public Button bPucharse;
    public Button bEquip;
    public Image bDisabled;
    public TextMeshProUGUI bGoldText;
    public int bPrice = 1;
    private bool _bBought;
    private GameObject _bObject;
    
    [Header("Default Weapon")]
    public GameObject wPrefab;
    public Button wEquip;
    private GameObject _wObject;

    private int _equippedWeapon;
    private GameObject _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player");

        _dBought = DataSerializer.Load<bool>(SaveDataKeywords.dashBought);
        _dEquipped = DataSerializer.Load<bool>(SaveDataKeywords.dashEquipped);
        _sBought = DataSerializer.Load<bool>(SaveDataKeywords.shieldBought);
        _sEquipped = DataSerializer.Load<bool>(SaveDataKeywords.shieldEquipped);

        _tBought = DataSerializer.Load<bool>(SaveDataKeywords.tripleBought);
        _bBought = DataSerializer.Load<bool>(SaveDataKeywords.burstBought);
        _equippedWeapon = DataSerializer.Load<int>(SaveDataKeywords.weaponEquiped);

        // Check if Dash bought
        if (!_dBought)
        {
            dEquip.interactable = false;
        }
        else
        {
            dEquip.interactable = true;
            Destroy(dPucharse.gameObject);
            Destroy(dDisabled.gameObject);
            
            dEquip.GetComponent<Image>().color = _dEquipped? activeColor : inactiveColor;
            if (_dEquipped) DashInstanciate();
        }
        
        // Check if Shield bought
        if (!_sBought)
        {
            sEquip.interactable = false;
        }
        else
        {
            sEquip.interactable = true;
            Destroy(sPucharse.gameObject);
            Destroy(sDisabled.gameObject);
            
            sEquip.GetComponent<Image>().color = _sEquipped? activeColor : inactiveColor;
            if (_sEquipped) ShieldInstanciate();
        }
        
        // -------- WEAPONS --------------------------------
        // Check if Triple bought
        if (!_tBought)
        {
            tEquip.interactable = false;
        }
        else
        {
            tEquip.interactable = true;
            Destroy(tPucharse.gameObject);
            Destroy(tDisabled.gameObject);
            
            tEquip.GetComponent<Image>().color = _equippedWeapon == 1? activeColor : inactiveColor;
            if (_equippedWeapon == 1) TripleWInstanciate();
        }
        
        // Check if Burst bought
        if (!_bBought)
        {
            bEquip.interactable = false;
        }
        else
        {
            bEquip.interactable = true;
            Destroy(bPucharse.gameObject);
            Destroy(bDisabled.gameObject);
            
            bEquip.GetComponent<Image>().color = _equippedWeapon == 2? activeColor : inactiveColor;
            if (_equippedWeapon == 2) BurstWInstanciate();
        }
        
        // Default Elected
        wEquip.GetComponent<Image>().color = _equippedWeapon == 0? activeColor : inactiveColor;
        if (_equippedWeapon == 0) DefaultWInstanciate();
    }

    // Equip functions
    // This functions handle the equip button for each item
    
    public void DashEquip()
    {
        if (!_dEquipped)
        {
            _dEquipped = true;
            dEquip.GetComponent<Image>().color = activeColor;
            
            DashInstanciate();
        }
        else
        {
            _dEquipped = false;
            dEquip.GetComponent<Image>().color = inactiveColor;
            Destroy(_dObject);
        }
    }
    
    public void ShieldEquip()
    {
        if (!_sEquipped)
        {
            _sEquipped = true;
            sEquip.GetComponent<Image>().color = activeColor;
            
            ShieldInstanciate();
        }
        else
        {
            _sEquipped = false;
            sEquip.GetComponent<Image>().color = inactiveColor;
            Destroy(_sObject);
        }
    }
    
    public void TripleWEquip()
    {
        if (_equippedWeapon != 1)
        {
            _equippedWeapon = 1;
            wEquip.GetComponent<Image>().color = inactiveColor;
            tEquip.GetComponent<Image>().color = activeColor;
            bEquip.GetComponent<Image>().color = inactiveColor;
            
            Destroy(_wObject);
            Destroy(_bObject);
            
            TripleWInstanciate();
        }
    }

    public void BurstWEquip()
    {
        if (_equippedWeapon != 2)
        {
            _equippedWeapon = 2;
            wEquip.GetComponent<Image>().color = inactiveColor;
            tEquip.GetComponent<Image>().color = inactiveColor;
            bEquip.GetComponent<Image>().color = activeColor;
            
            Destroy(_tObject);
            Destroy(_wObject);
            
            BurstWInstanciate();
        }
    }
    
    public void DefaultWEquip()
    {
        if (_equippedWeapon != 0)
        {
            _equippedWeapon = 0;
            wEquip.GetComponent<Image>().color = activeColor;
            tEquip.GetComponent<Image>().color = inactiveColor;
            bEquip.GetComponent<Image>().color = inactiveColor;
            
            Destroy(_tObject);
            Destroy(_bObject);
            
            DefaultWInstanciate();
        }
    }


    // Instantiate functions
    // Each function instantiates the game object
    // Cant do classes because can't call function from non-component classes with Unity Events
    // Unity devs pls cut yourselves harder than I do -x
    void DashInstanciate()
    {
        // Player instanciate prefab
        _dObject = Instantiate(dPrefab, _player.transform, true);
        _dObject.GetComponent<Dash>().enabled = false;
        var tr = _dObject.transform;
        _dObject.transform.localPosition = Vector3.zero + tr.position;
        _dObject.transform.localScale = tr.lossyScale;
        _dObject.transform.localRotation = tr.rotation;
    }
    
    void ShieldInstanciate()
    {
        // Player instanciate prefab
        _sObject = Instantiate(sPrefab, _player.transform, true);
        _sObject.GetComponent<Shield>().enabled = false;
        var tr = _sObject.transform;
        _sObject.transform.localPosition = Vector3.zero + tr.position;
        _sObject.transform.localScale = tr.lossyScale;
        _sObject.transform.localRotation = tr.rotation;
    }
    
    void TripleWInstanciate()
    {
        // Player instanciate prefab
        _tObject = Instantiate(tPrefab, _player.transform, true);
        _tObject.GetComponent<TripleSettings>().enabled = false;
        var tr = _tObject.transform;
        _tObject.transform.localPosition = Vector3.zero + tr.position;
        _tObject.transform.localScale = tr.lossyScale;
        _tObject.transform.localRotation = tr.rotation;
    }
    
    void BurstWInstanciate()
    {
        // Player instanciate prefab
        _bObject = Instantiate(bPrefab, _player.transform, true);
        _bObject.GetComponent<BurstSettings>().enabled = false;
        var tr = _bObject.transform;
        _bObject.transform.localPosition = Vector3.zero + tr.position;
        _bObject.transform.localScale = tr.lossyScale;
        _bObject.transform.localRotation = tr.rotation;
    }
    
    void DefaultWInstanciate()
    {
        // Player instanciate prefab
        _wObject = Instantiate(wPrefab, _player.transform, true);
        _wObject.GetComponent<DefaultWeapon>().enabled = false;
        var tr = _wObject.transform;
        _wObject.transform.localPosition = Vector3.zero + tr.position;
        _wObject.transform.localScale = tr.lossyScale;
        _wObject.transform.localRotation = tr.rotation;
    }
}

// TODO: Try to make this a class FUCK UNITY