using UnityEngine;

public class credits : MonoBehaviour
{
    public float scrollSpeed = 40f;
    private float stopY = 409.92f;

    private RectTransform rectTransform;

    // first
    AudioManager audioManager;

    //second
    private void Awake()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found!");
        }
    }
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //third
        if (audioManager != null)
            audioManager.playSFX(audioManager.doorOpening, 1);
    }

    void Update()
    {
        if (rectTransform.anchoredPosition.y < stopY)
        {
            rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);

            // Clamp if it overshoots the stop position
            if (rectTransform.anchoredPosition.y > stopY)
            {
                rectTransform.anchoredPosition = new Vector2(
                    rectTransform.anchoredPosition.x,
                    stopY
                );
            }
        }
    }
}
