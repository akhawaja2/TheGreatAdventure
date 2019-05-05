using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//QuestMarker handles the marking of completion of quests 
//And whether or not an object is activated if a boxcollider is entered.
public class QuestMarker : MonoBehaviour
{
    public string questToMark;
    public bool markComplete;
    public bool markOnEnter;
    private bool canMark;
    public bool deactivateOnMarking;
    // Start is called before the first frame update
    void Start()
    {  }

    /**/        
    /*
    QuestMarker.Cs - Update()
    NAME
            void Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            Update is for when an object can be activated with left click - if in the quest area
            and the player left clicks the quest is marked complete.
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
        if (canMark && Input.GetButton("Fire1"))
        {
            canMark = false;
            MarkQuest();
        }
    }

    /**/        
    /*
    QuestMarker.Cs - MarkQuest()
    NAME
            void MarkQuest()
    SYNOPSIS
            MarkQuest marks a quest as complete or incomplete
    DESCRIPTION
            Based on the value of markComplete, MarkQuest calls the quest manager instance and marks
            the quest accordingly.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void MarkQuest()
    {
        if (markComplete)
        {
            QuestManager.instance.MarkQuestComplete(questToMark);
        }
        else
        {
            QuestManager.instance.MarkQuestIncomplete(questToMark);
        }
        //if we mark as true we want the object to be false - so whatever
        //opposite of deactivateonmarking is
        gameObject.SetActive(!deactivateOnMarking);
    }
    /**/        
    /*
    QuestMarker.Cs - OnTriggerEnter2D()
    NAME
            private void OnTriggerEnter2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) has entered a box 
            where a quest could be completed.
    DESCRIPTION
            This function checks if the player has entered a specific area where quest could be 
            marked as complete. 
            The area is decided by a game object in Unity (a box collider component), and if the 
            player enters the box collider then it is triggered to set markPickup to true.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //when we first enter the area we decide if we mark quest as true or not
            if(markOnEnter == true)
            {
                MarkQuest();
            }
            else
            {
                canMark = true;
            }
            
        }
    }

    /**/        
    /*
    QuestMarker.Cs - OnTriggerExit2D()
    NAME
            private void OnTriggerExit2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) has exited a box 
            where a quest could be completed.
    DESCRIPTION
            This function checks if the player has exited a specific area where quest could be 
            marked as incomplete. 
            The area is decided by a game object in Unity (a box collider component), and if the 
            player enters the box collider then it is triggered to set markPickup to false.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canMark = false;
        }
    }
}
