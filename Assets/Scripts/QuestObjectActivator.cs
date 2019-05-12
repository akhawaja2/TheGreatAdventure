using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This function activates an item based on quest completion
public class QuestObjectActivator : MonoBehaviour
{
    public GameObject objectToActivate;

    public string questToCheck;
    public bool activeIfComplete;

    private bool intitialCheckDone;
    // Start is called before the first frame update
    void Start() {}

    /**/        
    /*!
    QuestObjectActivator.Cs - Update()
    NAME
            void Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            Update checs for initialcheckDones value and it is not done it is set to true and
            quest completion is checked
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
        if (!intitialCheckDone)
        {
            intitialCheckDone = true;
            CheckCompletion();
        }
    }
    /**/        
    /*!
    QuestObjectActivator.Cs - CheckCompletion()
    NAME
            public void CheckCompletion()
    SYNOPSIS
            CheckCompletion checks if a quest to check if completed to activate an item.
    DESCRIPTION
            checks if a quest to check if completed to activate an item. if So, the object to 
            active is shown in the Unity game scene
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void CheckCompletion()
    {
        if (QuestManager.instance.CheckIfComplete(questToCheck))
        {
            objectToActivate.SetActive(activeIfComplete);
        }
    }
}
