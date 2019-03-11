using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerController.instance.transform;

        //main.orthographic size is built into Unity to get size of camera
        halfHeight = Camera.main.orthographicSize;
        //camera.main.aspect is our aspect ratio (Which in unity I set to 16:9)
        halfWidth = halfHeight * Camera.main.aspect;

        //Getting the max and min limits for camera (so it does not pan over into the void)
        //By adding  halfheight/halfwidth we move the camera up accordingly so we do not see the void
        bottomLeftLimit = theMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0);
        topRightLimit = theMap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0); 


    }

    // LateUpdate is called once per frame after Update()
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        //Keep camera inside bounds
        //Clamp: Takes a value and clamps it between two points (in this case bottomleft and topright
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);
    }
}
