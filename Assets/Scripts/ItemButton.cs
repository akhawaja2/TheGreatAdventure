using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Item button handles what happens when an items' button is pressed 
//Whethr it be in shops, inventories or in battle.
public class ItemButton : MonoBehaviour
{
    //The current buttons image
    public Image ButtonImage;
    //The current amount of the item we have
    public Text amountText;
    //button slot in the inventory:
    //0 1 2 3 4 5 6 7 8 9
    //10 11 12 13 14 15 16 17
    //etc. etc.
    public int buttonValue;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update(){}
    /**/    
    /*!
    ItemButton.Cs - Press()
    NAME
            public void Press()
    SYNOPSIS
            Press is called when the user clicks on an item button.
    DESCRIPTION
            This function, when the user presses any button dealing with an item checks the proper
            interface being accessed and selects the item for use in that specific scenario. 
            Ex. If the player is in the menu and clicks an item, this function is called which calls
            SelectItem in the GameMenu, which would update the item information in the users inventory.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void Press()
    {
        //if game menu is open do the next if statement
        if (GameMenu.instance.theMenu.activeInHierarchy)
        {
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                //If the button is not blank then we clal a function from GameMenu to set the item active and update
                //item name/description in the inventory
                GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));

            }
        }
        //if shop menu is open do the next if statement
        if (Shop.instance.shopMenu.activeInHierarchy)
        {
            //Checking if in buy or sell menu and calling the Shop instance
            //To call the appropriate function
            if (Shop.instance.buyMenu.activeInHierarchy)
            {
                Shop.instance.SelectBuyItem(GameManager.instance.GetItemDetails(Shop.instance.itemsForSale[buttonValue]));
            }

            if (Shop.instance.sellMenu.activeInHierarchy)
            {
                Shop.instance.SelectSellItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
        }
        //if battle menu is open do the next if statement
        if (BattleManager.instance.itemMenu.activeInHierarchy)
        {
            //Check if item is blank, if not do selectitembattle in the BattleManager.
            if (GameManager.instance.itemsHeld[buttonValue] != "")
            {
                BattleManager.instance.SelectItemBattle(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));
            }
                
        }
    }
}
