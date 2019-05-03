using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//The Camera Controller is the script behind how the custom camera is implemented ingame
public class CameraController : MonoBehaviour
{
    //The focus of the camera (ex. player, or maybe an NPC...)
    public Transform target;

    public Tilemap theMap;
    //Lets us know how far we can move updown/left/right
    private Vector3 bottomLeftLimit;
    private Vector3 topRightLimit;

    //The camera only shows half of the 'void' (space not worked on)
    //So I only need half the camera.
    private float halfHeight;
    private float halfWidth;

    //Playing music via the camera 
    public int musicToPlay;
    private bool musicStarted;
    /**/    
    /*
    CameraController.Cs 
    NAME
          void Start()
    SYNOPSIS
           What the camera does on instantiation
    DESCRIPTION
            This function searches for the player controller location, and get the 
            camera size and set the camera so it does not extend past the map (so the player
            cannot see into the void)
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
        //target = PlayerController.instance.transform;
        target = FindObjectOfType<PlayerController>().transform;
        //main.orthographic size is built into Unity to get size of camera
        halfHeight = Camera.main.orthographicSize;
        //camera.main.aspect is our aspect ratio (Which in unity I set to 16:9)
        halfWidth = halfHeight * Camera.main.aspect;

        //Getting the max and min limits for camera (so it does not pan over into the void)
        //By adding  halfheight/halfwidth we move the camera up accordingly so we do not see the void
        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0);
        topRightLimit = theMap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0);

        //Setting the bounds to the maps local bounds
        PlayerController.instance.SetBounds(theMap.localBounds.min, theMap.localBounds.max);
    }
    /**/    
    /*
    CameraController.Cs 
    NAME
          void LateUpdate()
    SYNOPSIS
           LateUpdate is called once per frame after Update()
    DESCRIPTION
            This function just follows the player every frame as the move and plays music if
            it has not started
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        //Keep camera inside bounds
        //Clamp: Takes a value and clamps it between two points (in this case bottomleft and topright
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

        if (!musicStarted)
        {
            musicStarted = true;
            AudioManager.instance.PlayBGM(musicToPlay);
        }
    }
}
