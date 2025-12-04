using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Needed for Button

public class levelSelect : MonoBehaviour
{
    public Button[] lvlButton; // Assign in inspector: index 0 = Level 1, etc.
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found!");
        }

        // Unlock levels
        int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1); // Default is 1

        for (int i = 0; i < lvlButton.Length; i++)
        {
            if (i < unlockedLevel)
            {
                lvlButton[i].interactable = true;
            }
            else
            {
                lvlButton[i].interactable = false;
                lvlButton[i].GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f); // dark gray
            }
        }
    }

    public void levelMenu(int sceneId)
    {
        if (audioManager != null)
        {
            if (sceneId == 1)
                audioManager.PlayMusic(audioManager.lvl1bgMusic);
            else if (sceneId == 2)
                audioManager.PlayMusic(audioManager.lvl2bgMusic);
            else if (sceneId == 3)
                audioManager.PlayMusic(audioManager.lvl3bgMusic);

            //audioManager.SetMusicVolume(0.3f);
            //audioManager.SetSFXVolume(0.5f);
        }

        SceneManager.LoadScene(sceneId);
    }

    public void back()
    {
        Debug.Log("home");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Mainmenu");
    }

    public void sound()
    {
        if (audioManager != null)
            audioManager.playSFX(audioManager.click, 1);
    }
}
