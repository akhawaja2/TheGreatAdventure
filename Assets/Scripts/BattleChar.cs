using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour
{
    //bool to check for player
    public bool isPlayer;
    //String for all the moves available during battle
    public string[] movesAvailable;

    //Character name/stats/whether or not they have died
    public string charName;
    public int currentHP, maxHP, currentMP, maxMP, strength, defence, wpnPower, armrPower;
    public bool hasDied;

    //Sprites to load if character dies or not
    public SpriteRenderer theSprite;
    public Sprite deadSprite, aliveSprite;

    //Fading screen after battle
    private bool shouldFade;
    public float fadeSpeed = 1f;
    // Start is called before the first frame update
    void Start(){}

    /**/
    /*
    BattleChar.cs --- Update()
    NAME
            Update() - In Unity,   Update is called once per frame
    SYNOPSIS
           Checking if the battle is over
    DESCRIPTION
           Every frame we check shouldFade - that means the battle's over. Once it becomes true, we begin to change the sprite color to move towards black,
           and once the sprite is fully transparent we set the game object to be false and remove it.
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
        if (shouldFade)
        {
            theSprite.color = new Color(Mathf.MoveTowards(theSprite.color.r, 1f, fadeSpeed *Time.deltaTime),
                Mathf.MoveTowards(theSprite.color.g, 0f, fadeSpeed * Time.deltaTime), 
                Mathf.MoveTowards(theSprite.color.b, 0f, fadeSpeed * Time.deltaTime),
                Mathf.MoveTowards(theSprite.color.a, 0f, fadeSpeed * Time.deltaTime));
            if ( theSprite.color.a == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    /**/
    /*
    BattleChar.cs --- EnemyFade()
    NAME
            EnemyFade() - Setting our shouldFade to true
    SYNOPSIS
            Setting our shouldFade variable to true
    DESCRIPTION
            Setting shouldFade to true so next time update gets to it we handle the battle ending.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void EnemyFade()
    {
        shouldFade = true;
    }
}
