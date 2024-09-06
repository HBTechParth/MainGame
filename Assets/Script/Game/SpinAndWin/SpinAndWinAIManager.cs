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


public class SpinAndWinAIManager : MonoBehaviour
{
    public bool isAutoGenerateOn;
    
    public Text dragonPriceTxt;
    public Text tigerPriceTxt;
    public Text tiePriceTxt;
    
    public static SpinAndWinAIManager Instance;
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


    /* public void ChipLocation(int place)
     {
         switch (place)
         {
             case 1:
                 // Dragon conduction
                 SoundManager.Instance.ThreeBetSound();
                 Vector3 dPos = new Vector3(UnityEngine.Random.Range(SpinAndWinManager.Instance.minDragonX, SpinAndWinManager.Instance.maxDragonX), UnityEngine.Random.Range(SpinAndWinManager.Instance.minDragonY, SpinAndWinManager.Instance.maxDragonY));
                 int spawnDCoin = Random.Range(0, chips.Count);
                 GameObject chipGenD = Instantiate(chips[spawnDCoin], SpinAndWinManager.Instance.dragonParent.transform);
                 int spawnLocationD = Random.Range(0, spawnLocations.Count);
                 chipGenD.transform.position = spawnLocations[spawnLocationD].transform.position;
                 _dMinBalance += SpinAndWinManager.Instance.chipPrice[spawnDCoin];
                 genChipList_Dragon.Add(chipGenD);
                 ChipGenerate(chipGenD, dPos);
                 UpdateDragonPrice();
                 break;
             case 2:
                 // tiger conduction
                 SoundManager.Instance.ThreeBetSound();
                 Vector3 tPos = new Vector3(UnityEngine.Random.Range(SpinAndWinManager.Instance.minTigerX, SpinAndWinManager.Instance.maxTigerX), UnityEngine.Random.Range(SpinAndWinManager.Instance.minTigerY, SpinAndWinManager.Instance.maxTigerY));
                 int spawnTCoin = Random.Range(0, chips.Count);
                 GameObject chipGenT = Instantiate(chips[spawnTCoin], SpinAndWinManager.Instance.tigerParent.transform);
                 int spawnLocationT = Random.Range(0, spawnLocations.Count);
                 chipGenT.transform.position = spawnLocations[spawnLocationT].transform.position;
                 _tMinBalance += SpinAndWinManager.Instance.chipPrice[spawnTCoin];
                 genChipList_Tiger.Add(chipGenT);
                 ChipGenerate(chipGenT, tPos);
                 UpdateTigerPrice();
                 break;
             case 3:
                 // tie conduction
                 SoundManager.Instance.ThreeBetSound();
                 Vector3 Pos = new Vector3(UnityEngine.Random.Range(SpinAndWinManager.Instance.minTieX, SpinAndWinManager.Instance.maxTieX), UnityEngine.Random.Range(SpinAndWinManager.Instance.minTieY, SpinAndWinManager.Instance.maxTieY));
                 int spawnCoin = Random.Range(0, 4);
                 GameObject chipGen = Instantiate(chips[spawnCoin], SpinAndWinManager.Instance.tieParent.transform);
                 int spawnLocation = Random.Range(0, spawnLocations.Count);
                 chipGen.transform.position = spawnLocations[spawnLocation].transform.position;
                 _tiMinBalance += SpinAndWinManager.Instance.chipPrice[spawnCoin];
                 genChipList_Tie.Add(chipGen);
                 ChipGenerate(chipGen, Pos);
                 UpdateTiePrice();
                 break;
         }
     }*/

