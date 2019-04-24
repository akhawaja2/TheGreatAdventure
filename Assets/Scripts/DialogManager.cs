﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogManager : MonoBehaviour
{
    public Text dialogText;
    public Text nameText;
    public GameObject dialogBox;
    public GameObject nameBox;

    public string [] dialogLines;
    private bool justStarted;

    public int currentLine;

    public static DialogManager instance;

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

    // Update is called once per frame
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

    //1.ShowDialog gets called, box opens on screen
    //Player is holding down button.
    //Player releases button - we go to Update()
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

    public void CheckIfName()
    {
        if (dialogLines[currentLine].StartsWith("n-"))
        {
            //replacing the n- that starts dialog
            nameText.text = dialogLines[currentLine].Replace("n-", "");
            currentLine++;
        }
    }
    public void ShouldActivateQuestAtEnd(string questName, bool markComplete)
    {
        questToMark = questName;
        markQuestComplete = markComplete;
        shouldMarkQuest = true;
    }
}
