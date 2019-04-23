using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{

    private bool canPickup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //If player can pick it up, left click AND they are allowed to move
        //(Meaning not in a menu or combat) we can pickup
        if (canPickup && Input.GetButtonDown("Fire1") && PlayerController.instance.canMove)
        {
            Debug.Log("Trying to pickup item...");
            GameManager.instance.AddItem(GetComponent<Item>().itemName);
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canPickup = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canPickup = false;
        }
    }
}
