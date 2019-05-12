using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//DialogManager handles the dialog information in the Unity inspector.
public class DialogManager : MonoBehaviour
{
    //The objects appearing to the player
    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;

    //Dialog lines (added in unity inspector)
    public string [] dialogLines;
    private bool justStarted;

    public int currentLine;

    //Making it an instance so only 1 object can be made
    public static DialogManager instance;

    //Handling quest information
    private string questToMark;
    private bool markQuestComplete;
    private bool shouldMarkQuest;

    // Start is called before the first frame update
    void Start()
    {
        //Sending dialog lines to our game object
        //dialogText.text = dialogLines[currentLine];
        instance = this;
    }
    /**/    
    /*!
    DialogManager.Cs - Update()
    NAME
           void  Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            This function checks if the user left clicks and if so initiates dialog
            and checks whether or not it starts a quest or if it is a person discussing (in which
            case the name is shown)
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
        //equivalent to if left click is released
        if (Input.GetButtonUp("Fire1"))
        {
            if (!justStarted)
            {
                //Going to next line of text
                currentLine++;

                if (currentLine >= dialogLines.Length)
                {
                    //turning off dialogbox
                    dialogBox.SetActive(false);
                    GameManager.instance.dialogActive = false;

                    //if we shoul dmake a quest then we have o activae it
                    if(shouldMarkQuest)
                    {
                        shouldMarkQuest = false;
                        if(markQuestComplete)
                        {
                            QuestManager.instance.MarkQuestComplete(questToMark);
                        }
                        else
                        {
                            QuestManager.instance.MarkQuestIncomplete(questToMark);
                        }
                    }
                }
                else
                {
                    CheckIfName();
                    //showing next line of text
                    dialogText.text = dialogLines[currentLine];
                }
            }
            else
            {
                justStarted = false;
            }
            
            
        }
    }
    /**/    
    /*!
    DialogManager.Cs - ShowDialog()
    NAME
           public void ShowDialog(string[] newLines, bool isPerson)
    SYNOPSIS
            how the dialog is shown to the player
    DESCRIPTION
             //1.ShowDialog gets called, box opens on screen
            //Player is holding down button.
            //Player releases button -  go to Update()
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
   
    public void ShowDialog(string[] newLines, bool isPerson)
    {
        dialogLines = newLines;

        currentLine = 0;

        CheckIfName();

        dialogText.text = dialogLines[currentLine];
        dialogBox.SetActive(true);
        justStarted = true;

        nameBox.SetActive(isPerson);

        GameManager.instance.dialogActive= true;
    }
    /**/    
    /*!
    DialogManager.Cs - CheckIfName()
    NAME
           public void CheckIfName( )
    SYNOPSIS
            Checking if a dialog line is a name and showing it on the game screen
    DESCRIPTION
            This function checks if a dialog line starts with 'n-'. If so,
            a player is speaking so the name text is replaced and the line is incremented
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void CheckIfName()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            //replacing the n- that starts dialog
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }
    /**/    
    /*!
    DialogManager.Cs - ShouldActivateQuestAtEnd()
    NAME
           public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    SYNOPSIS
            Checking if a question should be activated at the end of dialog.
    DESCRIPTION
            Checking if a question should be activated at the end of dialog.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;
        shouldMarkQuest = true;
    }
}
