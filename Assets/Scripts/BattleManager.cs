using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

            BattleStart(new string[] { "BlueWizard", "Spider" });
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
    public void BattleStart(string[] enemiesToSpawn)
    {
        if (!battleActive)
        { 
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
            }
            else
            {
                //check if enemy - check if dead
                if (activeBattlers[i].isPlayer)
                {
                    allPlayersDead = false;
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
                }
                else
                {
                    Debug.Log("DEFEAT!");
                    //failed battle
                }
                battleScene.SetActive(false);
                GameManager.instance.battleActive = false;
                battleActive = false;
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
}
