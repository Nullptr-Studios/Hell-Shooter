using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHealthSystem : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public float criticalHitMultiplier = 2.0f;
    public int killedXp = 10;
    public int killedGold = 2; 
    
    private PlayerStats playerStats;
    
    // Start is called before the first frame update
    void Awake()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        currentHealth = maxHealth;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void ChangeMaxHealth(float newMax)
    {
        this.maxHealth = newMax;
        currentHealth = maxHealth;
    }

    /// <summary>
    ///  Custom damage event
    /// 
    /// </summary>
    /// <param name="damage">amount of damage to deal</param>
    public void DoDamage(float damage)
    {
        //@TODO: Change this to bullet
        var damageMultiplier = playerStats.GetStat(StatID.damageMultiplier);
        var critHitPercentage = playerStats.GetStat(StatID.criticalHitPercentage);
        bool _isCrit = false;
        if (playerStats.GetStatLevel(StatID.criticalHitPercentage) > 1)
        {
            if (Random.Range(0, 100) % Mathf.RoundToInt(critHitPercentage * 8) == 0)
                _isCrit = true;
        }

        currentHealth -= damage * damageMultiplier * (_isCrit ? criticalHitMultiplier : 1);
        
        // Debug.Log("Ouch: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            // Debug.Log("I'm Ded");
            currentHealth = 0;
            playerStats.GiveXP(killedXp);
            playerStats.GiveGold(killedGold);
            playerStats.GiveScore(killedXp * (int)maxHealth);
            //Idunno if unity has something like delegates or event notifies in Unreal, in order to keep count of dead enemies and it's score
            //@TODO:Do something fancy (particles...etc)
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Gain Health void
    /// </summary>
    /// <param name="amount">Amount of health to regen</param>
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
