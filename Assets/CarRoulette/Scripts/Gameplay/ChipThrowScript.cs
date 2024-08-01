using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChipThrowScript : MonoBehaviour
{
    public List<Sprite> Chips;
    public int MyIndx;
    public bool PlayerChip = false;
    GameObject DeckMan;
    // Start is called before the first frame update
    void Start()
    {
        DeckMan = GameObject.FindGameObjectWithTag("CarRouletteManager");
    }

    // Update is called once per frame
    /*void Update()
    {
        if (DeckMan != null)
        {
            if (DeckMan.GetComponent<BotManagerScript>().OverlayMenu)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().enabled = true;
            }

        }
        GetComponent<Image>().sprite = Chips[MyIndx];
        GetComponent<SpriteRenderer>().sprite= Chips[MyIndx];
    }*/
}
