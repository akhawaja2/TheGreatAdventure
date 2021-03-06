﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerController is how the player movement is handled.

public class PlayerController : MonoBehaviour
{
    //Our players rigidbody and moveSpeed
    public Rigidbody2D rigidBody;
    public float moveSpeed;
    //The players animations (when changing direction/moving)
    public Animator myAnim;
    //Making only one player to controll
    public static PlayerController instance;
    //Name of transitions
    public string areaTransitionName;

    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;


    public bool canMove = true;
    /**/    
    /*!
    PlayerController.Cs - Start()
    NAME
            void Start()
    SYNOPSIS
            Start is called before the first frame update
    DESCRIPTION
            The start function in this Class creates an instanceof the player if it exists. If the
            player is created and it is not equal to the instance (eg. duplicate) the duplicate is
            deleted.
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
        if (instance == null)
        {
            //Instance value set to player
            instance = this;
        }
        else
        {
            if (instance != this)
            {
                //removing any duplicates
                Destroy(gameObject);
            }
        }
        //If I don't have this when I switch scenes I lose the 
        //Game object (In this case the player)
        DontDestroyOnLoad(gameObject);
    }
    /**/    
    /*!
    PlayerController.Cs - Update()
    NAME
            public void Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            The Update function here checks if the player can move and sets their speed if they can.
            It also checks which direction the player is in and moves that direction.
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
        if (canMove)
        {
            rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
            
        }
        else
        {
            rigidBody.velocity = Vector2.zero;
        }

        myAnim.SetFloat("moveX", rigidBody.velocity.x);
        myAnim.SetFloat("moveY", rigidBody.velocity.y);

        //Checking if horizontal/verticla axis is equal to 1
        if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            if (canMove)
            {
                //Player is pressing in that direction
                myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
                myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
            }
           
        }

        //Just liked I used this to keep the camera inside the map bounds
        //I am using the same line of code for the player to keep him in bounds
        //Clamp: Takes a value and clamps it between two points 
        //(in this case bottomleft and topright)
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }

    /**/    
    /*!
    PlayerController.Cs - SetBounds()
    NAME
            public void SetBounds(Vector3 botLeft, Vector3 topRight)
    SYNOPSIS
            Setter for our bounds
    DESCRIPTION
            The SetBounds function is to ensure that the player model does not get clipped off
            when the player collides with the edge of the map.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SetBounds(Vector3 botLeft, Vector3 topRight)
    {
        //Adding the new vector so the player model does not get clipped off when reaching an end
        bottomLeftLimit = botLeft + new Vector3(.5f, 1f, 0f);
        topRightLimit = topRight + new Vector3(-.5f, -1f, 0f);
    }



}
