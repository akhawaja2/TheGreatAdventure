using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//Access scene management stuff built in by unity
//but not default in there
public class AreaExit : MonoBehaviour
{
    public string areaToLoad;
    public string areaTransitionName;

    public AreaEntrance theEntrance;

    public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;
    // Start is called before the first frame update
    void Start()
    {
        theEntrance.transitionName = areaTransitionName;
        
    }

    // Update is called once per frame
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
