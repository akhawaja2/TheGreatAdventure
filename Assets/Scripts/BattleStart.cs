using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class handles the start of battles - whether you are activating a battle by
//Leaving a box collider, staying inside a box collider or exiting one.
public class BattleStart : MonoBehaviour
{
    public BattleType[] potentialBattles;

    //activate straight away, on stay or when exiting
    public bool activateOnEnter, activateOnStay, activateOnExit;

    //To check if player is in box collider
    private bool inArea;


    //For battles that require the player to be in an area for battle -
    //A good game doesn't have a player exit battle just to walk one step
    //And re-enter it!
    public float timeBetweenBattles;
    private float betweenBattleCounter;

    //Deactivating the box collider for boss battles so they can walk around in it
    //Without fighting the boss again
    public bool deactivateAfterStarting;

    //For boss battles so you can't flee vs. them
    public bool cannotFlee;

    //Whether or not a quest should be completed after battle - and
    //The name of the quest being completed
    public bool shouldCompleteQuest;
    public string questToComplete;

    /**/    
    /*!
    BattleStart.Cs - CloseRewardScreen()
    NAME
          void Start()
    SYNOPSIS
           Randomizes the battle between counter
    DESCRIPTION
            Randomizes the "time ticking" down until a random battle is started 
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
        betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
    }

    /**/    
    /*!
    BattleStart.Cs - Update()
    NAME
          void Update()
    SYNOPSIS
            Checks if player is eligible for a random battle.
    DESCRIPTION
            This function checks if the player is in an area eligible for a battle to start. If they can move,
            the betweenBAttleCounter timer is decremented and then every frame it checks if it is equal to 0.
            If it is, it means it is time for the player to get to battle, and a coroutine is started for battle! 
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
        //if player is in area
        if (inArea && PlayerController.instance.canMove)
        {
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                //start timer to count time between random battles
                betweenBattleCounter -= Time.deltaTime;
            }
            if (betweenBattleCounter <= 0)
            {
                betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
                StartCoroutine(StartBattleCo());
            }
        }
    }
    /**/    
    /*!
    BattleStart.Cs - OnTriggerEnter2D()
    NAME
          private void OnTriggerEnter2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) has entered a box where a battle could be started
    DESCRIPTION
            This function checks if the player has entered a specific area where a battle could begin. The area is decided by a 
            game object in Unity (a box collider component), and if the activateOnEnter is true then a battle is started immediately (eg. boss battles). 
            Otherwise, if the player walks into it but there is no start on enter the inArea is marked true (for battle instances where
            the player exits or a random battle inside the area).
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
        if (other.tag == "Player")
        {
            
            if (activateOnEnter)
            {
                StartCoroutine(StartBattleCo());
                //immediately start battle
            }
            else
            {
                inArea = true;
            }

        }
    }
    /**/    
    /*!
    BattleStart.Cs - OnTriggerExit2D()
    NAME
          private void OnTriggerExit2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) has entered a box where a battle could be started
    DESCRIPTION
            This function checks if the player has exited a specific area where a battle could begin. The area is decided by a 
            game object in Unity (a box collider component), and if the activateOnEnter is true then a battle is started immediately (eg. boss battles). 
            Otherwise, if the player walks into it but there is no start on enter the inArea is marked false (for battle instances where
            the player exits or a random battle inside the area).
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
            if (activateOnExit)
            {
                //activate when leaving
                StartCoroutine(StartBattleCo());
            }
            else
            {
                inArea = false;
            }
            
        }
    }
    /**/    
    /*!
    BattleStart.Cs - StartBattleCo()
    NAME
          public IEnumerator StartBattleCo()
    SYNOPSIS
            Starts a battle
    DESCRIPTION
            This function starts a battle by first fading into the battle scene. After that yield is ran the Battle is started and the function
            fades from blank and in the background if deactivateAfterStarting is true the function hides the (usually boss NPC )object from the users view. 
            Then the BattleReward quest information (set in Unity) is updated to whatever was set here. 
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public IEnumerator StartBattleCo()
    {
        //fading into battle
        UIFade.instance.FadeToBlack();
        GameManager.instance.battleActive = true;

        int selectedBattle = Random.Range(0, potentialBattles.Length);

        //Setting rewards
        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].rewardXP;

        //waiting for screen to finish waiting
        yield return new WaitForSeconds(1.5f);

        //Starting a battle after fading
        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cannotFlee);

        UIFade.instance.FadeFromBlack();
        //Hiding the game object(ex. in the cave the dragon sprite disappears from the map so it can't be interacted with
        //more then once)
        if (deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }
        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = questToComplete;
    }
}
