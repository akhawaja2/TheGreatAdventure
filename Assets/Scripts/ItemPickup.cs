using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ItemPickup is how the game deals with items on the ground in the game,
//And what happens if the user decides to walk up to them and interact with it
public class ItemPickup : MonoBehaviour
{

    private bool canPickup;
    // Start is called before the first frame update
    void Start(){}
    /**/    
    /*
    ItemPickup.Cs - Update()
    NAME
            public void ItemPickup()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            This function checks if the User is interacting with a box collider (so canPickup would be true)
            and if they left click and are eligible to move (so not inside of a menu or a shop), the item is added
            to their inventory and then the object is destroyed from the world.
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
        //If player can pick it up, left click AND they are allowed to move
        //(Meaning not in a menu or combat) we can pickup
        if (canPickup && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove)
        {
            Debug.Log("Trying to pickup item...");
            GameManager.instance.AddItem(GetComponent<Item>().itemName);
            Destroy(gameObject);
        }

    }
    /**/    
    /*
    ItemPickup.Cs - OnTriggerEnter2D()
    NAME
          private void OnTriggerEnter2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) has entered a box where a an item could be picked up
    DESCRIPTION
            This function checks if the player has entered a specific area where an item pickup is available. 
            The area is decided by a game object in Unity (a box collider component), and if the 
            player enters the box collider then it is triggered to set canPickup to true.
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
        if(other.tag == "Player")
        {
            canPickup = true;
        }
    }
    /**/    
    /*
    ItemPickup.Cs - OnTriggerExit2D()
    NAME
          private void OnTriggerExit2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) 
            has exited an area where a an item could have been picked up
    DESCRIPTION
            This function checks if the player has exited a specific area where an item pickup was available. 
            The area is decided by a game object in Unity (a box collider component), and if the 
            player exits the box collider then it is triggered to set canPickup to false.
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
            canPickup = false;
        }
    }
}
