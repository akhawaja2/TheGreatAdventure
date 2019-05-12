using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Shopkeeper handles the shop keeper who is in charge of the shop
//so the player can interact with them to view a shop
public class Shopkeeper : MonoBehaviour
{

    private bool canOpen;
    public string[] itemsForSale = new string[40];
    // Start is called before the first frame update
    void Start(){}

    /**/        
    /*!
    Shopkeeper.Cs - SelectBuyItem()
    NAME
            public void Update( )
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            Update checks if a shop is eligible to be opened, if the user left clicks and if 
            they can move and if a shop is already not open and if the following is all true then 
            the shops items are set to the current items and shop instance's openshop function is called
            to show the player the buy menu.
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
        if(canOpen && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove && !Shop.instance.shopMenu.activeInHierarchy)
        {
            //Whatever my shop keeper is selling is equal to this instance of the shop
            Shop.instance.itemsForSale = itemsForSale;
            Shop.instance.OpenShop();
        }
        
    }
    /**/        
    /*!
    QuestMarker.Cs - OnTriggerEnter2D()
    NAME
            private void OnTriggerEnter2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) has entered a box 
            where a shop can be opened.
    DESCRIPTION
            This function checks if the player has entered a specific area where a shop can be opened.
            The area is decided by a game object in Unity (a box collider component), and if the 
            player enters the box collider then it is triggered to set canOpen to true.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = true;
        }


    }
        /**/        
    /*!
    QuestMarker.Cs - OnTriggerExit2D()
    NAME
            private void OnTriggerExit2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) has exited a box
            where a shop can be opened.
    DESCRIPTION
            This function checks if the player has exited a specific area where a shop can be opened.
            The area is decided by a game object in Unity (a box collider component), and if the 
            player enters the box collider then it is triggered to set canOpen to false.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canOpen = false;
        }

    }
}
