using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static UnityEngine.Rendering.DebugUI;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    public HealthbarScript healthbar;

    public GameObject failedPanel;

    // first
    AudioManager audioManager;

    //second

    private void Awake()
    {
        currentHealth = startingHealth;
        healthbar.setMaxHealth(startingHealth);
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();

        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogWarning("AudioManager not found!");
        }

    }
    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        healthbar.setHealth(currentHealth);

        if (currentHealth > 0)
        {
            Debug.Log(currentHealth);
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                Debug.Log("patay");

                // Disable all components
                foreach (Behaviour component in components)
                    component.enabled = false;

                // Disable movement and AI scripts
                if (GetComponent<PlayerMovement>() != null)
                {
                    Debug.Log("patay character");
                    GetComponent<PlayerMovement>().enabled = false;
                    Debug.Log("PlayerMovement enabled: " + GetComponentInParent<PlayerMovement>().enabled);
                    StartCoroutine(WaitBeforePause());
                }

                if (GetComponentInParent<EnemyPatrol>() != null)
                {
                    
                    GetComponentInParent<EnemyPatrol>().enabled = false;
                    
                    
                }    

                if (GetComponent<MeleeEnemy>() != null)
                {
                    Debug.Log("patay enemy(1)");
                    GetComponent<MeleeEnemy>().enabled = false;
                }

                // ? Subtract from enemy counter if this object is tagged as Enemy
                if (gameObject.CompareTag("Enemy") && EnemyCounter.instance != null)
                {
                    EnemyCounter.instance.SubtractEnemy();
                }


                dead = true;
            }

        }
    }

    private IEnumerator WaitBeforePause()
    {

        failedPanel.SetActive(true); // show panel right away
                                     //third
        if (audioManager != null)
            audioManager.playSFX(audioManager.lose, 1);

        yield return new WaitForSecondsRealtime(3f); // wait 1 second (unaffected by timeScale)
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        healthbar.setHealth(currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("health") && currentHealth < startingHealth)
        {
            Debug.Log("additional health");
            AddHealth(30);
            if (audioManager != null)
                audioManager.playSFX(audioManager.heart, 1);
            Destroy(collision.gameObject);
        }
    }



    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        TakeDamage(1);
    //    }
    //}
}