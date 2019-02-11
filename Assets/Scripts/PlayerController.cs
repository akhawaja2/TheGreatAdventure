using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float moveSpeed;

    public Animator myAnim;

    public static PlayerController instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            //Instance value set to player
            instance = this;
        }
        else
        {
            //removing any duplicates
            Destroy(gameObject);
        }
        //If I don't have this when I switch scenes I lose the 
        //Game object (In this case the player)
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        rigidBody.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;
        myAnim.SetFloat("moveX", rigidBody.velocity.x);
        myAnim.SetFloat("moveY", rigidBody.velocity.y);

        //Checking if horizontal/verticla axis is equal to 1
        if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1 || Input.GetAxisRaw("Vertical") == 1 || Input.GetAxisRaw("Vertical") == -1)
        {
            //Player is pressing in that direction
            myAnim.SetFloat("lastMoveX", Input.GetAxisRaw("Horizontal"));
            myAnim.SetFloat("lastMoveY", Input.GetAxisRaw("Vertical"));
        }
    }
}
