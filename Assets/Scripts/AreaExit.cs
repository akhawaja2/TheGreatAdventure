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
    // Start is called before the first frame update
    void Start()
    {
        theEntrance.transitionName = areaTransitionName;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //Load into a new scene
            SceneManager.LoadScene(areaToLoad);

            PlayerController.instance.areaTransitionName = areaTransitionName;
        }
    }
}
