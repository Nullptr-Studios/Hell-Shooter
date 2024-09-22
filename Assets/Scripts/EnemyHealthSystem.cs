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
}
