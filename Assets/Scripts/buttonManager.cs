using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonManager : MonoBehaviour
{
    public GameObject pausePanel;
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found!");
        }
    }

    public void sound()
    {
        if (audioManager != null)
            audioManager.playSFX(audioManager.click, 1);
    }

    public void pause()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        pausePanel.SetActive(true);
        audioManager.musicSource.Pause();
    }

    public void resume()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        pausePanel.SetActive(false);
        audioManager.musicSource.UnPause();
    }

    public void startGame()
    {
        Debug.Log("start");
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("Start");
    }

    public void settings()
    {
        Debug.Log("Settings");
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("Settings");
    }

    public void howToPlay()
    {
        Debug.Log("Howtoplay");
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("Howtoplay");
    }

    public void quit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void home()
    {
        Debug.Log("home");
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene("Start");
    }

    public void restart(int sceneId)
    {
        Debug.Log("restarting level: " + sceneId);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        SceneManager.LoadScene(sceneId);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("unlockedLevel");
        PlayerPrefs.Save();
        Debug.Log("Progress Reset");
    }

}