﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName;
    // Start is called before the first frame update
    void Start()
    {
        if ( transitionName == PlayerController.instance.areaTransitionName)
        {
            Debug.Log("Hey I entered the areaEntrance start function");
            PlayerController.instance.transform.position = transform.position;
        }

        //Fading from black to the scene
        UIFade.instance.FadeFromBlack();
        GameManager.instance.fadingBetweenAreas = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
