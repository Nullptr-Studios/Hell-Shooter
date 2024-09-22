using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    
    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    ///  Custom damage event
    ///     - damage: amount of damage to deal
    /// </summary>
    /// <param name="damage"></param>
    public void DoDamage(float damage)
    {
        currentHealth -= damage;
        
        Debug.Log("Ouch: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Debug.Log("I'm Ded");
            currentHealth = 0;
            //Idunno if unity has something like delegates or event notifies in Unreal, in order to keep count of dead enemies and it's score
            //@TODO:Do something fancy (particles...etc)
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Gain Health void
    ///     - amount: amount to regen
    /// </summary>
    /// <param name="amount"></param>
    public void GainHealth(float amount)
    {
        if (currentHealth >= 100)
        {
            currentHealth = 100;
        }
        else
        {
            currentHealth += MathF.Abs(amount);
        }
    }
}
