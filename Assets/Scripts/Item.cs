using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Item class is how we create items and interact with them
//This data is set into the Unity Inspector.
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
    /**/    
    /*!
    Item.Cs - Use()
    NAME
            public void Use(int charToUseOn)
    SYNOPSIS
            Use is called when the user selects to use an item.
    DESCRIPTION
            This function gets the selected character based on the charToUseOn variable
            passed inside the function, then checks what type of item it is dealing with. 
            If the item is supposed to affect HP, it updates the HP value. If MP, it updates the
            MP value and if it affects Str it affects the Strength value. If weapon/armor it also
            updates that information. It then removes the item from the players inventory.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void Use(int charToUseOn)
    {
        CharStats selectedChar = GameManager.instance.playerStats[charToUseOn];
        //BattleChar selectedBattleChar = BattleManager.instance.activeBattlers[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedChar.currentHP += amountToChange;
                //selectedBattleChar.currentHP += amountToChange;
                if (selectedChar.currentHP > selectedChar.maxHP)
                {
                    selectedChar.currentHP = selectedChar.maxHP;
                    //selectedBattleChar.currentHP = selectedBattleChar.maxHP;
                }
            }
            if (affectMP)
            {
                selectedChar.currentMP += amountToChange;
                //selectedBattleChar.currentMP += amountToChange;
                if (selectedChar.currentMP > selectedChar.maxMP)
                {
                    selectedChar.currentMP = selectedChar.maxMP;
                   // selectedBattleChar.currentMP = selectedBattleChar.maxMP;
                }
            }
            if (affectStr)
            {
                selectedChar.strength += amountToChange;
                //selectedBattleChar.strength += amountToChange;
            }
        }

        if (isWeapon)
        {
            if (selectedChar.equippedWpn != "")
            {
                GameManager.instance.AddItem(selectedChar.equippedWpn);
            }
            selectedChar.equippedWpn = itemName;
            selectedChar.wpnPwr = weaponStrength;
        }

        if (isArmour)
        {
            if (selectedChar.equppedArmr != "")
            {
                GameManager.instance.AddItem(selectedChar.equppedArmr);
            }
            selectedChar.equppedArmr = itemName;
            selectedChar.armrPwr = armourStrength;
        }
        GameManager.instance.RemoveItem(itemName);
    }
     /**/    
    /*!
    Item.Cs - UseInBattle()
    NAME
            public void UseInBattle(int charToUseOn)
    SYNOPSIS
            UseInBattle is called when the user selects to use an item during battle.
    DESCRIPTION
            This function does the same thing as use with a few alterations - instead of affecting
            a player normally in the game world, it is making changes to a players BattleChar stats. It also
            currently does not allow for the player to equip armor or weapons during battle.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void UseInBattle(int charToUseOn)
    {
        BattleChar selectedBattleChar = BattleManager.instance.activeBattlers[charToUseOn];

        if (isItem)
        {
            if (affectHP)
            {
                selectedBattleChar.currentHP += amountToChange;
                //selectedBattleChar.currentHP += amountToChange;
                if (selectedBattleChar.currentHP > selectedBattleChar.maxHP)
                {
                    selectedBattleChar.currentHP = selectedBattleChar.maxHP;
                    //selectedBattleChar.currentHP = selectedBattleChar.maxHP;
                }
            }
            if (affectMP)
            {
                selectedBattleChar.currentMP += amountToChange;
                //selectedBattleChar.currentMP += amountToChange;
                if (selectedBattleChar.currentMP > selectedBattleChar.maxMP)
                {
                    selectedBattleChar.currentMP = selectedBattleChar.maxMP;
                    // selectedBattleChar.currentMP = selectedBattleChar.maxMP;
                }
            }
            if (affectStr)
            {
                selectedBattleChar.strength += amountToChange;
                //selectedBattleChar.strength += amountToChange;
            }
        }

        //Curently cant switch weapons or armor in battle
        /*if (isWeapon)
        {
            if (selectedBattleChar.wpn != "")
            {
                GameManager.instance.AddItem(selectedBattleChar.equippedWpn);
            }
            selectedBattleChar.equippedWpn = itemName;
            selectedBattleChar.wpnPwr = weaponStrength;
        }

        if (isArmour)
        {
            if (selectedChar.equppedArmr != "")
            {
                GameManager.instance.AddItem(selectedChar.equppedArmr);
            }
            selectedChar.equppedArmr = itemName;
            selectedChar.armrPwr = armourStrength;
        }*/
        GameManager.instance.RemoveItem(itemName);
    }
}
