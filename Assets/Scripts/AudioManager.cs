using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip backggroundMusic;
    public AudioClip lvl1bgMusic;
    public AudioClip lvl2bgMusic;
    public AudioClip lvl3bgMusic;
    public AudioClip playerAttack;
    public AudioClip enemyAttack;
    public AudioClip lavaAndMetalHurt;
    public AudioClip walking;
    public AudioClip click;
    public AudioClip doorOpening;
    public AudioClip key;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip jump;
    public AudioClip heart;

    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Initialize volumes from PlayerPrefs
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            float savedMusicVol = PlayerPrefs.GetFloat("musicVolume");
            float savedSFXVol = PlayerPrefs.GetFloat("SFXVolume");
            SetMusicVolume(savedMusicVol);
            SetSFXVolume(savedSFXVol);
        }
        else
        {
            // Default values if no PlayerPrefs exist
            SetMusicVolume(0.3f);  // 30% volume as default
            SetSFXVolume(0.3f);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name);

        musicSource.volume = musicVolume;
        SFXSource.volume = sfxVolume;

        if (scene.name == "Start")
        {
            PlayMusic(backggroundMusic);
        }
        else if (scene.name.Contains("Level"))
        {
            // Handle level-specific music
            switch (scene.name)
            {
                case "Level1":
                    PlayMusic(lvl1bgMusic);
                    break;
                case "Level2":
                    PlayMusic(lvl2bgMusic);
                    break;
                case "Level3-1":
                    PlayMusic(lvl3bgMusic);
                    break;
                default:
                    PlayMusic(backggroundMusic);
                    break;
            }
        }
    }

    private void Start()
    {
        if (musicSource.clip == backggroundMusic)
        {
            musicVolume = 0.1f; // 20% volume
        }

        musicSource.volume = musicVolume;
        SFXSource.volume = sfxVolume;

        

        if (musicSource.clip == null)
        {
            musicSource.clip = backggroundMusic;
        }

        if (!musicSource.isPlaying)
        {
            musicSource.Play();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void playSFX(AudioClip audioClip, float v)
    {
        if (audioClip != null && SFXSource != null)
        {
            SFXSource.PlayOneShot(audioClip);
        }
    }

    public void PlayMusic(AudioClip music)
    {
        if (music == null || musicSource == null) return;

        if (musicSource.clip != music || !musicSource.isPlaying)
        {
            musicSource.clip = music;
            musicSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        if (musicSource != null)
        {
            musicSource.volume = musicVolume;
        }
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (SFXSource != null)
        {
            SFXSource.volume = sfxVolume;
        }
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void PauseMusic()
    {
        if (musicSource != null)
        {
            musicSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }
}