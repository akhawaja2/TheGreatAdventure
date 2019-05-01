using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    //name of the transition
    public string transitionName;
    


/**/
/*
AreaEntrance.cs --- Start()
NAME
        Start() - Function is ran whenever I have exited one scene and am entering another
SYNOPSIS
        Where our character will end up entering from when changing scenes
DESCRIPTION
        N/A
RETURNS
        N/A
AUTHOR
        Abu Khawaja
DATE
        4/30/2019
*/
/**/
// Start is called before the first frame update
    void Start()
    {
        //If the transition name is equal to where the player is transitioning to we change positions
        if ( transitionName == PlayerController.instance.areaTransitionName)
        {
            Debug.Log("Hey I entered the areaEntrance start function");
            PlayerController.instance.transform.position = transform.position;
        }

        //Fading from black to the scene
        UIFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;
    }

    //I'm scared to delete this in case it breaks the program
    // Update is called once per frame
    void Update(){}
}
