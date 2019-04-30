using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStart : MonoBehaviour
{
    public BattleType[] potentialBattles;

    //activate straight away, on stay or when exiting
    public bool activateOnEnter, activateOnStay, activateOnExit;

    private bool inArea;

    public float timeBetweenBattles;
    private float betweenBattleCounter;

    public bool deactivateAfterStarting;

    //For boss battles so you can't flee vs. them
    public bool cannotFlee;

    public bool shouldCompleteQuest;
    public string questToComplete;
    // Start is called before the first frame update
    void Start()
    {
        betweenBattleCounter = Random.Range(timeBetweenBattles * .5f, timeBetweenBattles * 1.5f);
    }

    // Update is called once per frame
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
    
    public IEnumerator StartBattleCo()
    {
        //fading into battle
        UIFade.instance.FadeToBlack();
        GameManager.instance.battleActive = true;

        int selectedBattle = Random.Range(0, potentialBattles.Length);

        BattleManager.instance.rewardItems = potentialBattles[selectedBattle].rewardItems;
        BattleManager.instance.rewardXP = potentialBattles[selectedBattle].rewardXP;

        //waiting for screen to finish waiting
        yield return new WaitForSeconds(1.5f);

        BattleManager.instance.BattleStart(potentialBattles[selectedBattle].enemies, cannotFlee);

        UIFade.instance.FadeFromBlack();
        if (deactivateAfterStarting)
        {
            gameObject.SetActive(false);
        }

        BattleReward.instance.markQuestComplete = shouldCompleteQuest;
        BattleReward.instance.questToMark = questToComplete;
    }
}
