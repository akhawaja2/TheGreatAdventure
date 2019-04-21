using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemButton : MonoBehaviour
{
    //The current buttons image
    public Image ButtonImage;
    //The current amount of the item we have
    public Text amountText;
    //button slot in the inventory:
    //0 1 2 3 4 5 6 7 8 9
    //10 11 12 13 14 15 16 17
    //etc. etc.
    public int buttonValue;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Press()
    {
        if (GameManager.instance.itemsHeld[buttonValue] != "")
        {
            //If the button is not blank then we clal a function from GameMenu to set the item active and update
            //item name/description in the inventory
            GameMenu.instance.SelectItem(GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[buttonValue]));

        }
    }
}
