using System.Collections;
using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    private bool isInLava = false;
    private Coroutine damageCoroutine;
    public Health playerHealth; // Reference to your health script!

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("lava"))
        {
            Debug.Log("Entered lava!");
            if (!isInLava)
            {
                isInLava = true;
                damageCoroutine = StartCoroutine(DamageOverTime());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("lava"))
        {
            Debug.Log("Exited lava!");
            if (isInLava)
            {
                isInLava = false;
                StopCoroutine(damageCoroutine);
            }
        }
    }

    private IEnumerator DamageOverTime()
    {
        while (isInLava)
        {
            //third
            if (audioManager != null)
                audioManager.playSFX(audioManager.lavaAndMetalHurt, 1);
            playerHealth.TakeDamage(30); // call your health script's method to damage player
            yield return new WaitForSeconds(0.5f);
        }
    }
}
