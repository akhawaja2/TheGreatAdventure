using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Loading scene is a script to aid in loading in Unity's PlayerPrefs
//into the Player Data so players can load from a previous save.
public class LoadingScene : MonoBehaviour
{
    public float waitToLoad;
    // Start is called before the first frame update
    void Start()  { }
    /**/    
    /*
    LoadingScene.Cs - Update()
    NAME
            void Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            This function runs every frame, decrementing wait to load until it hits 0. Once it hits 0,
            that means the game is done fading, so the SceneManager loads the scene and the
            LoadData and LoadQuestData are executed inside of our GameManager and
            QuestManager instances.

            Update is called once per frame so there is no need for a counter loop to decrement the time
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
        if(waitToLoad > 0)
        {
            waitToLoad -= Time.deltaTime;
            if(waitToLoad <= 0)
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("Current_Scene"));

                GameManager.instance.LoadData();
                QuestManager.instance.LoadQuestData();
            }
        }
    }
}
