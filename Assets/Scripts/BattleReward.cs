using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleReward : MonoBehaviour
{
    public static BattleReward instance;
    //Text to show rewards
    public Text xpText, itemText;
    //Control turnin screen on and off
    public GameObject rewardScreen;

    public string[] rewardItems;
    public int xpEarned;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            OpenRewardsScreen(54, new string[] { "Iron Sword", "Wooden Sword" });
        }
    }

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
        for (int i = 0; i < rewardItems.Length; i++)
        {
            GameManager.instance.AddItem(rewardItems[i]);
        }
        rewardScreen.SetActive(false);
        GameManager.instance.battleActive = false;
    }
}
