using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//We can't add this into Unity 
//But we can call reference it in BattleManager
[System.Serializable]
public class BattleMove
{
    //just using it for these variables. dont need it to exist in the world

    public string moveName;
    public int movePower;
    public int moveCost;
    //visual sprite
    public AttackEffect theEffect;
    //big list of all the moves we can do
    // Start is called before the first frame update
}
