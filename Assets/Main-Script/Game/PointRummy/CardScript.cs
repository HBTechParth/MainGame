using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public Button button;
    public PointRummyManager.CardSuffle card;
    public GameObject wildJoker;
    // Start is called before the first frame update
    void Start()
    {
        if(button != null)
            button.onClick.AddListener(() => PointRummyManager.Instance.CardClick(this.gameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
