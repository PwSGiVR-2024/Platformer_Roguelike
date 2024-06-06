using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEditor.Events;
using UnityEngine.Events;

public class TextManager : MonoBehaviour
{
    
    [SerializeField]
    GameObject inventory;
    [SerializeField]
    List<TMP_Text> StatsTexts;

    Dictionary<string, float> StatsValues;

    [SerializeField]
    GameObject spellListParent;
    [SerializeField]
    GameObject descriptionParent;
    [SerializeField]
    GameObject upgradeListParent;
    [SerializeField]
    GameObject player;

    PlayerStatsManager playerStatsManager;
    MoneyManager moneyManager;
    SpellManager spellManager;
    ItemOnShop itemOnShop;

    [SerializeField] 
    private TMP_Text goldText;
    [SerializeField]
    private TMP_Text hpText;
    [SerializeField]
    private TMP_Text staminaText;
    [SerializeField]
    private TMP_Text adText;
    [SerializeField]
    private TMP_Text asText;
    [SerializeField]
    private TMP_Text movementText;
    [SerializeField]
    private TMP_Text jpText; //jump Power
    [SerializeField]
    private TMP_Text dpText; //dash Power

    // Start is called before the first frame update
    void Start()
    {
        playerStatsManager = player.GetComponent<PlayerStatsManager>();
        moneyManager = player.GetComponent<MoneyManager>();
        spellManager = player.GetComponent<SpellManager>();
        
        foreach(var spell in spellManager.spells)
        {
            Image newImage = new GameObject("Image").AddComponent<Image>();
            newImage.transform.SetParent(spellListParent.transform); // ustaw parentTransform na odpowiedni transform rodzica dla obiekt�w Image

            Sprite sprite = spell.GetComponent<Spell>().spellData.ImageItem;
            newImage.sprite = sprite;
        }
        inventory.SetActive(!inventory.activeSelf);
        StatsValues = new Dictionary<string, float>()
        {
            {"HealthValue", playerStatsManager.playerStats.stats[0].value},
            {"StaminaValue", playerStatsManager.playerStats.stats[3].value},
            {"AttackDamageValue",playerStatsManager.playerStats.stats[6].value},
            {"AttackSpeedValue", playerStatsManager.playerStats.stats[8].value},
            {"MovementValue", playerStatsManager.playerStats.stats[15].value},
            {"JumpPowerValue", playerStatsManager.playerStats.stats[14].value},
            {"DashPowerValue", playerStatsManager.playerStats.stats[16].value},
            {"GoldValue", moneyManager.GetMoney()}
        };
        playerStatsManager.updateGUI += UpdateGUI;
        ItemOnShop.updateGUIUpgrades += UpdateUpgrades;
        ItemOnShop.showDescription += ShowDescription;
        UpdateGUI();
    }
    private void UpdateGUI()
    {
        foreach (var stat in StatsTexts)
        {
            StatsValues["HealthValue"] = playerStatsManager.playerStats.stats[0].value;
            StatsValues["StaminaValue"] = playerStatsManager.playerStats.stats[3].value;
            StatsValues["AttackDamageValue"] = playerStatsManager.playerStats.stats[6].value;
            StatsValues["AttackSpeedValue"] = playerStatsManager.playerStats.stats[8].value;
            StatsValues["MovementValue"] = playerStatsManager.playerStats.stats[15].value;
            StatsValues["JumpPowerValue"] = playerStatsManager.playerStats.stats[14].value;
            StatsValues["DashPowerValue"] = playerStatsManager.playerStats.stats[16].value;
            StatsValues["GoldValue"] = moneyManager.GetMoney();
        
            float value = 0;
            StatsValues.TryGetValue(stat.name, out value);

            stat.text = value.ToString();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        { 
            UpdateGUI();
            inventory.SetActive(!inventory.activeSelf);
        }
    }
    private void UpdateUpgrades(GameObject prefabOfItem)
    {
        GameObject newUpgrade = Instantiate(prefabOfItem);
        Destroy(newUpgrade.GetComponent<SpriteRenderer>());
        newUpgrade.transform.SetParent(upgradeListParent.transform);
        Image image = newUpgrade.AddComponent<Image>();
        Items newItem = newUpgrade.GetComponent<ItemOnShop>().m_ScriptableObject;
        image.sprite = newItem.ImageItem;
        
        
    }


    public void ShowDescription(GameObject prefabOfItem)
    {
        print("wykryto mysz");
        descriptionParent.GetComponent<TMP_Text>().text = prefabOfItem.GetComponent<ItemOnShop>().m_ScriptableObject.Description;
    }

}