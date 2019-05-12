using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class handles the battle notifications - mainly the message the player gets if they cannot flee or no longer have enough mana
//for a magic move.

public class BattleNotification : MonoBehaviour
{
    public float awakeTime;
    private float awakeCounter;
    public Text theText;
    // Start is called before the first frame update
    void Start() {}

    /*!*/    
    /*!
    BattleNotification.cs - Update();
    NAME
          void Update()
    SYNOPSIS
           Update is called once per frame 
    DESCRIPTION
            The awake counter is checked and gradually decremented until it is at 0,
            at which the game object (the battle notification) disappears.

    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /*!*/
    
    void Update()
    {
        if (awakeCounter > 0)
        {
            awakeCounter -= Time.deltaTime;
            if (awakeCounter <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
    /*!*/    
    /*!
    Battlenotification.Cs - Activate()
    NAME
          public void Activate()
    SYNOPSIS
           Sets the notification window active in the unity scene.
    DESCRIPTION
            This function sets the notification window active in the unity scene while also setting the 
            awakeCounter to the awakeTime (which is set in the inspector).

    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /*!*/
    public void Activate()
    {
        gameObject.SetActive(true);
        awakeCounter = awakeTime;
    }
}
