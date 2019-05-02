using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class handles rewards given to player after battle (experience points, items, and any quests completed).

public class BattleReward : MonoBehaviour
{

    public static BattleReward instance;
    //Text to show rewards
    public Text xpText, itemText;
    //Control turnin screen on and off
    public GameObject rewardScreen;

    //An array of all the reward items we are giving to the player
    public string[] rewardItems;
    //How much exp the players get
    public int xpEarned;

    //If any quests need to be completed their names are stored and
    //the quests are marked as complete.
    public bool markQuestComplete;
    public string questToMark;
    // Start is called before the first frame update
    void Start()
    {
        //Since there's only one instance going to set it to this
        instance = this;
    }

    /**/    
    /*
    BattleReward.Cs - Update()
    NAME
          public void Update()
    SYNOPSIS
           Used for testing
    DESCRIPTION
            This update was used for testing - Whenever ingame and the Y key is pressed an iron and wooden sword are added
            to the inventory (if the function ran successfully).
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
        if(Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardsScreen(54, new string[] { "Iron Sword", "Wooden Sword" });
        }
    }
    /**/    
    /*
    BattleReward.Cs - OpenRewardsScreen()
    NAME
          public void OpenRewardsScreen(int xp, string[] rewards)
    SYNOPSIS
           How the rewards screen is opened for the player to view.
    DESCRIPTION
            This function opens the rewards screen and updates the text view for the player.

            EX: After killing an enemy, the player would see:
            "Everyone earned 100 xp!"
            "You got the following items:"
            "Iron Sword"
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void OpenRewardsScreen(int xp, string[] rewards)
    {
        xpEarned = xp;
        rewardItems = rewards;

        xpText.text = "Everyone earned " + xpEarned + " xp!";
        //clearing past item text
        itemText.text = "";

        //printing out the items earned
        for (int i = 0; i < rewardItems.Length; i++)
        {
            itemText.text += rewards[i] + "\n";
        }

        rewardScreen.SetActive(true);
    }
    /**/    
    /*
    BattleReward.Cs - CloseRewardScreen()
    NAME
          public void CloseRewardScreen()
    SYNOPSIS
           How the rewards screen is closed after the player views it.
    DESCRIPTION
            This function updates data after the player closes the rewards screen.
            Each player that is active in the scene has the experience points added,
            and the items are also looped through and added to the inventory.

            After that, the screen is closed and then the function exits the battle instance and
            marks the quest complete if there is any quest that was completed on defeat of an enemy.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void CloseRewardScreen()
    {
        for (int i = 0; i < GameManager.instance.playerStats.Length; i++)
        {
            //If the player is active and being used add xp
            if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
            {
                GameManager.instance.playerStats[i].AddExp(xpEarned); 
            }  
        }
        //Looping through the items and adding them to the player inventory
        for (int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i]);
        }
        rewardScreen.SetActive(false);
        GameManager.instance.battleActive = false;

        if (markQuestComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
    }
}
