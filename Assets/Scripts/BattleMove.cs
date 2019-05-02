using System.Collections;
using System.Collections.Generic;
using UnityEngine;



    /**/    
    /*
    BattleMove.cs 
    NAME
          BattleMove class
    SYNOPSIS
            An Enumerator for showing the user the gameover screen
    DESCRIPTION
            We can't add this function into Unity (not extending from monobehaviour) 
            But we can call reference it in BattleManager. [System.Serializable] allows for the class
            to be used in the Unity inspector to attach a class with sub properties - in this case, a moves name,
            power, and animation effect.

    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/

[System.Serializable]
public class BattleMove
{
    //just using it for these variables. dont need it to exist in the world

    public string moveName;
    public int movePower;
    public int moveCost;
    //visual sprite
    public AttackEffect theEffect;
    //big list of all the moves we can do
    // Start is called before the first frame update
}
