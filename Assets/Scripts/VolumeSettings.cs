using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        // Initialize slider values from PlayerPrefs or defaults
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            // Set default values if none exist
            musicSlider.value = 0.3f;
            SFXSlider.value = 0.3f;
            setMusicVolume();
            setSFXVolume();
        }

        // Add listeners for real-time updates
        musicSlider.onValueChanged.AddListener(delegate { setMusicVolume(); });
        SFXSlider.onValueChanged.AddListener(delegate { setSFXVolume(); });
    }

    public void setMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);

        // Update AudioManager if it exists
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetMusicVolume(volume);
        }
    }

    public void setSFXVolume()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);

        // Update AudioManager if it exists
        if (AudioManager.instance != null)
        {
            AudioManager.instance.SetSFXVolume(volume);
        }
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        setMusicVolume();
        setSFXVolume();
    }
}