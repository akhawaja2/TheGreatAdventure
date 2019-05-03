using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//GameOver is a class in charge of showing the user their options
//If they die in battle - They can either load the last save
//Or go to the main menu.
public class GameOver : MonoBehaviour
{
    //names of scenes to load
    public string mainMenuScene;
    public string loadGameScene;
    // Start is called before the first frame update
    void Start()
    {
       /*AudioManager.instance.PlayBGM(4);
       PlayerController.instance.gameObject.SetActive(false);
       GameMenu.instance.gameObject.SetActive(false);
        BattleManager.instance.gameObject.SetActive(false);*/
    }
    
    void Update(){}
    /**/    
    /*
    GameOver.Cs - QuitToMain()
    NAME
            public void QuitToMain()
    SYNOPSIS
            QuitToMain is called when the user selects to quit to the main menu after dying.
    DESCRIPTION
            This function just destroys all of the objects and then loads the main menu scene.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void QuitToMain()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);

        //Destroy(BattleManager.instance.gameObject);
        SceneManager.LoadScene(mainMenuScene);
    }
    /**/    
    /*
    GameOver.Cs - LoadLastSave()
    NAME
            public void LoadLastSave()
    SYNOPSIS
            LoadLastSave is called when the user selects to load their last save after dying.
    DESCRIPTION
            This function destroys all o fthe current game objects and then loads the scene 
            loadGameScene which internally handles a lot of the loading process in Unity.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void LoadLastSave()
    {
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(GameMenu.instance.gameObject);
        //Destroy(BattleManager.instance.gameObject);


        SceneManager.LoadScene(loadGameScene);
    }
}
