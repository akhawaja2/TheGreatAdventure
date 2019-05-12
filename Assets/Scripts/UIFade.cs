using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI Fade handles fading done from switching scenes so the game does not look
//choppy
public class UIFade : MonoBehaviour
{
    //Setting UIFade to be an instance
    public static UIFade instance;

    //This script will always check if we should be fading the screen in and out of black
    public bool shouldFadeToBlack;
    public bool shouldFadeFromBlack;

    //The amount of time to fade in/out and the image (which is just a black screen)
    public float fadeSpeed;
    public Image fadeScreen;

    /**/        
    /*!
    UIFade.Cs - Start()
    NAME
            public void Start( )
    SYNOPSIS
            Start is called before the first frame update
    DESCRIPTION
            Start creates an instance of the UIFade and 
            DontDestroyOnLoad is used to to preserve an Object during loading
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

        DontDestroyOnLoad(gameObject);
    }

    /**/        
    /*!
    UIFade.Cs - Update()
    NAME
            public void Update( )
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            Update checks if the game is fading TO black or FROM black.
            If fading to, the fade screen color is set to a black color.
            If fading from, the fade screen color is set to fully transparent color. 
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
        if (shouldFadeToBlack)
        {
            //Changing alpha value to 1 gradually using the MoveTowards function
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
        }
        else if (shouldFadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
        }
    }
    /**/        
    /*!
    UIFade.Cs - FadeToBlack()
    NAME
            public void FadeToBlack( )
    SYNOPSIS
            Sets the fading variables
    DESCRIPTION
           FadeToBlack sets shouldFadeToBlack to true, and shouldFadeFromBlack to false.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }
    /**/        
    /*!
    UIFade.Cs - FadeFromBlack()
    NAME
            public void FadeFromBlack( )
    SYNOPSIS
            Sets the fading variables
    DESCRIPTION
           FadeToBlack sets shouldFadeToBlack to false, and shouldFadeFromBlack to true.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void FadeFromBlack()
    {
        shouldFadeToBlack = false;
        shouldFadeFromBlack = true;
    }
}
