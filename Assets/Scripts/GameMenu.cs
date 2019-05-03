using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//The GameMenu is for handling everything menu related - Player progression, stats,
//And inventory related mechanics.
public class GameMenu : MonoBehaviour
{
    //Unity stuff for the canvas menu and windows
    public GameObject theMenu;
    public GameObject[] windows;

    //array of player stats
    private CharStats[] playerStats;
    //Text boxes for player information (which is on the stats page)
    public Text[] nameText, hpText, mpText, lvlText, expText;
    public Slider[] expSlider;
    public Image[] charImage;
    //Stat holder for the player
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

    public string mainMenuName;
    /**/    
    /*
    GameMenu.Cs - Start()
    NAME
            void Start()
    SYNOPSIS
            Start is called before the first frame update
    DESCRIPTION
            Start creates an instance of the game menu (there should only be one ingame!)
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    
    void Start()
    {
        instance = this;
    }
    /**/    
    /*
    GameMenu.Cs - Update()
    NAME
            void Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            The update function in the GameMenu checks whenever the player right clicks, and
            if they do the inventory is closed or opened - it is closed if the inventory is already
            open, and opened if it is already closed. Every time it is opened the player stats are
            updated. 
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    
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
            //A nice little sound effect to play when the menu is opened.
            AudioManager.instance.PlaySFX(5);
        }
    }
    /**/    
    /*
    GameMenu.Cs - UpdateMainStats()
    NAME
            void UpdateMainStats()
    SYNOPSIS
            UpdateMainStats handles updating the main stats for each player in the menu.
    DESCRIPTION
            The update main stats function is in charge of getting the player stats from the game manager
            and then looping through them. It checks for active players and if a player is active,
            their stats are updated and the exp slider is also updated to show proper progression
            to the next level. If the player is not active their stat overlay is hidden. After all of that,
            the gold text object in Unity is updated with the players amount of gold.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    GameMenu.Cs - ToggleWindow()
    NAME
            void ToggleWindow(int windowNumber)
    SYNOPSIS
            ToggleWindow handles showing the appropriate window on player button click
    DESCRIPTION
            The function updates the main stats then loops through to check which window was selected
            based on an integer value set in Unity. IF the window selected matches a window in the window
            array, then the window is set active or deactive in the heirarchy depending on if it is currently open
            or not. 
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
        //the window that allows us to  select to use an item on a player
        itemCharChoiceMenu.SetActive(false);
    }
    /**/    
    /*
    GameMenu.Cs - CloseMenu()
    NAME
            void CloseMenu()
    SYNOPSIS
            Close menu closes everything menu related.
    DESCRIPTION
            The function loops through all of the windows that are present in Unity
            and sets them all to inactive. Then the game manager menu variable is updated
            and so is the ItemCharchoiceMenu
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    GameMenu.Cs - OpenStatus()
    NAME
            public void OpenStatus()
    SYNOPSIS
            OpenStatus updates the main stat buttons
    DESCRIPTION
            The function loops through all of the status buttons and sets them active and updates
            the stats.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    GameMenu.Cs - StatusChar()
    NAME
            public void StatusChar(int selected)
    SYNOPSIS
            StatusChar sets the text boxes of the player stats passed in.
    DESCRIPTION
            The function loops through all of the status buttons and sets them active and updates
            the stats text fields. It also sets the character sprite so when the player views the stats
            they see the players sprite and their statistics and equipped items/armor.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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

    /**/    
    /*
    GameMenu.Cs - ShowItems()
    NAME
            public void ShowItems()
    SYNOPSIS
            ShowItems shows the users inventory in the menu.
    DESCRIPTION
             This functions sorts the inventory for good measure and then loops through the inventory, 
             checks for valid items and if the item is a valid item it shows the item and sets the amount of
             the items the player has. If there is not an item in the the slot of the users inventory then
             it is shown as blank.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    GameMenu.Cs - SelectItem()
    NAME
            public void SelectItem(Item newItem)
    SYNOPSIS
            SelectItem sets text based on what the item selected is.
    DESCRIPTION
             This functions checks if an item selected is a weapon or potion and updates the text box accordingly
             (so weapons would show "equip" while potions would be "use"). Then the item name
             and the description text fields are updated with appropriate text.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    GameMenu.Cs - DiscardItem()
    NAME
            public void DiscardItem()
    SYNOPSIS
            DiscardItem discards a selected item from the users inventory.
    DESCRIPTION
             This function checks if an item is selected. If it is, then it accesses the gamemanager instance and
             removes the item from there.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void DiscardItem()
    {
        //checking if item is selected
        if (activeItem != null)
        {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }
    /**/    
    /*
    GameMenu.Cs - OpenItemCharChoice()
    NAME
            public void OpenItemCharChoice()
    SYNOPSIS
            OpenItemCharChoice shows a list of buttons of characters to select to use an item on.
    DESCRIPTION
             This function first shows the window of buttons of players to use an item on, and loops through 
             the characters and sets the buttons active to the corresponding names of the players eligible
             to equip or use the item.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    GameMenu.Cs - CloseItemCharChoice()
    NAME
            public void CloseItemCharChoice()
    SYNOPSIS
            CloseItemCharChoice closes out the item character choice selection for usable items.
    DESCRIPTION
             This function sets the buttons for characters to use items on false and then
             does the same in case an item is being used in battle and sets it false as well.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void CloseItemCharChoice()
    {
        itemCharChoiceMenu.SetActive(false);
        BattleManager.instance.itemMenu.SetActive(false);
    }
    /**/    
    /*
    GameMenu.Cs - UseItem()
    NAME
            public void UseItem(int selectChar)
    SYNOPSIS
            UseItem is a function that calls a function to use an item on a player.
    DESCRIPTION
             This function calls Use on a selected character and closes the 
             game window where a player to use an item on was selected.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        CloseItemCharChoice();
    }
    /**/    
    /*
    GameMenu.Cs - SaveGame()
    NAME
            public void SaveGame()
    SYNOPSIS
            SaveGame is a function that saves the game.
    DESCRIPTION
            This function calls the gamemanager/questmanager for the player and quest data
            and calls the respective function to save the data to PlayerPrefs.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }
    /**/    
    /*
    GameMenu.Cs - PlayButtonSound()
    NAME
            public void PlayButtonSound()
    SYNOPSIS
            PlayButtonSound is a function that plays a button sound.
    DESCRIPTION
            PlayButtonSound is a function that plays a button sound that is pre-defined in the
            Unity inspector. It is used in the menu and plays the same button sound for each menu interaction
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void PlayButtonSound()
    {
        AudioManager.instance.PlaySFX(4);
    }
    /**/    
    /*
    GameMenu.Cs - QuitGame()
    NAME
            public void QuitGame()
    SYNOPSIS
            QuitGame is a function that exits the game.
    DESCRIPTION
            This function loads our main menu scene, and while doing so 
            closes out the different game/audio managers and objects and the player data so
            it does not persist in the main menu scene.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);

        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }
}
