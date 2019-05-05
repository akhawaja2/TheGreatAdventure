using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//QuestManager stores all the information regarding quests
//and their completion
public class QuestManager : MonoBehaviour
{
    public string[] questMarkerNames;
    public bool[] questMarkersComplete;

    //Making it an instance - only want 1
    public static QuestManager instance;
    /**/    
    /*
    QuestManager.Cs - Start()
    NAME
            void Start()
    SYNOPSIS
            Start is called before the first frame update
    DESCRIPTION
            The start function in this Class creates an instance of a QuestManager and then creates
            a new array of questMarkersComplete that is the length of the quests string
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
        instance = this;
        questMarkersComplete = new bool[questMarkerNames.Length];
    }
    /**/    
    /*
    QuestManager.Cs - Update()
    NAME
            void Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            Update is used for testing - When the Keys are pressed quests are marked incomplete
            or are saved and loaded to PlayerPrefs
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log(CheckIfComplete("Test the Quest"));
            MarkQuestComplete("Test the Quest");
            MarkQuestIncomplete("Fight the demon");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveQuestData();
            Debug.Log("Saved game");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            LoadQuestData();

            Debug.Log("loaded game");
        }
    }
    /**/    
    /*
    QuestManager.Cs - GetQuestNumber()
    NAME
            void GetQuestNumber(string questToFind)
    SYNOPSIS
            GetQuestNumber gets the index of the quest in the array.
    DESCRIPTION
            GetQuestNumber loops through the quest names array and looks for our quest, if found
            returns the index. If not found returns 0 and sends an error to me in the console.
    RETURNS
            integer index of quest location.
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public int GetQuestNumber(string questToFind)
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            if (questMarkerNames[i] == questToFind)
            {
                return i;
            }
        }
        Debug.LogError("Quest" + questToFind + " does not exist");
        return 0;
    }

    /**/    
    /*
    QuestManager.Cs - CheckIfComplete()
    NAME
            public bool CheckIfComplete(string questToCheck)
    SYNOPSIS
            CheckIfComplete checks if a quest is complete
    DESCRIPTION
            CheckIfComplete calls GetQuestNumber at the name of the quest to Check and if it is not 0
            then returns the status of the quest. Otherwise it returns false.
    RETURNS
            true/false based on quest completion.
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public bool CheckIfComplete(string questToCheck)
    {
        if (GetQuestNumber(questToCheck) != 0)
        {
            return questMarkersComplete[GetQuestNumber(questToCheck)];
        }
        return false;
    }
    /**/        
    /*
    QuestManager.Cs - MarkQuestComplete()
    NAME
            public void MarkQuestComplete(string questToMark)
    SYNOPSIS
            MarkQuestComplete sets a quest in the bool array to complete
    DESCRIPTION
            MarkQuestComplete sets the questMarketsComplete @ the index of the quest to mark to true
            and then updates any local quest objects via UpdateLocalQuestObjects(incase an item
            has to be triggered as anything).
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void MarkQuestComplete(string questToMark)
    {
        questMarkersComplete[GetQuestNumber(questToMark)] = true;
        UpdateLocalQuestObjects();
    }
    /**/        
    /*
    QuestManager.Cs - MarkQuestIncomplete()
    NAME
             public void MarkQuestIncomplete(string questToMark)
    SYNOPSIS
            MarkQuestComplete sets a quest in the bool array to incomplete
    DESCRIPTION
            MarkQuestComplete sets the questMarketsComplete @ the index of the quest to mark to false
            and then updates any local quest objects via UpdateLocalQuestObjects(incase an item
            has to be triggered as anything).
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void MarkQuestIncomplete(string questToMark)
    {
        questMarkersComplete[GetQuestNumber(questToMark)] = false;
        UpdateLocalQuestObjects();
    }

    /**/        
    /*
    QuestManager.Cs - UpdateLocalQuestObjects()
    NAME
             public void UpdateLocalQuestObjects()
    SYNOPSIS
            UpdateLocalQuestObjects updates/moves any quest objects that are activated from
            a quest
    DESCRIPTION
            UpdateLocalQuestObjects makes an object array of quest objects and then checks whether
            or not they should be activated (ex. the lava in the game moves upon quest completion).
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void UpdateLocalQuestObjects()
    {
        QuestObjectActivator[] questObjects = FindObjectsOfType<QuestObjectActivator>();

        if (questObjects.Length > 0)
        {
            for (int i = 0; i < questObjects.Length; i++)
            {
                questObjects[i].CheckCompletion();
            }
        }
    }
    /**/        
    /*
    QuestManager.Cs - SaveQuestData()
    NAME
            public void SaveQuestData()
    SYNOPSIS
            SaveQuestData saves quest data
    DESCRIPTION
            SaveQuestData loops through all the quests and sets the quests to an integer based on 
            whether or not they are completed or not (1 for complete, 0 for incomplete).
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SaveQuestData()
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            if (questMarkersComplete[i])
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 1);
            }
            else
            {
                PlayerPrefs.SetInt("QuestMarker_" + questMarkerNames[i], 0);
            }
        }
    }
    /**/        
    /*
    QuestManager.Cs - LoadQuestData()
    NAME
            public void LoadQuestData()
    SYNOPSIS
            LoadQuestData loads quest data
    DESCRIPTION
            LoadQuestData loops through all the quests and finds all of the quests stored and
            then sets the games questMarkerNames and complete values to their associated index value.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void LoadQuestData()
    {
        for (int i = 0; i < questMarkerNames.Length; i++)
        {
            //Getting the quest data (complete or not)
            int valueToSet = 0;
            if ( PlayerPrefs.HasKey("QuestMarker_" + questMarkerNames[i]))
            {
                valueToSet = PlayerPrefs.GetInt("QuestMarker_" + questMarkerNames[i]);
            }

            //Setting the data here
            if (valueToSet == 0)
            {
                questMarkersComplete[i] = false;
            }
            else
            {
                questMarkersComplete[i] = true;
            }
        }
    }
}
