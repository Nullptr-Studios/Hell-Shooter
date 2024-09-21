using UnityEngine;

public class BulletScript : MonoBehaviour
{
    void Start()
    {
        //Add initial force
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(gameObject.transform.up * 500);
    }

    private void OnBecameInvisible()
    {
        //destroy object outside bounds to save memory
        Destroy(gameObject);
    }
    
    //@TODO: Interaction with enemies
}