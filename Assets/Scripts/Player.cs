using UnityEngine;

public class Player : MonoBehaviour
{
    public float health = 100f; // Player's starting health

    public Healthbar healthbar;


    private void Start() {
        healthbar.SetMaxHealth(health);
    }


    public void TakeDamage(float amount)
    {

        health -= amount; // Subtract the damage amount from the player's health
        Debug.Log("Player took " + amount + " damage.");
        healthbar.SetHealth(health);

        if (health <= 0) // Check if the player's health is depleted
        {
            Die(); // Call the Die method if health is 0 or less
        }
    }

    void Die()
    {
        Debug.Log("Player died.");
        FindObjectOfType<GameManager>().GameOver();

    }
}
