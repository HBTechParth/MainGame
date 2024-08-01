using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenAd : MonoBehaviour
{
    public Image bannerImage;
    public GameObject fullscreenPopup;
    
    public event Action OnBannerClosed;
    public void ClosePopUp()
    {
        SoundManager.Instance.ButtonClick();
        if (fullscreenPopup == null) return;
        Image bannerImage = fullscreenPopup.GetComponent<Image>();
            
        fullscreenPopup.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack) // Scale from 1 to 0
            .OnComplete(() =>
            {
                bannerImage.DOFade(0f, 0.5f);
                OnBannerClosed?.Invoke(); // Invoke the event when the banner is closed
                OnBannerClosed = null; // Unsubscribe from the event
                Destroy(fullscreenPopup, 0.5f);
            });
    }
}