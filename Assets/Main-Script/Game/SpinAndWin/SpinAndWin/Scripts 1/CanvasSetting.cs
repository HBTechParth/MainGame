using UnityEngine;

public class CanvasSetting : MonoBehaviour
{
    private UnityEngine.UI.CanvasScaler canvasScaler;

    private void Awake()
    {
        canvasScaler = GetComponent<UnityEngine.UI.CanvasScaler>();
    }

    private void Start()
    {
        SetMatchRatio();
    }

    private void SetMatchRatio()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Reference resolution
        float referenceWidth = 1080f;
        float referenceHeight = 1920f;

        // Calculate the aspect ratios
        float targetAspectRatio = referenceWidth / referenceHeight;
        float currentAspectRatio = screenWidth / screenHeight;

        // Set matchWidthOrHeight to 0.6
        canvasScaler.matchWidthOrHeight = 0.52f;

        // Optionally, you can also set the reference resolution
        canvasScaler.referenceResolution = new Vector2(referenceWidth, referenceHeight);
    }
}
