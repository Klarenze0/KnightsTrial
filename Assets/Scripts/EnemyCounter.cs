using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyCounter : MonoBehaviour
{
    public static EnemyCounter instance;

    public int enemiesleft = 8;

    public Animator anim;
    public GameObject winPanel;
    public GameObject losePanel;
    bool allEnemyDead = false;

    public GameObject player;
    private PlayerMovement playerMovement;

    // first
    AudioManager audioManager;




    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found!");
        }
    }

    public void SubtractEnemy()
    {
        enemiesleft--;

        if (enemiesleft <= 0)
        {
            enemiesleft = 0;
            OnAllEnemiesDefeated();
        }
    }

    private void OnGUI()
    {
        GUIStyle textStyle = new GUIStyle(GUI.skin.label);
        textStyle.fontSize = 40;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;

        string labelText = enemiesleft.ToString();

        float width = 250;
        float height = 50;
        float x = Screen.width - width;
        float y = 25;

        GUI.Label(new Rect(x, y, width, height), labelText, textStyle);
    }


    private void OnAllEnemiesDefeated()
    {
        Debug.Log("All enemies defeated!");

        anim.SetBool("enemyZero", true);
        allEnemyDead = true;
        Debug.Log("gate open");
        //third
        if (audioManager != null)
            audioManager.playSFX(audioManager.doorOpening, 1);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        bool playerHasKey = playerMovement.hasKey;

        if (collision.CompareTag("door"))
        {
            if (allEnemyDead && playerHasKey)
            {
                //third
                if (audioManager != null)
                    audioManager.playSFX(audioManager.win, 1);
                Debug.Log("going to the next level");

                // UNLOCK NEXT LEVEL / PARA SA PLAYERPREFS
                int currentLevel = SceneManager.GetActiveScene().buildIndex;
                int unlockedLevel = PlayerPrefs.GetInt("unlockedLevel", 1);
                if (currentLevel >= unlockedLevel)
                {
                    PlayerPrefs.SetInt("unlockedLevel", currentLevel + 1);
                    PlayerPrefs.Save();
                }

                winPanel.SetActive(true);
                Time.timeScale = 0f;
                //SceneManager.LoadScene("Level2");
            }  
        }

        if (collision.CompareTag("lastlvl"))
        {
            // Stop level music and play main background music
            if (audioManager != null)
            {
                audioManager.musicSource.Stop(); // Stop current level music
                audioManager.PlayMusic(audioManager.backggroundMusic); // Play main menu music
            }

            Time.timeScale = 1f;
            SceneManager.LoadScene("credits");
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("drop"))
        {
            losePanel.SetActive(true);

            if (audioManager != null)
                audioManager.playSFX(audioManager.lose, 1);

            StartCoroutine(PauseAfterDelay(3f)); // 0.1s delay (adjust if needed)
        }
    }

    private IEnumerator PauseAfterDelay(float delay)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < startTime + delay)
        {
            yield return null; // Wait in real time
        }

        AudioListener.pause = true;
        Time.timeScale = 0f;
    }



}
