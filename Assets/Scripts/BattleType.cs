using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**/    
    /*
    BattleType.Cs 
    NAME
          public class BattleType 
    SYNOPSIS
           How I instantiate enemies in the game world
    DESCRIPTION
            The BattleType is a class used by classes in Unity.
            In the inspector, the different types and instances of enemies can be set
            within the inspector, as well as the rewards given upon completion.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/

//The [Serializable] attribute allows Unity to 
//delve into the members of the class, causing them to show up in the inspector.
[System.Serializable]
public class BattleType 
{
    public string[] enemies;
    public int rewardXP;
    public string[] rewardItems;

}
