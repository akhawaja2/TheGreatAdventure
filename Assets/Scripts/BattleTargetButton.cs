using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//BattleTargetButton allows for us to run an attack when a player selects a target button to
//initiate an attack on

public class BattleTargetButton : MonoBehaviour
{
    public string moveName;
    public int activeBattlerTarget;
    public Text targetName;
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
    BattleTargetButton.Cs - Press()
    NAME
          void Press()
    SYNOPSIS
           How player attacks
    DESCRIPTION
            The Press function is set in the Unity game engine on the target 
            buttons which called the PlayerAttack function in the BattleManager
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
        BattleManager.instance.PlayerAttack(moveName, activeBattlerTarget);
    }
}
