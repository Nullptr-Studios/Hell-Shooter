using UnityEngine;

public class CritAnim : MonoBehaviour
{
    private float timer;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponentInChildren<SpriteRenderer>().color = new Color(Random.Range(0.5f,1f), Random.Range(0.5f,1f), Random.Range(0.5f,1f));
        transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-20f, 20f));
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Kill child
        if (timer >= 1f) Destroy(this.gameObject);
    }
}
