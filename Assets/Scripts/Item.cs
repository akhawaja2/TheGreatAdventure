using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmour;
    public string itemName;
    public string description;
    public int value;
    //item sprite
    public Sprite itemSprite;

    //Formats the Unity Inspector to be more readable
    [Header("Item Details")]
    //If it's giving health - how much health is it giving me?
    //if giving str/def? How much? that is what amounttoChange holds
    public int amountToChange;
    //What kind of affect the itme will have on the player
    public bool affectHP, affectMP, affectStr;


    [Header("Weapon/Armour Details")]
    public int weaponStrength;
    public int armourStrength;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