    public Transform dragonTransform;
    public Transform tigerTransform;
    public Transform tieTransform;
    public void ChipLocation(int place)
    {
        Transform targetParent = null;

        switch (place)
        {
            case 1:
                // Dragon area
                SoundManager.Instance.ThreeBetSound();
                targetParent = SpinAndWinManager.Instance.dragonParent.transform;
                break;

            case 2:
                // Tiger area
                SoundManager.Instance.ThreeBetSound();
                targetParent = SpinAndWinManager.Instance.tigerParent.transform;
                break;

            case 3:
                // Tie area
                SoundManager.Instance.ThreeBetSound();
                targetParent = SpinAndWinManager.Instance.tieParent.transform;
                break;
        }

        if (targetParent != null)
        {
            // Select a random spawn location
            int spawnLocation = Random.Range(0, spawnLocations.Count);
            Vector3 startPos = spawnLocations[spawnLocation].transform.position;

            // Generate a random position within the bounds of the targetParent
            Vector3 randomPos = GetRandomPositionWithinTransform(targetParent);

            // Instantiate the chip at the spawn location
            int spawnCoin = Random.Range(0, chips.Count);
            GameObject chipGen = Instantiate(chips[spawnCoin], startPos, Quaternion.identity, targetParent);

            // Animate the chip to the random position
            ChipGenerate(chipGen, randomPos);

            // Add the chip to the appropriate list and update the balance
            switch (place)
            {
                case 1:
                    _dMinBalance += SpinAndWinManager.Instance.chipPrice[spawnCoin];
                    genChipList_Dragon.Add(chipGen);
                    UpdateDragonPrice();
                    break;
                case 2:
                    _tMinBalance += SpinAndWinManager.Instance.chipPrice[spawnCoin];
                    genChipList_Tiger.Add(chipGen);
                    UpdateTigerPrice();
                    break;
                case 3:
                    _tiMinBalance += SpinAndWinManager.Instance.chipPrice[spawnCoin];
                    genChipList_Tie.Add(chipGen);
                    UpdateTiePrice();
                    break;
            }
        }
    }

    // Helper method to get a random position within the bounds of a Transform
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
    public Transform collectCoin;
    public IEnumerator CoinDestroy(int winNo)
    {
        float waitTime = 0;
        if (winNo == 2 || winNo == 3 || winNo==1)
            waitTime = 5;

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
                        genChipList_Dragon[no].transform.DOMove(collectCoin.position, animSpeed).OnComplete(() =>
                        {
                            Vector3 rPos = GetRandomPositionWithinTransform(SpinAndWinManager.Instance.tieParent.transform);
                            genChipList_Dragon[no].transform.DOMove(rPos, animSpeed);
                            genChipList_Dragon[no].transform.DOScale(Vector3.one, animSpeed);
                            genChipList_Dragon[no].transform.SetParent(SpinAndWinManager.Instance.tieParent.transform);
                            genChipList_Dragon.Add(genChipList_Dragon[no]);
                            genChipList_Dragon[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                            {
                                genChipList_Tie.RemoveAt(no);

                                UpdateList(tNum, genChipList_Tie, genChipList_Dragon, winNo);
                            });
                        });
                    }

