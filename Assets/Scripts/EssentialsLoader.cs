using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Essentials loader is in charge of loading all of the necessary
//Game mechanics when a scene if loaded. It checks if a scene contains aspects
//such as audio/player/game manager/battle manager and if i tdoes not it will load a 
//fresh one. The essentials loader is also a prefab in Unity containing all of the objects.
public class EssentialsLoader : MonoBehaviour
{
    //Objects to chekto load in
    public GameObject UIScreen;
    public GameObject player;
    public GameObject gameMan;
    public GameObject audioMan;
    public GameObject battleMan;

    /**/    
    /*!
    EssentialsLoader.Cs - Start()
    NAME
           void  Start()
    SYNOPSIS
            Start is called before the first frame update
    DESCRIPTION
            This function runs a bunch of if statements to check if our canvas, playercontroller,
            gamemanager, audiomanager or battlemanager have been instantiated and are in our game. If
            they are not then they are then created. 
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
        if (UIFade.instance == null)
        {
            UIFade.instance = Instantiate(UIScreen).GetComponent<UIFade>();
        }
        if (PlayerController.instance == null)
        {
            PlayerController clone = Instantiate(player).GetComponent<PlayerController>();
            PlayerController.instance = clone;
        }
        if (GameManager.instance == null)
        {
            GameManager.instance = Instantiate(gameMan).GetComponent<GameManager>();
        }
        if (AudioManager.instance == null)
        {
            AudioManager.instance = Instantiate(audioMan).GetComponent<AudioManager>();
        }
        if (BattleManager.instance == null)
        {
            BattleManager.instance = Instantiate(battleMan).GetComponent<BattleManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
