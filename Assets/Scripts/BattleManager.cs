using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BattleManager : MonoBehaviour
{
    //Only want 1 battlemanager so making it an instance
    public static BattleManager instance;

    //To check if we are currently in battle
    private bool battleActive;

    //The battlescene view we are using (could be a desrt one or a cave one or a snowy one)
    public GameObject battleScene;

    //The positions of the players & enemies on the screen
    public Transform[] playerPositions;
    public Transform[] enemyPositions;

    //The prefab information for the players and enemies
    public BattleChar[] playerPrefabs;
    public BattleChar[] enemyPrefabs;
    //A list of the active battlers
    public List<BattleChar> activeBattlers = new List<BattleChar>();

    //cycle through activebattlers then resets to 0
    public int currentTurn;
    //while waiting for turn to end - whether it be from player or from enemy
    public bool turnWaiting;

    //Holds the battle option buttons
    public GameObject uiButtonsHolder;

    //Here we are referencing the battle move object
    public BattleMove[] movesList;

    //The animation for the enemy attack
    public GameObject enemyAttackEffect;

    //The amount of damage we are doing
    public DamageNumber theDamageNumber;

    //to update all of the player info during battle
    public Text[] playerName, playerHP, playerMP;

    //Variables to deal with what targets we want to select
    public GameObject targetMenu;
    public BattleTargetButton[] targetButtons;

    //Variable to deal with the magic menu
    public GameObject magicMenu;
    public BattleMagicSelect[] magicButtons;

    //Variable to deal with notifications during battle
    //e.x. Out of mana or if player cannot flee
    public BattleNotification battleNotice;


    //Base odd of fleeing
    public int chanceToFlee = 35;
    private bool fleeing;

    //The item menu functionality 
    public GameObject itemMenu;
    public ItemButton[] itemButtonsToShow;
    public Item activeItemBattle;
    public Text itemNameBattle, itemDescriptionBattle, useButtonTextBattle;

    //Game over scene name if player botches the battle
    public string gameOverScene;

    //Rewards to give post-successful battle
    public int rewardXP;
    public string[] rewardItems;

    //For boss battles player should not be able to run away
    //(Who can run away froma dragon)?
    public bool cannotFlee;

    /**/
    /*
    BattleManager.cs --- Start()
    NAME
            Start() 
    SYNOPSIS
           // Start is called before the first frame update
    DESCRIPTION
            1. Set our instance to the current object
            2. Tell Unity not to destroy this object and preserve it even if we load a new scene
                    (having 2 battlemanagers gets quite messy with canvases and button inputs etc. etc)
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
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /**/
    /*
    BattleManager.cs --- Update()
    NAME
            Update() 
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            1. I used this function to test battles initially (So if I hit T a battle would start automatically)
            2. Otherwise I check if a battle is active
            3. if it is our turn I show the available button options for the user, otherwise I start a coroutine for the enemy move
            4. I also had testing for the N key  to go to the next active battlers turn
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

    /**/
    /*
    BattleManager.cs --- BattleStart()
    NAME
            public void BattleStart(string[] enemiesToSpawn, bool setCannotFlee)
    SYNOPSIS
            This function is in charge of handling the first initial turn of the battle
    DESCRIPTION
            BattleStart is how I start the battle I check if we are currently in a battle. If we aren't in a battle, I set our 
            flee variable to whether or not we are eligible to escape in this battle (which is only false during boss battles). We set
            battle active to be true, and then show our battle scene game object so the users can see the battle.

            Then we loop through our playerstats (which in this case is MAX 3), and  check if the player is an active player.
            If the play is active then a BattleChar is created - it's just an object used during the battle scene.
            We then add the player and their stats to our Active Battlers List, so we have a list of who is fighting in the battle. 
            Then we add our enemies to the active battlers list, which is dependant on what is in the battlemanager in Unity. 
            Then we set turn waiting to be true (so the UI does not appear) and then we randomize the turn and update the UI accordingly
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
  
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
/**/
    /*
    BattleManager.cs --- NextTurn()
    NAME
            NextTurn() 
    SYNOPSIS
            Moving on to the next turn in battle
    DESCRIPTION
            I increment the turn - If the turn number is greater then the amount of people in battle, I set it to zero
            because that means the battle phase is starting over at player 0. Then I set turn waiting to true incase it is
            not our turn and then update the battle & UiStats. 
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    BattleManager.cs --- UpdateBattle()
    NAME
            public void UpdateBattle()
    SYNOPSIS
            Updating our battle information every turn.
    DESCRIPTION
            I loop through the active battlers and check if any of them are dead. If they are, I update their sprite.
            For players, that means their sprite changes to them lying on the ground 'dead'. For enemies, that means they
            fade to black and then are removed from the scene.
            Then I check if all the players or enemies are dead. If all the players are dead, it's game over, and I give
            The user the option to load from save or quit the game. If all the enemies are dead, The user gets their rewards and is taken out of 
            battle. 
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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

    /**/    
    /*
    BattleManager.cs --- EnemyMoveCo()
    NAME
            public IEnumerator EnemyMoveCo()
    SYNOPSIS
            Updating our battle information every turn.
    DESCRIPTION
            IEnumerator is a coroutine-  something that
            happens outside normal order in unity
            When called, it will start running and everything
            else will run while this function is is in the background

            In summation, in this function turnwaiting is st to false and then we pause for a second,
            then run EnemyAttack() and while the enemy attack animation is ran the function pauses for another second before
            Going to the next turn.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    
    public IEnumerator EnemyMoveCo()
    {
        turnWaiting = false;
        yield return new WaitForSeconds(1f);
        EnemyAttack();
        yield return new WaitForSeconds(1f);
        NextTurn();
    }

     /**/    
    /*
    BattleManager.cs --- EnemyAttack()
    NAME
            public void EnemyAttack()
    SYNOPSIS
            How the enemy executes attacks on the player.
    DESCRIPTION
            This function gets  alist of players from our active battlers (can't be attacking their teammates!).
            And from there selects a player at random, selects an attack at random,
            and then shows the move sprite on the player's sprite (so it looks like they are being attacked) and then deal the damage
            To the player
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    BattleManager.cs --- DealDamage()
    NAME
           public void DealDamage(int target, int movePower)
    SYNOPSIS
            How damage is dealt to a battler
    DESCRIPTION
            This function calculates damage dealt to a battler. The algorithm for calculating damage is:
            damageGiven = (attack power / defencepower) * movePower * a random float between .9 & 1.1.
            Then the damage is displayed on the screen and the battlers health and the UI are updated.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void DealDamage(int target, int movePower)
    {
        //The attackers power
        float atkPwr = activeBattlers[currentTurn].strength + activeBattlers[currentTurn].wpnPower;
        
        
        //The targets defence
        float defPwr = activeBattlers[target].defence + activeBattlers[target].armrPower;

        //damage calculation
        float damageCalc = (atkPwr / defPwr) * movePower * Random.Range(.9f, 1.1f);

        int damageToGive = Mathf.RoundToInt(damageCalc);

        //Debug.Log(activeBattlers[currentTurn].charName + " is dealing " + damageCalc + " to " + activeBattlers[target].charName);

        //Updating hp after taking damage
        activeBattlers[target].currentHP -= damageToGive;

        //Creatign a DamageNumber object - the position is going to be a little above the player, the text being the amount of damage given.
        Instantiate(theDamageNumber, activeBattlers[target].transform.position, activeBattlers[target].transform.rotation).SetDamage(damageToGive);
        UpdateUIStats();
    }
    /**/    
    /*
    BattleManager.cs --- UpdateUIStats()
    NAME
           public void UpdateUIStats()
    SYNOPSIS
            Updating the UI stats.
    DESCRIPTION
            This function updates the UI stats by looping through all of the active players. The first few people in activebattlers will always be
            players, so I double check, and then set their gameobject to be active and update the text for their name, health, and Mana.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void UpdateUIStats()
    {
        for (int i = 0; i < playerName.Length; i++)
        {
            //Validating still in array - would bring problems if same # of players and enemies. 
            if (activeBattlers.Count > i)
            {
                if(activeBattlers[i].isPlayer)
                {
                    //battlechar reference to update UI
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
    /**/    
    /*
    BattleManager.cs --- PlayerAttack()
    NAME
          public void PlayerAttack(string moveName  , int selectedTarget)
    SYNOPSIS
            Dealing with player attack (what move and their target).
    DESCRIPTION
            This function checks if the selected movename is a valid list in the movelist, and then sets the movePower. Then like previously, the 
            move animation is instantiated and the attack effect is also created. After that, the damage is dealt, stats are updated, menu is hidden again and
            the next turn begins.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    BattleManager.cs --- OpenTargetMenu()
    NAME
          public void OpenTargetMenu(string moveName)
    SYNOPSIS
            Showing the player what targets they can deal damage to.
    DESCRIPTION
            This function sets the target UI to appear in the game, and gathers a list of enemies by checking if an activebattler
            is an enemy. After getting the list, the buttons text with the alive enemies are shown to the player.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
    /**/    
    /*
    BattleManager.cs --- OpenMagicMenu()
    NAME
          public void OpenMagicMenu()
    SYNOPSIS
            Showing the player what magic abilities are available to them.
    DESCRIPTION
            This function loops through the magic menu and adds buttons for every magical ability the user can use (set in the Unity Game engine).
            Then the name and the amount of mana that it costs is updated for the player to see in game.

    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void OpenMagicMenu()
    {
        magicMenu.SetActive(true);
        for (int i = 0; i < magicButtons.Length; i++)
        {
            //Checking if the current active battler has magic moves.
            if (activeBattlers[currentTurn].movesAvailable.Length > i)
            {
                //Setting the button to click to do the move to appear on the screen.
                magicButtons[i].gameObject.SetActive(true);

                //Setting the spellname 
                magicButtons[i].spellName = activeBattlers[currentTurn].movesAvailable[i];
                //Setting the text for the name
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
    /**/    
    /*
    BattleManager.cs --- Flee()
    NAME
          public void Flee()
    SYNOPSIS
            How the player can flee from a battle
    DESCRIPTION
            This function checks the cannotFlee bool, and then act accordingly. If the player can't flee,
            the battleNotice object is shown on the screen letting the player know they cannot flee. If they player can flee,
            the success number is calculated from 0-100. If the number is less then 35 (our flee rate), then the player can flee and the battle scene is 
            deactivated and the battleactive is set to false, and then the scene fades out. If the flee check fails, then the players turn is skipped and they
            are told that they cannot flee.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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
   /**/    
    /*
    BattleManager.cs --- OpenItemMenu()
    NAME
          public void OpenItemMenu()
    SYNOPSIS
            Shows the players their items during battle.
    DESCRIPTION
            This function shows the player the function menu, loops through all of our items and shows them to the player. The item menu shown here
            is the same as the inventory menu, but this one is specifically for use during battles.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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

    /**/    
    /*
    BattleManager.cs --- SelectItemBattle()
    NAME
          public void SelectItemBattle(Item newItem)
    SYNOPSIS
            How the player can see the description of an item and the name of what they select in battle/
    DESCRIPTION
            This function takes the selected item and updates the description and name of the item on the window for the player to see.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SelectItemBattle(Item newItem)
    {
        activeItemBattle = newItem;


        //Use is for potions, equip is for weapons (unless the player wants to use a potion as a weapon...)
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
    /**/    
    /*
    BattleManager.cs --- UseItemBattle()
    NAME
          public void UseItemBattle()
    SYNOPSIS
            How the player can use items during battle.
    DESCRIPTION
            This function calls the UseInBattle function (located in the Item script) for the current player (which is their currentTurn index in the ActiveBattlers list)
            and then goes on to the next turn
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void UseItemBattle()
    {
        activeItemBattle.UseInBattle(currentTurn);
        itemMenu.SetActive(false);
        NextTurn();
    }
    /**/    
    /*
    BattleManager.cs --- EndBattleCo()
    NAME
          public IEnumerator EndBattleCo()
    SYNOPSIS
            An Enumerator for ending the battle and showing the proper animations.
    DESCRIPTION
            This function sets every menu and our battleActive bool to false (so they aren't shown outside of battle), and then wait for the last 
            enemy to fade out before coming back into the function and fading to black - after fading to black the active players stats are updated 
            (since if a player lost hp in battle but got out of it alive their hp should still be the same as it was in battle).
            Then the battleChars are destroyed since they will no longer be needed. 
            After that the game fades from black to the current scene again and clears the list of active battlers.
            Then a check is done to see if the player fleed or won the battle - if they fled then they receive no rewards for the battle (the
            rwards which are set in the Unity Game Engine).
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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

/**/    
    /*
    BattleManager.cs --- GameOverCo()
    NAME
          public IEnumerator GameOverCo()
    SYNOPSIS
            An Enumerator for showing the user the gameover screen
    DESCRIPTION
            This function is called when a player loses a battle - the game fades to black while setting the battle active bool and the scene to false,
            and then loads the game over scene (which lets the player load the last save OR quit to the menu).
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
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


