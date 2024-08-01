using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public GameObject error;
    public Transform playerDetailsPrefabParent;
    public GameObject playerDetailsPrefab;
    public Sprite firstPlaceTrophy;
    public Sprite secondPlaceTrophy;
    public Sprite thirdPlaceTrophy;

    private Button activeButton; 

    private void Start()
    {
        LoadLeaderBoard();
    }

    private void LoadLeaderBoard()
    {
        string apiUrl = "/api/v1/players/winnertop/daily";
        
        if (string.IsNullOrEmpty(apiUrl)) return;
        ClearPlayerDetails();
        StartCoroutine(GetBoardData(apiUrl));
    }

    #region LeaderBoard
    

    private IEnumerator GetBoardData(string apiUrl)
    {
        error.gameObject.GetComponent<Text>().text = "Please wait...";
        error.gameObject.SetActive(true);

        var url = DataManager.Instance.url + apiUrl;
        var request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Authorization", "Bearer " + PlayerPrefs.GetString("token"));
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            error.gameObject.GetComponent<Text>().text = "Error loading data.";
            error.gameObject.SetActive(true);
        }
        else
        {
            var jsonNode = JSON.Parse(request.downloadHandler.text);

            if (jsonNode["success"].AsBool)
            {
                error.gameObject.SetActive(false);
                var dataArray = jsonNode["data"].AsArray;

                if (dataArray != null && dataArray.Count > 0)
                {
                    for (var i = 0; i < dataArray.Count; i++)
                    {
                        var dataNode = dataArray[i];

                        float balance = dataNode["balance"].AsFloat;
                        var firstName = dataNode["firstName"].Value;
                        var picture = dataNode["picture"].Value;
                        

                        var playerDetailsObj = Instantiate(playerDetailsPrefab, playerDetailsPrefabParent);
                        var playerDetailsTransform = playerDetailsObj.transform;

                        playerDetailsTransform.GetChild(3).GetComponent<Text>().text = balance.ToString();
                        playerDetailsTransform.GetChild(2).GetComponent<Text>().text = firstName;
                        StartCoroutine(GetImages(picture,
                            playerDetailsTransform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>()));

                        var serialNumber = i + 1;
                        playerDetailsTransform.GetChild(0).GetComponent<Text>().text = serialNumber.ToString();

                        var trophyImage = playerDetailsTransform.GetChild(4).GetComponent<Image>();
                        trophyImage.enabled = true;

                        switch (serialNumber)
                        {
                            case 1:
                                trophyImage.sprite = firstPlaceTrophy;
                                break;
                            case 2:
                                trophyImage.sprite = secondPlaceTrophy;
                                break;
                            case 3:
                                trophyImage.sprite = thirdPlaceTrophy;
                                break;
                            default:
                                trophyImage.enabled = false;
                                break;
                        }
                    }
                }
                else
                {
                    error.gameObject.GetComponent<Text>().text = "No data found.";
                    error.gameObject.SetActive(true);
                }
            }
            else
            {
                error.gameObject.GetComponent<Text>().text = "Error loading data.";
                error.gameObject.SetActive(true);
            }
        }
    }
    
    IEnumerator GetImages(string URl, Image image)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URl);
        yield return request.SendWebRequest();

        if (request.error == null)
        {
            var texture = DownloadHandlerTexture.GetContent(request);
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            if (image != null)
            {
                image.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
            }
        }
    }
    
    private void ClearPlayerDetails()
    {
        foreach (Transform child in playerDetailsPrefabParent)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion

    public void BackButtonClick()
    {
        SoundManager.Instance.ButtonClick();
        this.gameObject.SetActive(false);
    }

}
