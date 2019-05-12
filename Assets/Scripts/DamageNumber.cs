using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//DamageNumber is the object that deals with displaying text aesthetically in battle 
public class DamageNumber : MonoBehaviour
{
    public Text damageText;
    //Animation lifetime and speed
    public float lifetime = 1f;
    public float moveSpeed = 1f;

    //making dmg # not appear in the same place over and over again
    public float placementJitter = .5f;
    // Start is called before the first frame update
    void Start(){}

    /**/    
    /*!
    DamageNumber.Cs - Update()
    NAME
           void Update()
    SYNOPSIS
            Update is called once per frame
    DESCRIPTION
            This function destroys the object if it has already been created and then gets a
            new position to show the damage location
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
        Destroy(gameObject, lifetime);
        transform.position += new Vector3(0f, moveSpeed * Time.deltaTime, 0f);
    }
     /**/    
    /*!
    DamageNumber.Cs - SetDamage()
    NAME
           public void SetDamage(int damageAmount)
    SYNOPSIS
            SetDamage is how the damageText object gets the text
    DESCRIPTION
            The function gets the damageamount and sets the animation text to it, and gets
            a new position to display the text
    RETURNS
            N/A
    AUTHOR
            Abu Khawaja
    DATE
            4/30/2019
    */
    /**/
    public void SetDamage(int damageAmount)
    {
        damageText.text = damageAmount.ToString();
        transform.position += new Vector3(Random.Range(-placementJitter, placementJitter), Random.Range(-placementJitter, placementJitter), 0f);

    }
}
