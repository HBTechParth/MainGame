using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class DragonTigerAIManager : MonoBehaviour
{
    public bool isAutoGenerateOn;
    
    public Text dragonPriceTxt;
    public Text tigerPriceTxt;
    public Text tiePriceTxt;
    
    public static DragonTigerAIManager Instance;
    public List<GameObject> chips;
    public List<Transform> spawnLocations;
    
    public List<GameObject> genChipList_Dragon = new List<GameObject>();
    public List<GameObject> genChipList_Tiger = new List<GameObject>();
    public List<GameObject> genChipList_Tie = new List<GameObject>();

    public bool isActive;

    public float _price = 356;
    public float _dMinBalance;
    public float _tMinBalance;
    public float _tiMinBalance;
    
    private Dictionary<int, int> _weightDictionary = new Dictionary<int, int>();
    
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        isActive = false;
    }

    private void Start()
    {
        // Initialize the weight dictionary
        _weightDictionary.Add(1, 6); // Number 1 has a weight of 6
        _weightDictionary.Add(2, 6); // Number 2 has a weight of 6
        _weightDictionary.Add(3, 2); // Number 3 has a weight of 3
    }

    float interval = 0.25f; 
    float nextTime = 0;
    

    private void Update()
    {
        if (isActive)
        {
            if (Time.time >= nextTime)
            {
                GetChipLocation();
                nextTime += interval;
            }
        }
        
    }
    
    private int GetWeightedRandomNumber()
    {
        int totalWeight = _weightDictionary.Values.Sum();
        
        int randomWeight = Random.Range(0, totalWeight);
        
        int currentWeight = 0;
        foreach (KeyValuePair<int, int> number in _weightDictionary)
        {
            currentWeight += number.Value;
            if (randomWeight <= currentWeight)
            {
                return number.Key;
            }
        }
        
        return 0;
    }


    public void GetChipLocation()
    {
        int place = GetWeightedRandomNumber();
        ChipLocation(place);
        DeductBalance();
    }
    

    public void ChipLocation(int place)
    {
        switch (place)
        {
            case 1:
                // Dragon conduction
                SoundManager.Instance.ThreeBetSound();
                // Vector3 dPos = new Vector3(UnityEngine.Random.Range(DragonTigerManager.Instance.minDragonX, DragonTigerManager.Instance.maxDragonX), UnityEngine.Random.Range(DragonTigerManager.Instance.minDragonY, DragonTigerManager.Instance.maxDragonY));
                Vector3 dPos = GetRandomPositionWithinTransform(DragonTigerManager.Instance.dragonParent.transform);
                int spawnDCoin = Random.Range(0, chips.Count);
                GameObject chipGenD = Instantiate(chips[spawnDCoin], DragonTigerManager.Instance.dragonParent.transform);
                int spawnLocationD = Random.Range(0, spawnLocations.Count);
                chipGenD.transform.position = spawnLocations[spawnLocationD].transform.position;
                _dMinBalance += DragonTigerManager.Instance.chipPrice[spawnDCoin];
                genChipList_Dragon.Add(chipGenD);
                ChipGenerate(chipGenD, dPos);
                UpdateDragonPrice();
                break;
            case 2:
                // tiger conduction
                SoundManager.Instance.ThreeBetSound();
                //  Vector3 tPos = new Vector3(UnityEngine.Random.Range(DragonTigerManager.Instance.minTigerX, DragonTigerManager.Instance.maxTigerX), UnityEngine.Random.Range(DragonTigerManager.Instance.minTigerY, DragonTigerManager.Instance.maxTigerY));
                Vector3 tPos = GetRandomPositionWithinTransform(DragonTigerManager.Instance.tigerParent.transform);

                int spawnTCoin = Random.Range(0, chips.Count);
                GameObject chipGenT = Instantiate(chips[spawnTCoin], DragonTigerManager.Instance.tigerParent.transform);
                int spawnLocationT = Random.Range(0, spawnLocations.Count);
                chipGenT.transform.position = spawnLocations[spawnLocationT].transform.position;
                _tMinBalance += DragonTigerManager.Instance.chipPrice[spawnTCoin];
                genChipList_Tiger.Add(chipGenT);
                ChipGenerate(chipGenT, tPos);
                UpdateTigerPrice();
                break;
            case 3:
                // tie conduction
                SoundManager.Instance.ThreeBetSound();
                //   Vector3 Pos = new Vector3(UnityEngine.Random.Range(DragonTigerManager.Instance.minTieX, DragonTigerManager.Instance.maxTieX), UnityEngine.Random.Range(DragonTigerManager.Instance.minTieY, DragonTigerManager.Instance.maxTieY));
                Vector3 Pos = GetRandomPositionWithinTransform(DragonTigerManager.Instance.tieParent.transform);
                int spawnCoin = Random.Range(0, 4);
                GameObject chipGen = Instantiate(chips[spawnCoin], DragonTigerManager.Instance.tieParent.transform);
                int spawnLocation = Random.Range(0, spawnLocations.Count);
                chipGen.transform.position = spawnLocations[spawnLocation].transform.position;
                _tiMinBalance += DragonTigerManager.Instance.chipPrice[spawnCoin];
                genChipList_Tie.Add(chipGen);
                ChipGenerate(chipGen, Pos);
                UpdateTiePrice();
                break;
        }
    }
    
    public Vector3 GetRandomPositionWithinTransform(Transform targetTransform)
    {
        RectTransform rectTransform = targetTransform.GetComponent<RectTransform>();

        // Calculate the local bounds
        Vector2 size = rectTransform.rect.size;
        Vector3 localRandomPos = new Vector3(
            UnityEngine.Random.Range(-size.x / 2, size.x / 2),
            UnityEngine.Random.Range(-size.y / 2, size.y / 2),
            0
        );

        // Convert local position to world position
        Vector3 worldRandomPos = targetTransform.TransformPoint(localRandomPos);

        return worldRandomPos;
    }
    public void ChipGenerate(GameObject chip, Vector3 endPos)
    {
        chip.transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), 0.2f);
        chip.transform.DOMove(endPos, 0.2f).OnComplete(() =>
        {
            chip.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f).OnComplete(() =>
            {
                chip.transform.DOScale(Vector3.one, 0.07f);
            });
        });
    }

    public void ResetChipsAi()
    {
        genChipList_Dragon.Clear();
        genChipList_Tiger.Clear();
        genChipList_Tie.Clear();
    }

    public IEnumerator CoinDestroy(int winNo)
    {
        float waitTime = 0;
        if (winNo == 2 || winNo == 3)
            waitTime = 3.16f;
        
        
        yield return new WaitForSeconds(waitTime);
        
        switch (winNo)
        {
            case 1:
            {
                float animSpeed = 0.3f;
                int tNum = genChipList_Tie.Count + genChipList_Tiger.Count + genChipList_Dragon.Count;

                for (int i = 0; i < genChipList_Dragon.Count; i++)
                {
                    int no = i;

                    genChipList_Dragon[no].transform.DOScale(Vector3.zero, animSpeed);
                    genChipList_Dragon[no].transform.DOMove(DragonTigerManager.Instance.cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                    {
                        Vector3 rPos = GetRandomPositionWithinTransform(DragonTigerManager.Instance.tieParent.transform);
                       // Vector3 rPos = new Vector3(UnityEngine.Random.Range(DragonTigerManager.Instance.minTieX, DragonTigerManager.Instance.maxTieX), UnityEngine.Random.Range(DragonTigerManager.Instance.minTieY, DragonTigerManager.Instance.maxTieY));
                        genChipList_Dragon[no].transform.DOMove(rPos, animSpeed);
                        genChipList_Dragon[no].transform.DOScale(Vector3.one, animSpeed);
                        genChipList_Dragon[no].transform.SetParent(DragonTigerManager.Instance.tieParent.transform);
                        genChipList_Dragon.Add(genChipList_Dragon[no]);
                        genChipList_Dragon[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            genChipList_Tie.RemoveAt(no);
                            
                            UpdateList(tNum, genChipList_Tie, genChipList_Dragon,winNo);
                        });
                    });
                }

                for (int i = 0; i < genChipList_Tiger.Count; i++)
                {

                    int no = i;
                    genChipList_Tiger[no].transform.DOScale(Vector3.zero, animSpeed);
                    genChipList_Tiger[no].transform.DOMove(DragonTigerManager.Instance.cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                    {
                        Vector3 rPos = GetRandomPositionWithinTransform(DragonTigerManager.Instance.tieParent.transform);

                        //Vector3 rPos = new Vector3(UnityEngine.Random.Range(DragonTigerManager.Instance.minTieX, DragonTigerManager.Instance.maxTieX), UnityEngine.Random.Range(DragonTigerManager.Instance.minTieY, DragonTigerManager.Instance.maxTieY));
                        genChipList_Tiger[no].transform.DOMove(rPos, animSpeed);
                        genChipList_Tiger[no].transform.DOScale(Vector3.one, animSpeed);
                        genChipList_Tiger[no].transform.SetParent(DragonTigerManager.Instance.tieParent.transform);
                        genChipList_Tiger.Add(genChipList_Dragon[no]);
                        genChipList_Tiger[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            genChipList_Tie.RemoveAt(no);

                            UpdateList(tNum, genChipList_Tie, genChipList_Tiger,winNo);

                        });
                    });
                }

                break;
            }
            case 2:
            {
                float animSpeed = 0.3f;


                int tNum = genChipList_Tie.Count + genChipList_Tiger.Count + genChipList_Dragon.Count;
                for (int i = 0; i < genChipList_Tie.Count; i++)
                {
                    int no = i;

                    genChipList_Tie[no].transform.DOScale(Vector3.zero, animSpeed);
                    genChipList_Tie[no].transform.DOMove(DragonTigerManager.Instance.cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                    {
                        Vector3 rPos = GetRandomPositionWithinTransform(DragonTigerManager.Instance.dragonParent.transform);

                        //Vector3 rPos = new Vector3(UnityEngine.Random.Range(DragonTigerManager.Instance.minDragonX, DragonTigerManager.Instance.maxDragonX), UnityEngine.Random.Range(DragonTigerManager.Instance.minDragonY, DragonTigerManager.Instance.maxDragonY));
                        genChipList_Tie[no].transform.DOMove(rPos, animSpeed);

                        genChipList_Tie[no].transform.DOScale(Vector3.one, animSpeed);
                        genChipList_Tie[no].transform.SetParent(DragonTigerManager.Instance.dragonParent.transform);
                        genChipList_Dragon.Add(genChipList_Tie[no]);

                        genChipList_Tie[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            genChipList_Tie.RemoveAt(no);

                            UpdateList(tNum, genChipList_Dragon, genChipList_Tie,winNo);
                        });
                    });
                }

                for (int i = 0; i < genChipList_Tiger.Count; i++)
                {

                    int no = i;
                    genChipList_Tiger[no].transform.DOScale(Vector3.zero, animSpeed);
                    genChipList_Tiger[no].transform.DOMove(DragonTigerManager.Instance.cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                    {
                        Vector3 rPos = GetRandomPositionWithinTransform(DragonTigerManager.Instance.dragonParent.transform);

                        //Vector3 rPos = new Vector3(UnityEngine.Random.Range(DragonTigerManager.Instance.minDragonX, DragonTigerManager.Instance.maxDragonX), UnityEngine.Random.Range(DragonTigerManager.Instance.minDragonY, DragonTigerManager.Instance.maxDragonY));
                        genChipList_Tiger[no].transform.DOMove(rPos, animSpeed);
                        genChipList_Tiger[no].transform.DOScale(Vector3.one, animSpeed);
                        genChipList_Tiger[no].transform.SetParent(DragonTigerManager.Instance.dragonParent.transform);
                        genChipList_Dragon.Add(genChipList_Tiger[no]);
                        genChipList_Tiger[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {
                            genChipList_Tiger.Remove(genChipList_Tiger[no]);
                            UpdateList(tNum, genChipList_Dragon, genChipList_Tiger,winNo);
                        });
                    });
                }

                break;
            }
            case 3:
            {
                float animSpeed = 0.3f;

                int tNum = genChipList_Tie.Count + genChipList_Tiger.Count + genChipList_Dragon.Count;
                for (int i = 0; i < genChipList_Dragon.Count; i++)
                {
                    int no = i;
                    genChipList_Dragon[no].transform.DOScale(Vector3.zero, animSpeed);
                    genChipList_Dragon[no].transform.DOMove(DragonTigerManager.Instance.cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                    {
                        Vector3 rPos = GetRandomPositionWithinTransform(DragonTigerManager.Instance.tigerParent.transform);

                        //Vector3 rPos = new Vector3(UnityEngine.Random.Range(DragonTigerManager.Instance.minTigerX, DragonTigerManager.Instance.maxTigerX), UnityEngine.Random.Range(DragonTigerManager.Instance.minTigerY, DragonTigerManager.Instance.maxTigerY));
                        genChipList_Dragon[no].transform.DOMove(rPos, animSpeed);
                        genChipList_Dragon[no].transform.DOScale(Vector3.one, animSpeed);

                        genChipList_Dragon[no].transform.SetParent(DragonTigerManager.Instance.tigerParent.transform);
                        genChipList_Tiger.Add(genChipList_Dragon[no]);

                        genChipList_Dragon[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {

                            UpdateList(tNum, genChipList_Tiger, genChipList_Dragon,winNo);

                        });
                    });
                }

                for (int i = 0; i < genChipList_Tie.Count; i++)
                {
                    int no = i;
                    genChipList_Tie[no].transform.DOScale(Vector3.zero, animSpeed);

                    genChipList_Tie[no].transform.DOMove(DragonTigerManager.Instance.cardCenterObj.transform.position, animSpeed).OnComplete(() =>
                    {
                        Vector3 rPos = GetRandomPositionWithinTransform(DragonTigerManager.Instance.tigerParent.transform);

                        //Vector3 rPos = new Vector3(UnityEngine.Random.Range(DragonTigerManager.Instance.minTigerX, DragonTigerManager.Instance.maxTigerX), UnityEngine.Random.Range(DragonTigerManager.Instance.minTigerY, DragonTigerManager.Instance.maxTigerY));
                        genChipList_Tie[no].transform.DOMove(rPos, animSpeed);
                        genChipList_Tie[no].transform.DOScale(Vector3.one, animSpeed);
                        genChipList_Tie[no].transform.SetParent(DragonTigerManager.Instance.tigerParent.transform);
                        genChipList_Tiger.Add(genChipList_Tie[no]);
                        genChipList_Tie[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                        {

                            UpdateList(tNum, genChipList_Tiger, genChipList_Tie, winNo);


                        });
                    });
                }

                break;
            }
        }
        
        // foreach (var t in genChipList_Dragon) { Destroy(t); }
        // genChipList_Dragon.Clear();
        // foreach (var t in genChipList_Tiger) { Destroy(t); }
        // genChipList_Tiger.Clear();
        // foreach (var t in genChipList_Tie) { Destroy(t); }
        // genChipList_Tie.Clear();
        
        ResetPrice();
    }
    
    void UpdateList(int no, List<GameObject> list, List<GameObject> list1, int winNo)
    {
        
        if (no == list.Count)
        {
            list1.Clear();
        }
        else
        {
            return;
        }
        float moveSpeed = 0.2f;

        switch (winNo)
        {
            case 1:
            {
                if (genChipList_Dragon.Count == 0 && genChipList_Tiger.Count == 0)
                {
                    foreach (var t in genChipList_Tie)
                    {
                        // t.transform.DOMove(DragonTigerManager.Instance.ourProfile.transform.position,
                        //     moveSpeed);
                        // t.transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        // {
                        //     Destroy(t);
                        // });
                        t.transform
                            .DOMove(DragonTigerManager.Instance.otherProfile.transform.position, moveSpeed);
                        t.transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(t);
                        });
                    }
                    
                    //genChipList_Tie.Clear();
                }

                break;
            }
            case 2:
            {
                if (genChipList_Tie.Count == 0 && genChipList_Tiger.Count == 0)
                {
                    foreach (var t in genChipList_Dragon)
                    {
                        // t.transform.DOMove(DragonTigerManager.Instance.ourProfile.transform.position,
                        //     moveSpeed);
                        // t.transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        // {
                        //     Destroy(t);
                        // });


                        t.transform.DOMove(DragonTigerManager.Instance.otherProfile.transform.position,
                            moveSpeed);
                        t.transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(t);
                        });
                    }

                    //genChipList_Dragon.Clear();
                }

                break;
            }
            case 3:
            {
                if (genChipList_Dragon.Count == 0 && genChipList_Tie.Count == 0)
                {
                    foreach (var t in genChipList_Tiger)
                    {
                        // t.transform.DOMove(DragonTigerManager.Instance.ourProfile.transform.position,
                        //     moveSpeed);
                        // t.transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        // {
                        //     Destroy(t);
                        // });


                        t.transform.DOMove(DragonTigerManager.Instance.otherProfile.transform.position,
                            moveSpeed);
                        t.transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        {
                            Destroy(t);
                        });
                    }

                    //genChipList_Tiger.Clear();
                }

                break;
            }
        }
        //ResetChipsAi();
    }

    private float negativebalance = 19f;

    public void DeductBalance()
    {
        int num = Random.Range(0, 7);
        var totalBalance = DragonTigerManager.Instance.DTPlayerList[num].balance -= negativebalance;
        DragonTigerManager.Instance.DTPlayerList[num].playerBalanceTxt.text = totalBalance.ToString(CultureInfo.InvariantCulture);
    }

    private void UpdateDragonPrice()
    {
        dragonPriceTxt.text = _dMinBalance.ToString(CultureInfo.InvariantCulture);
    }
    private void UpdateTigerPrice()
    {
        tigerPriceTxt.text = _tMinBalance.ToString(CultureInfo.InvariantCulture);
    }
    public void UpdateTiePrice()
    {
        tiePriceTxt.text = _tiMinBalance.ToString(CultureInfo.InvariantCulture);
    }

    private void ResetPrice()
    {
        _dMinBalance = 0;
        _tMinBalance = 0;
        _tiMinBalance = 0;
        
        UpdateDragonPrice();
        UpdateTigerPrice();
        UpdateTiePrice();
    }

}


