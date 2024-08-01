using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPrefabScript : MonoBehaviour
{
    public Image BG;
    public Image Symbol;
    public Sprite BGSprite;
    public List<Sprite> SymbolSprites;
    public int SymbolIndex;
    public bool NotHidden;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void SpriteSetter()
    {
        if(SymbolIndex == 0 || SymbolIndex == 1)
        {
            BG.sprite = BGSprite;
            
        }

       if(SymbolIndex == 8)
        {
            Symbol.gameObject.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        }
        Symbol.sprite = SymbolSprites[SymbolIndex];
    }
    public void SetHidden()
    {
        if (NotHidden)
        {
            BG.gameObject.SetActive(true);
        }
        else
        {
           BG.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpriteSetter();
        SetHidden();
    }
}
