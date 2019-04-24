using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestMarker : MonoBehaviour
{
    public string questToMark;
    public bool markComplete;
    public bool markOnEnter;
    private bool canMark;
    public bool deactivateOnMarking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canMark && Input.GetButton("Fire1"))
        {
            canMark = false;
            MarkQuest();
        }
    }
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
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canMark = false;
        }
    }
}
