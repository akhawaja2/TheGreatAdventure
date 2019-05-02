using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    //How long the animation is
    public float effectLength;
    //What the sound effect is
    public int soundEffect;
    /**/
    /*
    AttackEffect.cs --- Start()
    NAME
            Start() - In Unity,  Start is called before the first frame update
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
    /**/
    void Start()
    {
        AudioManager.instance.PlaySFX(soundEffect);
    }

    /**/
    /*
    AttackEffect.cs --- Update()
    NAME
            Update() - In Unity,   Update is called once per frame
    SYNOPSIS
           Deleting the attackeffect object and also the effect length.
    DESCRIPTION
            Deleting the attackeffect object and also the effect length so there aren't a billion of them lying around taking up space 
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
        Destroy(gameObject, effectLength);
    }
}
