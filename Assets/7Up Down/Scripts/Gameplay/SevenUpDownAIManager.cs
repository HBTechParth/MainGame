using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SevenUpDownAIManager : MonoBehaviour
{
    public static SevenUpDownAIManager Instance;

    public Sprite[] chipSprites;

    public List<GameObject> playerProfilesLocation;

    public bool isActive = false;

    private Dictionary<int, int> _weightDictionary = new Dictionary<int, int>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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
        if(isActive)
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
            case 1://7 Down
                SoundManager.Instance.ThreeBetSound();
                Vector3 downPosition = new Vector3(Random.Range(SevenUpDownManager.Instance.min7Downx, SevenUpDownManager.Instance.max7Downx), Random.Range(SevenUpDownManager.Instance.min7Downy, SevenUpDownManager.Instance.max7Downy));
                int downchipIndex = Random.Range(0, chipSprites.Length);
                GameObject downchip = Instantiate(SevenUpDownManager.Instance.chipPrefab, SevenUpDownManager.Instance.downArea);
                int downspawnLocation = Random.Range(0, playerProfilesLocation.Count);
                downchip.GetComponent<Image>().sprite = chipSprites[downchipIndex];
                downchip.transform.position = playerProfilesLocation[downspawnLocation].transform.position;
                SevenUpDownManager.Instance.downChips.Add(downchip);
                SevenUpDownManager.Instance.ChipGenerate(downchip, downPosition);
                break;
            case 2://7 Up
                SoundManager.Instance.ThreeBetSound();
                Vector3 upPosition = new Vector3(Random.Range(SevenUpDownManager.Instance.min7Upx, SevenUpDownManager.Instance.max7Upx), Random.Range(SevenUpDownManager.Instance.min7Upy, SevenUpDownManager.Instance.max7Upy));
                int upchipIndex = Random.Range(0, chipSprites.Length);
                GameObject upchip = Instantiate(SevenUpDownManager.Instance.chipPrefab, SevenUpDownManager.Instance.upArea);
                int upspawnLocation = Random.Range(0, playerProfilesLocation.Count);
                upchip.GetComponent<Image>().sprite = chipSprites[upchipIndex];
                upchip.transform.position = playerProfilesLocation[upspawnLocation].transform.position;
                SevenUpDownManager.Instance.upChips.Add(upchip);
                SevenUpDownManager.Instance.ChipGenerate(upchip, upPosition);
                break;
            case 3://7
                SoundManager.Instance.ThreeBetSound();
                Vector3 onPosition = new Vector3(Random.Range(SevenUpDownManager.Instance.min7Onx, SevenUpDownManager.Instance.max7Onx), Random.Range(SevenUpDownManager.Instance.min7Ony, SevenUpDownManager.Instance.max7Ony));
                int onchipIndex = Random.Range(0, chipSprites.Length);
                GameObject onchip = Instantiate(SevenUpDownManager.Instance.chipPrefab, SevenUpDownManager.Instance.onArea);
                int onspawnLocation = Random.Range(0, playerProfilesLocation.Count);
                onchip.GetComponent<Image>().sprite = chipSprites[onchipIndex];
                onchip.transform.position = playerProfilesLocation[onspawnLocation].transform.position;
                SevenUpDownManager.Instance.onChips.Add(onchip);
                SevenUpDownManager.Instance.ChipGenerate(onchip, onPosition);
                break;

        }
    }
}
