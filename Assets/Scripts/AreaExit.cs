using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Access scene management stuff built in by unity
//but not default in there
public class AreaExit : MonoBehaviour
{
    //The string of the scene to load
    public string areaToLoad;
    //The string of our transition name
    public string areaTransitionName;

    //An area entrance object which is the location we will enter from
    public AreaEntrance theEntrance;

    //The time we are waiting after exiting (so we can fade to make it
    //Look aesthetic) 
    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;
    // Start is called before the first frame update

    /*!!*/
    /*!!
    AreaExit.cs --- Start()
    NAME
            Start() - Function is ran whenever I have exited one scene and am entering another
    SYNOPSIS
            Setting the entrances transition name to area transition name
    DESCRIPTION
            I set the ENTRANCE transition name to the EXIT transitionName (which I label in the Unity engine) because if they are equal then 
            I am able to load into the scene.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /*!!*/
    // Start is called before the first frame update
    void Start()
    {
        theEntrance.transitionName = areaTransitionName;
        
    }



    /*!!*/
    /*!!
    AreaExit.cs --- Update()
    NAME
            Update() - In Unity, Update is called once per frame
    SYNOPSIS
            Checking if I should be loading after a fade. 
            If I should, then I slowly fade and then load the scene that I want to load to.
    DESCRIPTION
            I set the ENTRANCE transition name to the EXIT transitionName (which I label in the Unity engine) because if they are equal then 
            I am able to load into the scene.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /*!!*/
    
    void Update()
    {
        if(shouldLoadAfterFade)
        {
            //Fading appropriately to any computers hardware
            //(A faster hardware can deal with a faster fade)
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <=0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(areaToLoad);
            }
        }
    }

    /*!!*/
    /*!!
    AreaExit.cs --- OnTriggerEnter2D()
    NAME
            OnTriggerEnter2D()
    SYNOPSIS
            A function built into Unity that allows for me to do something when I exit a scene(or 'collide') with a certain area
    DESCRIPTION
            Since my player tag is "Player", I check if a Player is the tag of the object moving (in this case other). 
            If it is, I set shoudLoadAfterFade to true(so that we can begin fading and changing scenes)
            and I change it in the gamemanager class as well. I then fade to black
            and set equal the transition names so on later checks they will pass.
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /*!!*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Load into a new scene
            //SceneManager.LoadScene(areaToLoad);

            shouldLoadAfterFade = true;
            GameManager.instance.fadingBetweenAreas= true;
            UIFade.instance.FadeToBlack();
            PlayerController.instance.areaTransitionName = areaTransitionName;
        }
    }
}
