using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class PlayerHealthSystem : MonoBehaviour
{
    public int shield = 0;
    public float maxHealth = 100.0f;
    public float currentHealth;
    
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
        GameObject.Find("GUI").SendMessage("SetHealth", currentHealth/maxHealth);
    }

    /// <summary>
    ///  Custom damage event because unity doesn't have one
    /// </summary>
    /// <param name="damage">amount of damage to deal</param>
    public void DoDamage(float damage)
    {
        if (shield > 0)
        {
            Debug.Log("Shiled!!");
            shield--;
        }
        else
        {
            currentHealth -= damage;
            GameObject.Find("GUI").SendMessage("SetHealth", currentHealth/maxHealth);

            Debug.Log("Player Ouch: " + currentHealth);

            if (currentHealth <= 0)
            {
                Debug.Log("I'm Ded");
                currentHealth = 0;
                //@TODO:Delagate for UI?
                //@TODO:Do something fancy (particles...etc)
                Destroy(this.gameObject);
                SceneManager.LoadScene("LoseScreen");
            }
        }
    }
    
    /// <summary>
    /// Gain Health void
    ///     - amount: amount to regen
    /// </summary>
    /// <param name="amount"></param>
    public void GainHealth(float amount)
    {
        currentHealth += MathF.Abs(amount);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentHealth += MathF.Abs(amount);
    }
}
