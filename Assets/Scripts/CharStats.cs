using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CharStats is how the program keeps track of the character stats, weapons,
//armor, and their sprite 
public class CharStats : MonoBehaviour
{
    //Name/level/xp/xp to next level/maxlvl/base exp stored here
    public string charName;
    public int playerLevel = 1;
    public int currentEXP;
    public int[] expToNextLevel;
    public int maxLevel = 100;
    public int baseEXP = 1000;

    //Storing other stats
    public int currentHP;
    public int maxHP = 100;
    public int currentMP;
    public int maxMP = 30;
    public int[] mpLvlBonus = new int[100];
    public int strength;
    public int defence;
    public int wpnPwr;
    public int armrPwr;
    public string equippedWpn;
    public string equppedArmr;

    //variable to hold char sprite
    public Sprite charImage;
    
    /**/    
    /*!
    CharStats.Cs 
    NAME
          void Start()
    SYNOPSIS
           Start is called on the start of the object creation
    DESCRIPTION
            This function sets the exp rate for all of the levels and stores it
            inside of expToNextLevel. the equation is the previous level * 1.05
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
        //calculate xp
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseEXP;

        for (int i = 2; i < expToNextLevel.Length; i++)
        {
            expToNextLevel[i] = Mathf.FloorToInt(expToNextLevel[i - 1] * 1.05f);
        }
    }

    /**/    
    /*!
    CharStats.Cs 
    NAME
          void Update()
    SYNOPSIS
            This function was used for testing
    DESCRIPTION
            This function was used for testing - every time I hit k ingame 1000 exp
            should have been added to the player
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
        if (Input.GetKeyDown(KeyCode.K))
        {
            AddExp(1000);
        }
    }
    /**/    
    /*!
    CharStats.Cs - AddExp()
    NAME
           public void AddExp(int expToAdd)
    SYNOPSIS
            This function is how exp is added to the player
    DESCRIPTION
            This function is how exp is added to the player. It checks if the playerLevel is less 
            then the max level (100), and if it is it commences to add exp and reset the counter. If the player 
            levels up to an odd number, their defence si incremented - if even, their strength is incremented.
            Then their hp and mp is updated and replenished.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void AddExp(int expToAdd)
    {
        currentEXP += expToAdd;
        if (playerLevel < maxLevel)
        {
            //then we can do level up stuff
            if (currentEXP >= expToNextLevel[playerLevel]  && playerLevel < maxLevel)
            {
                currentEXP -= expToNextLevel[playerLevel];
                playerLevel++;
                //Determine whether to add to str/def based on odd or even
                //if even number we add str, odd defence

                if (playerLevel % 2 == 0)
                {
                    strength++;
                }
                else
                {
                    defence++;
                }
                //update HP 
                maxHP = Mathf.FloorToInt(maxHP * 1.05f);
                currentHP = maxHP;

                //update Mana points
                maxMP = maxMP + mpLvlBonus[playerLevel];
                currentMP = maxMP;
                
            }
        }
        if (playerLevel >= maxLevel)
        {
            currentEXP = 0;
        }
    }
}
