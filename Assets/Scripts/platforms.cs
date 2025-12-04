using UnityEngine;

public class platforms : MonoBehaviour
{
    public float disappearDelay = 2f; // Wait 2 seconds before disappearing
    public float respawnTime = 3f;    // Wait before reappearing

    private bool isDisappearing = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDisappearing)
        {
            Debug.Log("Player touched me! Will disappear in 2 seconds...");
            isDisappearing = true;
            Invoke("Disappear", disappearDelay);
        }
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
        Invoke("Respawn", respawnTime);
    }

    private void Respawn()
    {
        gameObject.SetActive(true);
        isDisappearing = false;
        Debug.Log("Platform is back!");
    }

}
