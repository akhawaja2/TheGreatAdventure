using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//The gameManager is in charge of the.. Game. It handles all of the menus,
//fading betwen areas, player statistics, shops, items, battles and player gold count.

public class GameManager : MonoBehaviour
{
    //Making it an instance so only 1 GameManager
    public static GameManager instance;
    //Stores player stats
    public CharStats[] playerStats;

    //Checking what menus are open
    public bool gameMenuOpen, dialogActive, fadingBetweenAreas, shopActive, battleActive;

    //For storing and collection of items and amt.
    public string[] itemsHeld;
    public int[] numberOfItems;
    //Find item in reference items list and display image 
    public Item[] referenceItems;

    public int currentGold;
     /**/    
    /*
    GameManager.Cs - Start()
    NAME
           void  Start()
    SYNOPSIS
            Start is called before the first frame update
    DESCRIPTION
            This function sets our instance to the current copy of GameManager, lets Unity not to destroy
            it upon load, and sorts items for good measure so all duplicates/spacing problems are fixed
            when the player first opens their inventory.
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

        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    /**/    
    /*
    GameManager.Cs - Update()
    NAME
           void  Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            This function checks if a menu, dialog, shop, or battle and also checks if the player is
            in between scenes. If so, then the player canMove is set to false so they cannot move while
            in these windows The KeyCodeJ if statement is used for inventory testing - specifically to add those items
            to the players inventory. KeyCode O and P are used for testing saving and loading via
            PlayerPrefs in Unity.
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
        if (gameMenuOpen || dialogActive || fadingBetweenAreas || shopActive || battleActive)
        {
            PlayerController.instance.canMove = false;
        }
        else
        {
            PlayerController.instance.canMove = true;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            AddItem("Iron Armor");
            AddItem("Mana Potion");
            AddItem("Iron Sword");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveData();
            Debug.Log("Saved game");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadData();

            Debug.Log("loaded game");
        }
    }

    /**/    
    /*
    GameManager.Cs - GetItemDetails()
    NAME
           public Item GetItemDetails(string itemToGet)
    SYNOPSIS
            GetItemDetails handles the retrieving of details regarding an item
    DESCRIPTION
            This function checks if an item exists in our reference item string. It takes the string
            of the item to find and searches through the item references and if found, returns the
            object. If nothing is found null is returned.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public Item GetItemDetails(string itemToGet)
    {
        for (int i = 0; i < referenceItems.Length; i++)
        {
            if (referenceItems[i].itemName ==itemToGet)
            {
                return referenceItems[i];
            }
        }
        return null;
    }
    /**/    
    /*
    GameManager.Cs - SortItems()
    NAME
           public Item SortItems()
    SYNOPSIS
            SortItems handles the sorting  of items in the player inventory.
    DESCRIPTION
            This function loops through the inventory and stacks any duplicate items and
            removes any spacing in between items so the inventory looks cleaner. It is pretty much less 
            optimal bubble sort. 
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SortItems()
    {
        //Checking if empty item after a blank space
        bool itemAfterSpace = true;

        while (itemAfterSpace)
        {
            itemAfterSpace = false;
            //for each item check if it is blank. if it is do a basic bubble sort
            for (int i = 0; i < itemsHeld.Length - 1; i++)
            {
                if (itemsHeld[i] == "")
                {
                    itemsHeld[i] = itemsHeld[i + 1];
                    itemsHeld[i + 1] = "";

                    numberOfItems[i] = numberOfItems[i + 1];
                    numberOfItems[i + 1] = 0;

                    if (itemsHeld[i] != "")
                    {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }
    /**/    
    /*
    GameManager.Cs - AddItem()
    NAME
           public void AddItem(string itemToAdd)
    SYNOPSIS
            Additem handles adding an item to the inventory. 
    DESCRIPTION
            This function loops through the inventory and searches for an empty space in the inventory.
            If found, it adds the item to the inventory after checking if the item exists in the
            inventory currently. If the item is not valid an error is thrown for me to try to figure out
            why it did not add.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void AddItem(string itemToAdd)
    {
        int newItemPosition = 0;
        bool foundSpace = false;
        //Looping through inventory and checking for spot to add it
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == "" || itemsHeld[i] == itemToAdd)
            {
                newItemPosition = i;
                i = itemsHeld.Length;
                foundSpace = true;
            }
        }
        //If a space is found then checking if the item exists befoe adding it just by
        //looping through the items again.. not the most optimal unsure what I was thinking at the time ha.
        if (foundSpace)
        {
            //checking i fitem exits before adding it
            bool itemExists = false;

            for (int i = 0; i  < referenceItems.Length; i++)
            {
                if (referenceItems[i].itemName == itemToAdd)
                {
                    itemExists = true;

                    i = referenceItems.Length;
                }
            }
            //Adding the item and increasing the #of items array to have accurate amt
            if (itemExists)
            {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            }
            else
            {
                Debug.LogError(itemToAdd + " doesn't exist");
            }
        }
        GameMenu.instance.ShowItems();
    }
    /**/    
    /*
    GameManager.Cs - RemoveItem()
    NAME
           public void RemoveItem(string itemToRemove)
    SYNOPSIS
            RemoveItem handles removing an item to the inventory. 
    DESCRIPTION
            This function runs similarly to AddItem. The inventory is looped through and the position of the item
            is stored if it is found. If it's found then the amount of the item is also updated in the 
            numberOfItems array, and if it is the last item being removed the slot in the inventory turns blank.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void RemoveItem(string itemToRemove)
    {
        bool foundItem = false;
        int itemPosition = 0;

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            if (itemsHeld[i] == itemToRemove)
            {
                foundItem = true;
                itemPosition = i;

                i = itemsHeld.Length;
            }
        }

        Debug.Log(foundItem);
        if (foundItem)
        {
            numberOfItems[itemPosition]--;
            if (numberOfItems[itemPosition] <= 0)
            {
                itemsHeld[itemPosition] = "";
                
            }
            GameMenu.instance.ShowItems();
        }
        else
        {
            Debug.LogError(itemToRemove + ": Problem removing item");
        }
    }
    /**/    
    /*
    GameManager.Cs - SaveData()
    NAME
           public void SaveData()
    SYNOPSIS
            SaveData handles saving player data.
    DESCRIPTION
            SaveData stores the player data I want stored (player stats,inventory, current scene,
            player position, player stats) into the Player Prefs via Key/Value pairings - 
            the key being the label of what I am storing
            and the value being whatever is currently stored in the game.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SaveData()
    {
        PlayerPrefs.SetString("Current_Scene", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        //save char info
        for (int i = 0; i < playerStats.Length; i++)
        {
            //getting active players
            if (playerStats[i].gameObject.activeInHierarchy)
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 1);
            }
            else
            {
                PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_active", 0);
            }

            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Level", playerStats[i].playerLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentExp", playerStats[i].currentEXP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentHP", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxHP", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_CurrentMP", playerStats[i].currentMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_MaxMP", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_Defence", playerStats[i].defence);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_WpnPwr", playerStats[i].wpnPwr);
            PlayerPrefs.SetInt("Player_" + playerStats[i].charName + "_ArmrPwr", playerStats[i].armrPwr);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedWpn", playerStats[i].equippedWpn);
            PlayerPrefs.SetString("Player_" + playerStats[i].charName + "_EquippedArmr", playerStats[i].equppedArmr);
        }

        //store inventory data
        for (int i = 0; i < itemsHeld.Length; i++)
        {
            PlayerPrefs.SetString("ItemInInventory_" +i, itemsHeld[i]);
            PlayerPrefs.SetInt("ItemAmount_" + i, numberOfItems[i]);
        }
    }
    /**/    
    /*
    GameManager.Cs - LoadData()
    NAME
           public void LoadData()
    SYNOPSIS
            LoadData handles saving player data.
    DESCRIPTION
            LoadData loads the player data if any is stored stored (player stats,inventory, current scene,
            player position, player stats) in the Player Prefs (which in Unity is stored in computer Registry)
            And retrieves everything via the key of the item and stores it into the player information. It is pretty 
            much doing the inverse of the saveData function.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void LoadData()
    {
        PlayerController.instance.transform.position = new Vector3(PlayerPrefs.GetFloat("Player_Position_x"), 
            PlayerPrefs.GetFloat("Player_Position_y"), 
            PlayerPrefs.GetFloat("Player_Position_z"));
        //Looping through player stats (for all players, not just 1. So Woody and Tom's stats are loaded,
        //not just toms.)
        for(int i = 0; i  < playerStats.Length; i++)
        {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_active") == 0)
            {
                playerStats[i].gameObject.SetActive(false);
            }
            else
            {
                playerStats[i].gameObject.SetActive(true);
            }

            playerStats[i].playerLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Level");
            playerStats[i].currentEXP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentExp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentHP");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxHP");
            playerStats[i].currentMP= PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_CurrentMP");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_MaxMP");
            playerStats[i].strength =  PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Strength");
            playerStats[i].defence =  PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_Defence");
            playerStats[i].wpnPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_WpnPwr");
            playerStats[i].armrPwr = PlayerPrefs.GetInt("Player_" + playerStats[i].charName + "_ArmrPwr");
            playerStats[i].equippedWpn = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedWpn");
            playerStats[i].equppedArmr = PlayerPrefs.GetString("Player_" + playerStats[i].charName + "_EquippedArmr");
        }

        for (int i = 0; i < itemsHeld.Length; i++)
        {
            itemsHeld[i] = PlayerPrefs.GetString("ItemInInventory_" + i);
            numberOfItems[i] = PlayerPrefs.GetInt("ItemAmount_" + i);
        }
    }
}
