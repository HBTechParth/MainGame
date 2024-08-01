using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class JhandiMundaAIManager : MonoBehaviour
{
    public static JhandiMundaAIManager Instance;

    public Sprite[] chipSprites;

    public List<GameObject> playerProfilesLocation;

    public bool isActive = false;

    public int[] totalBetValues;

    private Dictionary<int, int> _weightDictionary = new Dictionary<int, int>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        // Initialize the weight dictionary
        _weightDictionary.Add(0, 5); // Number 0 has a weight of 5
        _weightDictionary.Add(1, 5); // Number 1 has a weight of 5
        _weightDictionary.Add(2, 5); // Number 2 has a weight of 5
        _weightDictionary.Add(3, 5); // Number 3 has a weight of 5
        _weightDictionary.Add(4, 5); // Number 4 has a weight of 5
        _weightDictionary.Add(5, 5); // Number 5 has a weight of 5
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
        PlaceChipOnBoard(place);
    }

    public void PlaceChipOnBoard(int position)
    {
        switch (position)
        {

            case 0:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(JhandiMundaManager.Instance.minFirstSymbolX, JhandiMundaManager.Instance.maxFirstSymbolX), Random.Range(JhandiMundaManager.Instance.minFirstSymbolY, JhandiMundaManager.Instance.maxFirstSymbolY));
                    int firstSymbolChipsIndex = Random.Range(0, chipSprites.Length);
                    totalBetValues[0] += (int)JhandiMundaManager.Instance.chipValue[firstSymbolChipsIndex];
                    //Vector3 randomPosition = new Vector3(minSixthSymbolX, minSixthSymbolY);
                    GameObject chip = Instantiate(JhandiMundaManager.Instance.chipPrefab, JhandiMundaManager.Instance.symbolArea[0]);
                    int firstSymbolspawnLocation = Random.Range(0, playerProfilesLocation.Count);
                    chip.transform.GetComponent<Image>().sprite = chipSprites[firstSymbolChipsIndex];
                    chip.transform.position = playerProfilesLocation[firstSymbolspawnLocation].transform.position;
                    JhandiMundaManager.Instance.firstSymbolChips.Add(chip);
                    JhandiMundaManager.Instance.ChipGenerate(chip, randomPosition);
                    JhandiMundaManager.Instance.totalBetText[0].text = totalBetValues[0].ToString();
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }
            case 1:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(JhandiMundaManager.Instance.minSecondSymbolX, JhandiMundaManager.Instance.maxSecondSymbolX), Random.Range(JhandiMundaManager.Instance.minSecondSymbolY, JhandiMundaManager.Instance.maxSecondSymbolY));
                    int secondSymbolChipsIndex = Random.Range(0, chipSprites.Length);
                    totalBetValues[1] += (int)JhandiMundaManager.Instance.chipValue[secondSymbolChipsIndex];
                    //Vector3 randomPosition = new Vector3(minSixthSymbolX, minSixthSymbolY);
                    GameObject chip = Instantiate(JhandiMundaManager.Instance.chipPrefab, JhandiMundaManager.Instance.symbolArea[1]);
                    int secondSymbolspawnLocation = Random.Range(0, playerProfilesLocation.Count);
                    chip.transform.GetComponent<Image>().sprite = chipSprites[secondSymbolChipsIndex];
                    chip.transform.position = playerProfilesLocation[secondSymbolspawnLocation].transform.position;
                    JhandiMundaManager.Instance.secondSymbolChips.Add(chip);
                    JhandiMundaManager.Instance.ChipGenerate(chip, randomPosition);
                    JhandiMundaManager.Instance.totalBetText[1].text = totalBetValues[1].ToString();
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }


            case 2:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(JhandiMundaManager.Instance.minThirdSymbolX, JhandiMundaManager.Instance.maxThirdSymbolX), Random.Range(JhandiMundaManager.Instance.minThirdSymbolY, JhandiMundaManager.Instance.maxThirdSymbolY));
                    int thirdSymbolChipsIndex = Random.Range(0, chipSprites.Length);
                    totalBetValues[2] += (int)JhandiMundaManager.Instance.chipValue[thirdSymbolChipsIndex];
                    //Vector3 randomPosition = new Vector3(minSixthSymbolX, minSixthSymbolY);
                    GameObject chip = Instantiate(JhandiMundaManager.Instance.chipPrefab, JhandiMundaManager.Instance.symbolArea[2]);
                    int thirdSymbolspawnLocation = Random.Range(0, playerProfilesLocation.Count);
                    chip.transform.GetComponent<Image>().sprite = chipSprites[thirdSymbolChipsIndex];
                    chip.transform.position = playerProfilesLocation[thirdSymbolspawnLocation].transform.position;
                    JhandiMundaManager.Instance.thirdSymbolChips.Add(chip);
                    JhandiMundaManager.Instance.ChipGenerate(chip, randomPosition);
                    JhandiMundaManager.Instance.totalBetText[2].text = totalBetValues[2].ToString();
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }

            case 3:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(JhandiMundaManager.Instance.minFourthSymbolX, JhandiMundaManager.Instance.maxFourthSymbolX), Random.Range(JhandiMundaManager.Instance.minFourthSymbolY, JhandiMundaManager.Instance.maxFourthSymbolY));
                    int fourthSymbolChipsIndex = Random.Range(0, chipSprites.Length);
                    totalBetValues[3] += (int)JhandiMundaManager.Instance.chipValue[fourthSymbolChipsIndex];
                    //Vector3 randomPosition = new Vector3(minSixthSymbolX, minSixthSymbolY);
                    GameObject chip = Instantiate(JhandiMundaManager.Instance.chipPrefab, JhandiMundaManager.Instance.symbolArea[3]);
                    int fourthSymbolspawnLocation = Random.Range(0, playerProfilesLocation.Count);
                    chip.transform.GetComponent<Image>().sprite = chipSprites[fourthSymbolChipsIndex];
                    chip.transform.position = playerProfilesLocation[fourthSymbolspawnLocation].transform.position;
                    JhandiMundaManager.Instance.fourthSymbolChips.Add(chip);
                    JhandiMundaManager.Instance.ChipGenerate(chip, randomPosition);
                    JhandiMundaManager.Instance.totalBetText[3].text = totalBetValues[3].ToString();
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }

            case 4:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(JhandiMundaManager.Instance.minFifthSymbolX, JhandiMundaManager.Instance.maxFifthSymbolX), Random.Range(JhandiMundaManager.Instance.minFifthSymbolY, JhandiMundaManager.Instance.maxFifthSymbolY));
                    int fifthSymbolChipsIndex = Random.Range(0, chipSprites.Length);
                    totalBetValues[4] += (int)JhandiMundaManager.Instance.chipValue[fifthSymbolChipsIndex];
                    //Vector3 randomPosition = new Vector3(minSixthSymbolX, minSixthSymbolY);
                    GameObject chip = Instantiate(JhandiMundaManager.Instance.chipPrefab, JhandiMundaManager.Instance.symbolArea[4]);
                    int fifthSymbolspawnLocation = Random.Range(0, playerProfilesLocation.Count);
                    chip.transform.GetComponent<Image>().sprite = chipSprites[fifthSymbolChipsIndex];
                    chip.transform.position = playerProfilesLocation[fifthSymbolspawnLocation].transform.position;
                    JhandiMundaManager.Instance.fifthSymbolChips.Add(chip);
                    JhandiMundaManager.Instance.ChipGenerate(chip, randomPosition);
                    JhandiMundaManager.Instance.totalBetText[4].text = totalBetValues[4].ToString();
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }

            case 5:
                {
                    SoundManager.Instance.ThreeBetSound();
                    Vector3 randomPosition = new Vector3(Random.Range(JhandiMundaManager.Instance.minSixthSymbolX, JhandiMundaManager.Instance.maxSixthSymbolX), Random.Range(JhandiMundaManager.Instance.minSixthSymbolY, JhandiMundaManager.Instance.maxSixthSymbolY));
                    int sixthSymbolChipsIndex = Random.Range(0, chipSprites.Length);
                    totalBetValues[5] += (int)JhandiMundaManager.Instance.chipValue[sixthSymbolChipsIndex];
                    //Vector3 randomPosition = new Vector3(minSixthSymbolX, minSixthSymbolY);
                    GameObject chip = Instantiate(JhandiMundaManager.Instance.chipPrefab, JhandiMundaManager.Instance.symbolArea[5]);
                    int sixthSymbolspawnLocation = Random.Range(0, playerProfilesLocation.Count);
                    chip.transform.GetComponent<Image>().sprite = chipSprites[sixthSymbolChipsIndex];
                    chip.transform.position = playerProfilesLocation[sixthSymbolspawnLocation].transform.position;
                    JhandiMundaManager.Instance.sixthSymbolChips.Add(chip);
                    JhandiMundaManager.Instance.ChipGenerate(chip, randomPosition);
                    JhandiMundaManager.Instance.totalBetText[5].text = totalBetValues[5].ToString();
                    //print(randomPosition);
                    //totalBet += (int)chipValue[selectedChipNo];
                    break;
                }

        }
    }
}
