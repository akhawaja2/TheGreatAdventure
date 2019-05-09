using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//shop deals with player shopping in the game
public class Shop : MonoBehaviour
{
    //Making it na instance - only one shop should be active ingame
    public static Shop instance;

    //declaring the menus
    public GameObject shopMenu;
    public GameObject buyMenu;
    public GameObject sellMenu;

    public Text goldText;
    //Items for sale in the shop
    public string[] itemsForSale;
    //Array of 40 buttons set in Unity
    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    //Selected item and name/desription/value for both windows
    public Item selectedItem;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;
    /**/        
    /*
    Shop.Cs - Start()
    NAME
            void Start()
    SYNOPSIS
            Start is called before the first frame update
    DESCRIPTION
            Sets the shop instance to the current object
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
    Shop.Cs - Update()
    NAME
            void Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            This was used for testing - If I pressed k and a shop was not
            already open a shop opened.
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
        if (Input.GetKeyDown(KeyCode.K) && !shopMenu.activeInHierarchy)
        {
            OpenShop();
        }
    }
    /**/        
    /*
    Shop.Cs - OpenShop()
    NAME
            public void OpenShop()
    SYNOPSIS
            OpenShop opens the shop.
    DESCRIPTION
            OpenShop sets the shop object to true and starts with opening the buy menu,
            then setting the shop active in the game manager and the gold text
            to however much gold the player has.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();

        GameManager.instance.shopActive = true;
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }
    /**/        
    /*
    Shop.Cs - CloseShop()
    NAME
            public void CloseShop()
    SYNOPSIS
            CloseShop closes the shop.
    DESCRIPTION
            CloseShop sets the shop object to false and updates the 
            game manager shop active value
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void CloseShop()
    {
        shopMenu.SetActive(false);
        GameManager.instance.shopActive = false;
    }
    /**/        
    /*
    Shop.Cs - OpenBuyMenu()
    NAME
            public void OpenBuyMenu()
    SYNOPSIS
            OpenBuyMenu opens the buy menu.
    DESCRIPTION
            OpenBuyMenu sets the buy menu active, deactivates the sell menu if it is active in
            the game, and then loops through all of the buttons in the buy menu and sets them to the item
            that is set by me in Unity for the current shop being accessed.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void OpenBuyMenu()
    {
        //This is so that we don't see ItemNAme/Description text when we firs topen shops
        buyItemButtons[0].Press();
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for (int i = 0; i < buyItemButtons.Length; i++)
        {
            buyItemButtons[i].buttonValue = i;
            //If there's an item in that position
            if (itemsForSale[i] != "")
            {
                buyItemButtons[i].ButtonImage.gameObject.SetActive(true);
                //Calling item function in gamemanager, returning an item, going into item script and getting the sprite of the item
                buyItemButtons[i].ButtonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                //Setting amt of items
                buyItemButtons[i].amountText.text = "";
            }
            else
            {
                buyItemButtons[i].ButtonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }
    }
    /**/        
    /*
    Shop.Cs - OpenSellMenu()
    NAME
            public void OpenSellMenu()
    SYNOPSIS
            OpenSellMenu opens the sell menu.
    DESCRIPTION
            OpenSellMenu sets the sell menu active, deactivates the buy menu if it is active in
            the game, and then updates the GameManager instance to sort the items again in case there
            is any spaces or duplicates there.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void OpenSellMenu()
    {
        //This is so that we don't see ItemNAme/Description text when we firs topen shops
        buyItemButtons[0].Press();
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);
        
        GameManager.instance.SortItems();

        ShowSellItems();
    }
    /**/        
    /*
    Shop.Cs - ShowSellItems()
    NAME
            private void ShowSellItems()
    SYNOPSIS
            OpenSellMenu opens the sell menu.
    DESCRIPTION
            OpenSellMenu loops through all of the buttons in the shop and 
            and sets the shops buttons for items the player cna be sold to be equivalent
            to the ones in the players inventory.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    private void ShowSellItems()
    {
        for (int i = 0; i < sellItemButtons.Length; i++)
        {
            sellItemButtons[i].buttonValue = i;
            //If there's an item in that position
            if (GameManager.instance.itemsHeld[i] != "")
            {
                sellItemButtons[i].ButtonImage.gameObject.SetActive(true);
                //Calling item function in gamemanager, returning an item, going into item script and getting the sprite of the item
                sellItemButtons[i].ButtonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                //Setting amt of items
                sellItemButtons[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                sellItemButtons[i].ButtonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }
    }
    /**/        
    /*
    Shop.Cs - SelectBuyItem()
    NAME
            public void SelectBuyItem(Item selectedItem)
    SYNOPSIS
            SelectBuyItem updates the text and description of an item button selected
            by the player in the shop.
    DESCRIPTION
            SelectBuyItem sets the text, description, and value text fields in the game to
            the values of the selected item that is tied to each button containing an item value.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SelectBuyItem(Item sellItem)
    {
        selectedItem = sellItem;
        Debug.Log(selectedItem.itemName + "Costs " + selectedItem.value);
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "Value: " + selectedItem.value + "g";
    }
    /**/        
    /*
    Shop.Cs - SelectSellItem()
    NAME
            public void SelectSellItem(Item selectedItem)
    SYNOPSIS
            SelectSellItem updates the text and description of an item button selected
            by the player in the shop.
    DESCRIPTION
            SelectSellItem sets the value text fields in the game to
            the values of the selected item that is tied to each button containing an item value.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;
        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * .5f).ToString() + "g";
    }
    /**/        
    /*
    Shop.Cs - BuyItem()
    NAME
            public void BuyItem()
    SYNOPSIS
            BuyItem buys an item for a player.
    DESCRIPTION
            BuyItem checks if a selected item is valid, then checks if the player has gold and if so
            adds it to the players inventory  and updates the player's gold via the 
            Game manager's instance.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void BuyItem()
    {
        Debug.Log("Attempting to buy " + selectedItem.itemName + " For " + selectedItem.value);
        if (selectedItem != null)
        {
            if (GameManager.instance.currentGold >= selectedItem.value)
            {
                GameManager.instance.currentGold -= selectedItem.value;
                GameManager.instance.AddItem(selectedItem.itemName);
            }
        }
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }
    /**/        
    /*
    Shop.Cs - SellItem()
    NAME
            public void SellItem()
    SYNOPSIS
            SellItem sells an item for a player.
    DESCRIPTION
            SellItem checks if a selected item is valid, then adds the appropriate amount of gold
            to the players inventory and removes the item from the players inventory via the Game manager's 
            instance. It then updates the players gold count and re opens the sell menu to the updated view. 
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SellItem()
    {
        if (selectedItem != null)
        {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * .5f);
            GameManager.instance.RemoveItem(selectedItem.itemName);
        }
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
        ShowSellItems();
    }
}
