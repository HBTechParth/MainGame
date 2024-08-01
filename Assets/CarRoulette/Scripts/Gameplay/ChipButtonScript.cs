using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipButtonScript : MonoBehaviour
{
    public bool Selected;
    public GameObject SelectedChip;
    public GameObject UnselectedChip;
    public int Value;
    public GameObject GManager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Selected)
        {
            SelectedChip.SetActive(true);
            UnselectedChip.SetActive(false);
        }
        else
        {
            SelectedChip.SetActive(false);
            UnselectedChip.SetActive(true);
        }
    }
}
