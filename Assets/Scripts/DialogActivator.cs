using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class deals with what is said during dialog, 
//whether it should activate a quest and checks if the dialog
//is a person or a sign
public class DialogActivator : MonoBehaviour
{
    //Lines in the dialog
    public string[] lines;
    
    private bool canActivate;
    
    public bool isPerson = true;
    //variables checking quest status
    public bool shouldActivateQuest;
    public string questToMark;
    public bool markComplete;


    // Start is called before the first frame update
    void Start() {}
    /**/    
    /*
    DialogActivator.Cs - Update()
    NAME
           public void Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            If a dialog is allowed to be active and the player hits fire1 and 
            if the dialog is active in the hierarchy and activate the dialog and 
            if a quest has to be activated it is
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
        //if we click and our dialog box is not active in the heirarchy
        if (canActivate && Input.GetButtonDown("Fire1") && !DialogManager.instance.dialogBox.activeInHierarchy)
        {
            DialogManager.instance.ShowDialog(lines, isPerson);
            DialogManager.instance.ShouldActivateQuestAtEnd(questToMark, markComplete);
        }
    }
     /**/    
    /*
    DialogActivator.Cs - OnTriggerEnter2D()
    NAME
           void OnTriggerEnter2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) has entered a box where a dialog could be started
    DESCRIPTION
            This function checks if the player has entered a specific area where a dialog can be activated
            (close enough to an NPC) and if so canActivate is set true so if the player left clicks
            they can activate the dialog
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //activate dialog box
            canActivate = true;
        }
    }
    /**/    
    /*
    DialogActivator.Cs - OnTriggerExit2D()
    NAME
           void OnTriggerExit2D(Collider2D other)
    SYNOPSIS
            Checks if player (the other variable in function parameters) has exited a box where a dialog could be started
    DESCRIPTION
            This function checks if the player has exited  a specific area where a dialog is not able to  be activated
            (close enough to an NPC) and if so canActivate is set false  so if the player can no longer
            activate the dialog
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //deactivate dialog box
            canActivate = false;
        }
    }
}
