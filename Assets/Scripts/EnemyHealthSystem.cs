using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHealthSystem : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float currentHealth;
    public float criticalHitMultiplier = 2.0f;
    public int killedXp = 10;

    private PlayerStats playerStats;
    // public int killedGold = 2; prepared for when gold is used

    // Start is called before the first frame update
    void Awake()
    {
        playerStats = GameObject.Find("Player").GetComponent<PlayerStats>();
        currentHealth = maxHealth;
    }

    /// <summary>
    ///  Custom damage event
    ///     - damage: amount of damage to deal
    /// </summary>
    /// <param name="damage"></param>
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
        _isCrit = false;
        
        Debug.Log("Ouch: " + currentHealth);
        
        if (currentHealth <= 0)
        {
            Debug.Log("I'm Ded");
            currentHealth = 0;
            playerStats.GiveXP(killedXp);
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
