using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleMagicSelect : MonoBehaviour
{
    public string spellName;
    public int spellCost;
    public Text nameText;
    public Text costText;
    // Start is called before the first frame update
    void Start() {     }

    // Update is called once per frame
    void Update(){}

    /**/
    /*!
    BattleMagicSelect.cs --- Press()
    NAME
            Press() 
    SYNOPSIS
            How we handle when the user selects a spell to use in combat
    DESCRIPTION
            1. Check if the user has Mana.
            2. If they do, we close out the magic menu and then open the target menu
                And subtract the mana cost from the users mana points
            3. If they do no thave the Mana we let the user know they do not have enough 
                And then close the magic menu.
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
        if (BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP >= spellCost)
        {
            BattleManager.instance.magicMenu.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);

            BattleManager.instance.activeBattlers[BattleManager.instance.currentTurn].currentMP -= spellCost;
        }
        else
        {
            BattleManager.instance.battleNotice.theText.text = "Not enough MP!";
            BattleManager.instance.battleNotice.Activate();
            BattleManager.instance.magicMenu.SetActive(false);
        }
        
    }
}
