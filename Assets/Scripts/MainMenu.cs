using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    //New game scene to load
    public string newGameScene;
    //Button to control load game
    public GameObject continueButton;
    //Name of scene to load in
    public string loadGameScene;
    /**/    
    /*!
    MainMenu.Cs - Start()
    NAME
            void Start()
    SYNOPSIS
            Start is called before the first frame update
    DESCRIPTION
          Start is ran at the opening of the game - it checks if there is a previous save and
          if there is it adds the continue button, and if not it sets the continue button to not appear.
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
        if (PlayerPrefs.HasKey("Current_Scene"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update(){ }
    /**/    
    /*!
    MainMenu.Cs - Continue()
    NAME
            void Continue()
    SYNOPSIS
            Continue is used to load a previous save
    DESCRIPTION
          Continue is ran at the opening of the game - it is attached to the continue button in Unity and
          merely loads the loadGameScene which is the bridge between the main menu and the previous save
          that is loaded.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void Continue()
    {
        SceneManager.LoadScene(loadGameScene);
    }
    /**/    
    /*!
    MainMenu.Cs - NewGame()
    NAME
            void NewGame()
    SYNOPSIS
            NewGame is used to start a new game
    DESCRIPTION
          NewGame is ran at the opening of the game - when clicked it loads the new game scene
          which in this case is our countryside scene.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void NewGame()
    {
        SceneManager.LoadScene(newGameScene);
    }
    /**/    
    /*!
    MainMenu.Cs - Exit()
    NAME
            void Exit()
    SYNOPSIS
            Exit is used to quit the game
    DESCRIPTION
         if the user elects to quit the game then the Quit function is ran - Application.Quit()
         merely closes the game
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void Exit()
    {
        Application.Quit();
    }
}