                    for (int i = 0; i < genChipList_Tiger.Count; i++)
                    {
                        int no = i;
                        genChipList_Tiger[no].transform.DOScale(Vector3.zero, animSpeed);
                        genChipList_Tiger[no].transform.DOMove(collectCoin.position, animSpeed).OnComplete(() =>
                        {
                            Vector3 rPos = GetRandomPositionWithinTransform(SpinAndWinManager.Instance.tieParent.transform);
                            genChipList_Tiger[no].transform.DOMove(rPos, animSpeed);
                            genChipList_Tiger[no].transform.DOScale(Vector3.one, animSpeed);
                            genChipList_Tiger[no].transform.SetParent(SpinAndWinManager.Instance.tieParent.transform);
                            genChipList_Tiger.Add(genChipList_Dragon[no]);
                            genChipList_Tiger[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                            {
                                genChipList_Tie.RemoveAt(no);
                                UpdateList(tNum, genChipList_Tie, genChipList_Tiger, winNo);
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
                        genChipList_Tie[no].transform.DOMove(collectCoin.position, animSpeed).OnComplete(() =>
                        {
                            Vector3 rPos = GetRandomPositionWithinTransform(SpinAndWinManager.Instance.dragonParent.transform);
                            genChipList_Tie[no].transform.DOMove(rPos, animSpeed);
                            genChipList_Tie[no].transform.DOScale(Vector3.one, animSpeed);
                            genChipList_Tie[no].transform.SetParent(SpinAndWinManager.Instance.dragonParent.transform);
                            genChipList_Dragon.Add(genChipList_Tie[no]);

                            genChipList_Tie[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                            {
                                genChipList_Tie.RemoveAt(no);

                                UpdateList(tNum, genChipList_Dragon, genChipList_Tie, winNo);
                            });
                        });
                    }

                    for (int i = 0; i < genChipList_Tiger.Count; i++)
                    {
                        int no = i;
                        genChipList_Tiger[no].transform.DOScale(Vector3.zero, animSpeed);
                        genChipList_Tiger[no].transform.DOMove(collectCoin.position, animSpeed).OnComplete(() =>
                        {
                            Vector3 rPos = GetRandomPositionWithinTransform(SpinAndWinManager.Instance.dragonParent.transform);
                            genChipList_Tiger[no].transform.DOMove(rPos, animSpeed);
                            genChipList_Tiger[no].transform.DOScale(Vector3.one, animSpeed);
                            genChipList_Tiger[no].transform.SetParent(SpinAndWinManager.Instance.dragonParent.transform);
                            genChipList_Dragon.Add(genChipList_Tiger[no]);
                            genChipList_Tiger[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                            {
                                genChipList_Tiger.RemoveAt(no);
                                UpdateList(tNum, genChipList_Dragon, genChipList_Tiger, winNo);
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
                        genChipList_Dragon[no].transform.DOMove(collectCoin.position, animSpeed).OnComplete(() =>
                        {
                            Vector3 rPos = GetRandomPositionWithinTransform(SpinAndWinManager.Instance.tigerParent.transform);
                            genChipList_Dragon[no].transform.DOMove(rPos, animSpeed);
                            genChipList_Dragon[no].transform.DOScale(Vector3.one, animSpeed);

                            genChipList_Dragon[no].transform.SetParent(SpinAndWinManager.Instance.tigerParent.transform);
                            genChipList_Tiger.Add(genChipList_Dragon[no]);

                            genChipList_Dragon[no].transform.DORotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 360)), animSpeed).OnComplete(() =>
                            {
                                UpdateList(tNum, genChipList_Tiger, genChipList_Dragon, winNo);
                            });
                        });
                    }

                    for (int i = 0; i < genChipList_Tie.Count; i++)
                    {
                        int no = i;
                        genChipList_Tie[no].transform.DOScale(Vector3.zero, animSpeed);

                        genChipList_Tie[no].transform.DOMove(collectCoin.position, animSpeed).OnComplete(() =>
                        {
                            Vector3 rPos = GetRandomPositionWithinTransform(SpinAndWinManager.Instance.tigerParent.transform);
                            genChipList_Tie[no].transform.DOMove(rPos, animSpeed);
                            genChipList_Tie[no].transform.DOScale(Vector3.one, animSpeed);
                            genChipList_Tie[no].transform.SetParent(SpinAndWinManager.Instance.tigerParent.transform);
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
        if (SpinAndWinManager.Instance.isAdmin)
        {
            SpinAndWinManager.Instance.UpdateHistoryChips(winNo);
        }
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
                        // t.transform.DOMove(SpinAndWinManager.Instance.ourProfile.transform.position,
                        //     moveSpeed);
                        // t.transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        // {
                        //     Destroy(t);
                        // });
                        t.transform
                            .DOMove(SpinAndWinManager.Instance.otherProfile.transform.position, moveSpeed);
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
                        // t.transform.DOMove(SpinAndWinManager.Instance.ourProfile.transform.position,
                        //     moveSpeed);
                        // t.transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        // {
                        //     Destroy(t);
                        // });


                        t.transform.DOMove(SpinAndWinManager.Instance.otherProfile.transform.position,
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
                        // t.transform.DOMove(SpinAndWinManager.Instance.ourProfile.transform.position,
                        //     moveSpeed);
                        // t.transform.DOScale(Vector3.zero, moveSpeed).OnComplete(() =>
                        // {
                        //     Destroy(t);
                        // });


                        t.transform.DOMove(SpinAndWinManager.Instance.otherProfile.transform.position,
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
        var totalBalance = SpinAndWinManager.Instance.SpinAndWinPlayerList[num].balance -= negativebalance;
        SpinAndWinManager.Instance.SpinAndWinPlayerList[num].playerBalanceTxt.text = totalBalance.ToString(CultureInfo.InvariantCulture);
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


