using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerinfoPrefabScript : MonoBehaviour
{
    public string Name;
    public string id;
    
    [SerializeField]
    Text NameText;
    [SerializeField]
    Text idText;
   
    // Start is called before the first frame update
    void Start()
    {
        NameText.text = Name;
        idText.text = id;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
