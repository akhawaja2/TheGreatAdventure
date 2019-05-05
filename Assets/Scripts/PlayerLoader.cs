using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//PlayerLoader handles the instantiation of a player into the game world.
public class PlayerLoader : MonoBehaviour
{
    public GameObject player;
    /**/    
    /*
    PlayerController.Cs - Start()
    NAME
            void Start()
    SYNOPSIS
            Start is called before the first frame update
    DESCRIPTION
            The start function in this Class creates an instance of a playercontroller if one does
            not exist.
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
        if(PlayerController.instance == null)
        {
            Instantiate(player);
        }
    }

    // Update is called once per frame
    void Update(){}
}
