using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameMenu : MonoBehaviour
{
    public GameObject theMenu;
    public GameObject[] windows;


    private CharStats[] playerStats;

    public Text[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider;
    public Image[] charImage;

    public GameObject[] charStatHolder;

    //stat buttons
    public GameObject[] statusButtons;


    //for updating stats in stats window
    public Text statusName, statusHP, statusMP, statusStr, statusDef, statusWpnEqpd, statusWpnPwr, statusArmrEqp, statusArmrPwr, statusExp;
    public Image statusImage;

    //Our item buttons to show in the inventory
    public ItemButton[] itemButtons;
    //Item selected in inventory
    public string selectedItem;
    public Item activeItem;

    //refernece ot item name/description in menu
    public Text itemName, itemDescription, useButtonText;
    public static GameMenu instance;

    //For using items on players
    public GameObject itemCharChoiceMenu;
    public Text[] itemCharChoiceNames;

    public Text goldText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (theMenu.activeInHierarchy)
            {
                //theMenu.SetActive(false) ;
                //GameManager.instance.gameMenuOpen = false;
                CloseMenu();
            }
            else
            {
                theMenu.SetActive(true);
                UpdateMainStats();
                GameManager.instance.gameMenuOpen = true;
            }
        }
    }

    public void UpdateMainStats()
    {
        playerStats = GameManager.instance.playerStats;
        for (int i = 0; i < playerStats.Length; i++)
        {

            if (playerStats[i].gameObject.activeInHierarchy)
            {
                charStatHolder[i].SetActive(true);
                nameText[i].text = playerStats[i].charName;
                hpText[i].text = "HP: " + playerStats[i].currentHP + "/" + playerStats[i].maxHP;
                mpText[i].text = "MP: " + playerStats[i].currentMP + "/" + playerStats[i].maxMP;
                lvlText[i].text = "Level: " + playerStats[i].playerLevel;
                expText[i].text = "" + playerStats[i].currentEXP + "/" + playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].maxValue = playerStats[i].expToNextLevel[playerStats[i].playerLevel];
                expSlider[i].value = playerStats[i].currentEXP;
                charImage[i].sprite = playerStats[i].charImage;
                //update stats
            }
            else
            {
                //deactivate the stat holdr in menu
                charStatHolder[i].SetActive(false);
            }
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void ToggleWindow(int windowNumber)
    {
        UpdateMainStats();
        for (int i = 0; i < windows.Length; i++)
        {
            if (i == windowNumber)
            {
                windows[i].SetActive(!windows[i].activeInHierarchy);
            }
            else
            {
                windows[i].SetActive(false);
            }
        }
        itemCharChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].SetActive(false);
        }
        theMenu.SetActive(false);

        GameManager.instance.gameMenuOpen = false;

        itemCharChoiceMenu.SetActive(false);
    }

    public void OpenStatus()
    {
        UpdateMainStats();

        StatusChar(0);
        //update info
        for (int i = 0; i < statusButtons.Length; i++)
        {
            statusButtons[i].SetActive(playerStats[i].gameObject.activeInHierarchy);
            statusButtons[i].GetComponentInChildren<Text>().text = playerStats[i].charName;
        }
    }

    public void StatusChar(int selected)
    {
        statusName.text = playerStats[selected].charName;
        statusHP.text = "" + playerStats[selected].currentHP + "/" + playerStats[selected].maxHP;
        statusMP.text = "" + playerStats[selected].currentMP + "/" + playerStats[selected].maxMP;
        statusStr.text = playerStats[selected].strength.ToString();
        statusDef.text = playerStats[selected].defence.ToString();
        if (playerStats[selected].equippedWpn != "")
        {
            statusWpnEqpd.text = playerStats[selected].equippedWpn;
        }

        statusWpnPwr.text = playerStats[selected].wpnPwr.ToString();
        if (playerStats[selected].equppedArmr != "")
        {
            statusWpnEqpd.text = playerStats[selected].equppedArmr;
        }
        statusArmrPwr.text = playerStats[selected].armrPwr.ToString();
        //Getting exp to next level - current exp to find exp remaining.
        statusExp.text = (playerStats[selected].expToNextLevel[playerStats[selected].playerLevel] - 
            playerStats[selected].currentEXP).ToString();

        statusImage.sprite = playerStats[selected].charImage;
    }


    public void ShowItems()
    {
        GameManager.instance.SortItems();
        for (int i = 0; i < itemButtons.Length;i++)
        {
            itemButtons[i].buttonValue = i;
            //If there's an item in that position
            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtons[i].ButtonImage.gameObject.SetActive(true);
                //Calling item function in gamemanager, returning an item, going into item script and getting the sprite of the item
                itemButtons[i].ButtonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                //Setting amt of items
                itemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtons[i].ButtonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item newItem)
    {
        activeItem = newItem;

        if (activeItem.isItem)
        {
            useButtonText.text = "Use";
        }

        if (activeItem.isWeapon || activeItem.isArmour)
        {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        //checking if item is selected
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(true);

        for (int i = 0; i < itemCharChoiceNames.Length; i++)
        {

            itemCharChoiceNames[i].text = GameManager.instance.playerStats[i].charName;
            //Checks if character is active to show the button or nots
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(GameManager.instance.playerStats[i].gameObject.activeInHierarchy);

        }
    }
    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }
}
