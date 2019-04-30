using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private bool battleActive;

    public GameObject battleScene;

    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;
    public List<BattleChar> activeBattlers = new List<BattleChar>();

    //cycle through activebattlers then resets to 0
    public int currentTurn;
    //while waiting for turn to end - whether it be from player or from enemy
    public bool turnWaiting;

    public GameObject uiButtonsHolder;

    //Here we are referencing the battle move object
    public BattleMove[] movesList;

    public GameObject enemyAttackEffect;

    public DamageNumber theDamageNumber;


    //to update player info during battle
    public Text[] playerName, playerHP, playerMP;

    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    public GameObject magicMenu;

    public BattleMagicSelect[] magicButtons;

    public BattleNotification battleNotice;

    public int chanceToFlee = 35;
    private bool fleeing;

    public GameObject itemMenu;

    public ItemButton[] itemButtonsToShow;
    public Item activeItemBattle;
    public Text itemNameBattle, itemDescriptionBattle, useButtonTextBattle;

    public string gameOverScene;

    public int rewardXP;
    public string[] rewardItems;

    //For boss battles player should not be able to run away
    //(Who can run away froma dragon)?
    public bool cannotFlee;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            BattleStart(new string[] { "BlueWizard", "Spider" }, false);
        }
        if (battleActive)
        {
            if (turnWaiting)
            {
                if (activeBattlers[currentTurn].isPlayer)
                {
                    uiButtonsHolder.SetActive(true);

                }
                else
                {
                    uiButtonsHolder.SetActive(false);
                    //enemy attacks here
                    //Now when its the enemy turn they will loop through what they are doing
                    StartCoroutine(EnemyMoveCo());
                }

            }
        }
        //incrementing turn
        if (Input.GetKeyDown(KeyCode.N))
        {
            NextTurn();
        }
    }
    public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    {
        if (!battleActive)
        {
            cannotFlee = setCannotFlee;
            battleActive = true;
            GameManager.instance.battleActive = true;
            transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
            battleScene.SetActive(true);
            AudioManager.instance.PlayBGM(0);

            for (int i = 0; i < playerPositions.Length;i++)
            {
                if (GameManager.instance.playerStats[i].gameObject.activeInHierarchy)
                {
                    for (int j = 0; j  < playerPrefabs.Length; j++)
                    {
                        if (playerPrefabs[j].charName == GameManager.instance.playerStats[i].charName)
                        {
                            BattleChar newPlayer = Instantiate(playerPrefabs[j], playerPositions[i].position, playerPositions[i].rotation);
                            //reference to player positions
                            newPlayer.transform.parent = playerPositions[i];
                            activeBattlers.Add(newPlayer);

                            //Getting all of the active player stats
                            CharStats thePlayer = GameManager.instance.playerStats[i];
                            activeBattlers[i].currentHP = thePlayer.currentHP;
                            activeBattlers[i].maxHP = thePlayer.maxHP;
                            activeBattlers[i].currentMP = thePlayer.maxMP;
                            activeBattlers[i].strength = thePlayer.strength;
                            activeBattlers[i].defence = thePlayer.defence;
                            activeBattlers[i].wpnPower = thePlayer.wpnPwr;
                            activeBattlers[i].armrPower = thePlayer.armrPwr;
                        }
                    }
                }
            }
            for (int i = 0; i < enemiesToSpawn.Length; i++)
            {
                if (enemiesToSpawn[i] != "")
                {
                    for (int j = 0; j < enemyPrefabs.Length; j++)
                    {
                        if (enemyPrefabs[j].charName == enemiesToSpawn[i])
                        {
                            BattleChar newEnemy = Instantiate(enemyPrefabs[j], enemyPositions[i].position, enemyPositions[i].rotation);
                            newEnemy.transform.parent = enemyPositions[i];
                            activeBattlers.Add(newEnemy);
                        }
                       
                    }
                }
            }
        }
        turnWaiting = true;
        currentTurn = Random.Range(0, activeBattlers.Count);
        UpdateUIStats();
    }

    public void NextTurn()
    {
        currentTurn++;
        if (currentTurn >= activeBattlers.Count)
        {
            currentTurn = 0;
        }

        turnWaiting = true;
        UpdateBattle();
        UpdateUIStats();
    }
    public void UpdateBattle()
    {
        //true by default because we check if any player/enemy has health then set it to false
        bool allEnemiesDead = true;
        bool allPlayersDead = true;

        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].currentHP < 0)
            {
                activeBattlers[i].currentHP = 0;
            }

            if (activeBattlers[i].currentHP == 0)
            {
                //handle dead battler
                if (activeBattlers[i].isPlayer)
                {
                    //Changing alive sprite to dead sprite
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].deadSprite;
                }
                else
                {
                    activeBattlers[i].EnemyFade();
                }
            }
            else
            {
                //check if enemy - check if dead
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
                    //Since it isn't dead let's keep the alive sprite
                    activeBattlers[i].theSprite.sprite = activeBattlers[i].aliveSprite;
                }
                else
                {
                    Debug.Log("Set all enemies dead to false");
                    allEnemiesDead = false;
                    Debug.Log(allEnemiesDead + ".");
                }
            }
        }
            //if all enemies are dead we win battle
            //if all players deasd its over for the players!
            if (allEnemiesDead || allPlayersDead)
            {
                Debug.Log(allEnemiesDead + " allplayers- " + allPlayersDead);
                if (allEnemiesDead)
                {
                        Debug.Log("VICTORY!");
                    //end battle in victory
                    StartCoroutine(EndBattleCo());
                }
                else
                {
                    StartCoroutine(GameOverCo());
                }
                /*battleScene.SetActive(false);
                GameManager.instance.battleActive = false;
                battleActive = false;*/
            }
            else
            {
            //Skipping players that have died in battle..
                while (activeBattlers[currentTurn].currentHP == 0)
            {
                currentTurn++;
                if (currentTurn >= activeBattlers.Count)
                {
                    currentTurn = 0;
                }
            }
            }
        
    }

    //IEnumerator is a coroutine is something that
    //happens outside normal order in unity
    //When we call it, it will start running and everything
    //else will run while this is in the background
    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }
    public void EnemyAttack()
    {
        //pick player to attack - loop through active battlers list and
        //check if player

        List<int> players = new List<int>();
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            //checking if current battler is a player AND alive
            if (activeBattlers[i].isPlayer && activeBattlers[i].currentHP > 0)
            {
                players.Add(i);
            }
        }
        //randomly selecting a player
        int selectedTarget = players[Random.Range(0, players.Count)];

        //selecting a move 
        int selectAttack = Random.Range(0, activeBattlers[currentTurn].movesAvailable.Length);
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            //If the move is available
            if (movesList[i].moveName == activeBattlers[currentTurn].movesAvailable[selectAttack])
            {
                //Sprite shows up on player
                movePower = movesList[i].movePower;
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);
    }

    public void DealDamage(int target, int movePower)
    {
        //The attackers power
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;
        
        
        //The targets defence
        float defPwr = activeBattlers[target].defence + activeBattlers[target].armrPower;

        //damage calculation
        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);

        int damageToGive = Mathf.RoundToInt(damageCalc);

        Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + " to " + activeBattlers[target].charName);

        activeBattlers[target].currentHP -= damageToGive;

        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);
        UpdateUIStats();
    }

    public void UpdateUIStats()
    {
        for (int i = 0; i < playerName.Length; i++)
        {
            if (activeBattlers.Count > i)
            {
                if(activeBattlers[i].isPlayer)
                {
                    BattleChar playerData = activeBattlers[i];
                    playerName[i].gameObject.SetActive(true);
                    playerName[i].text= playerData.name;
                    //clamping current hp/mp so we don't see negative numbers in the UI
                    playerHP[i].text = Mathf.Clamp( playerData.currentHP, 0, int.MaxValue) + "/" + playerData.maxHP;
                    playerMP[i].text = Mathf.Clamp(playerData.currentMP, 0, int.MaxValue) + "/" + playerData.maxMP;
                }
                else
                { 
                    playerName[i].gameObject.SetActive(false);
                }
            }
            else
            {
                playerName[i].gameObject.SetActive(false);
            }
        }
    }

    public void PlayerAttack(string moveName  , int selectedTarget)
    {
        int movePower = 0;
        for (int i = 0; i < movesList.Length; i++)
        {
            //If the move is available
            if (movesList[i].moveName == moveName)
            {
                //Sprite shows up on player
                movePower = movesList[i].movePower;
                Instantiate(movesList[i].theEffect, activeBattlers[selectedTarget].transform.position, activeBattlers[selectedTarget].transform.rotation);
            }
        }
        Instantiate(enemyAttackEffect, activeBattlers[currentTurn].transform.position, activeBattlers[currentTurn].transform.rotation);
        DealDamage(selectedTarget, movePower);

        //So you can't double click a button or do any funny business to get 2 moves in
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        

        NextTurn();
        
    }
    public void OpenTargetMenu(string moveName)
    {
        targetMenu.SetActive(true);

        List<int> Enemies = new List<int>();

        //creating a list of enemies
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (!activeBattlers[i].isPlayer)
            {
                Enemies.Add(i);
            }
        }

        for (int i = 0; i < targetButtons.Length; i++)
        {
            //Checking if there is an enemy we can check
            //AND if it is alive.
            if (Enemies.Count > i && activeBattlers[Enemies[i]].currentHP > 0)
            {
                targetButtons[i].gameObject.SetActive(true);
                targetButtons[i].moveName = moveName;
                targetButtons[i].activeBattlerTarget = Enemies[i];
                targetButtons[i].targetName.text = activeBattlers[Enemies[i]].charName;
            }
            else
            {
                targetButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);
        for (int i = 0; i < magicButtons.Length; i++)
        {
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                magicButtons[i].gameObject.SetActive(true);

                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];

                magicButtons[i].nameText.text = magicButtons[i].spellName;
                for (int j = 0; j < movesList.Length; j++)
                {
                    if (movesList[j].moveName == magicButtons[i].spellName)
                    {
                        magicButtons[i].spellCost = movesList[j].moveCost;
                        magicButtons[i].costText.text = magicButtons[i].spellCost.ToString();
                    }
                }
            }
            else
            {
                magicButtons[i].gameObject.SetActive(false);
            }
        }
    }

    public void Flee()
    {
        if (cannotFlee)
        {
            battleNotice.theText.text = "Cannot flee this battle!";
            battleNotice.Activate();

        }
        else
        {
            int fleeSuccess = Random.Range(0, 100);
            if (fleeSuccess < chanceToFlee)
            {
                fleeing = true;
                //End battle
                battleActive = false;
                battleScene.SetActive(false);

                StartCoroutine(EndBattleCo());
            }
            else
            {
                NextTurn();
                battleNotice.theText.text = "Could not run!";
                battleNotice.Activate();
            }
        }
    }
   
    public void OpenItemMenu()
    {
        itemMenu.SetActive(true);

        //SIMILAR TO OUR GAMEMENU SHOWITEMS Function, but here we 
        //1. check the itemsheld to see if there's an item
        //and instead of updating the players inventory we update the item window in battle
        for (int i = 0; i < GameMenu.instance.itemButtons.Length; i++)
        {
            BattleManager.instance.itemButtonsToShow[i].buttonValue = i;
            //If there's an item in that position
            if (GameManager.instance.itemsHeld[i] != "")
            {
                itemButtonsToShow[i].ButtonImage.gameObject.SetActive(true);
                //Calling item function in gamemanager, returning an item, going into item script and getting the sprite of the item
                itemButtonsToShow[i].ButtonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                //Setting amt of items
                itemButtonsToShow[i].amountText.text = GameManager.instance.numberOfItems[i].ToString();
            }
            else
            {
                itemButtonsToShow[i].ButtonImage.gameObject.SetActive(false);
                itemButtonsToShow[i].amountText.text = "";
            }
        }

    }
    public void SelectItemBattle(Item newItem)
    {
        activeItemBattle = newItem;

        if (activeItemBattle.isItem)
        {
            useButtonTextBattle.text = "Use";
        }

        if (activeItemBattle.isWeapon || activeItemBattle.isArmour)
        {
            useButtonTextBattle.text = "Equip";
        }

        itemNameBattle.text = activeItemBattle.itemName;
        itemDescriptionBattle.text = activeItemBattle.description;
    }

    public void UseItemBattle()
    {
        activeItemBattle.UseInBattle(currentTurn);
        itemMenu.SetActive(false);
        NextTurn();
    }

    public IEnumerator EndBattleCo()
    {
        battleActive = false;
        uiButtonsHolder.SetActive(false);
        targetMenu.SetActive(false);
        magicMenu.SetActive(false);
        itemMenu.SetActive(false);

        //wait half a second so our last enemy fades out
        yield return new WaitForSeconds(.5f);

        //Fade out screen
        UIFade.instance.FadeToBlack();

        //Wait for fade out to finish
        yield return new WaitForSeconds(1.5f);

        //Our battle has ended. anything that's happened in battle should persist (hp/mp potions used)
        for (int i = 0; i < activeBattlers.Count; i++)
        {
            if (activeBattlers[i].isPlayer)
            {
                for(int j = 0; j < GameManager.instance.playerStats.Length;j++)
                {
                    if (activeBattlers[i].charName == GameManager.instance.playerStats[j].charName)
                    { 
                        //Updating the stats
                        GameManager.instance.playerStats[j].currentHP = activeBattlers[i].currentHP;
                        GameManager.instance.playerStats[j].currentMP = activeBattlers[i].currentMP;
                    }
                }

            }
            Destroy(activeBattlers[i].gameObject);
        }

        //Fading to screen and clearing battle data
        UIFade.instance.FadeFromBlack();
        battleScene.SetActive(false);
        activeBattlers.Clear();
        currentTurn = 0;
        //GameManager.instance.battleActive = false;
        if (fleeing)
        {
            GameManager.instance.battleActive = false;
            fleeing = false;
        }
        else
        {
            BattleReward.instance.OpenRewardsScreen(rewardXP, rewardItems);
        }

        AudioManager.instance.PlayBGM(FindObjectOfType<CameraController>().musicToPlay);
    }

    public IEnumerator GameOverCo()
    {
        battleActive = false;
        UIFade.instance.FadeToBlack();
        //waiting for fade to complete
        yield return new WaitForSeconds(1.5f);
        battleScene.SetActive(false);
        //loading into game over scene
        SceneManager.LoadScene(gameOverScene);

    }

}


