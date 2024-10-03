using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public AudioSource Source;

    public List<AudioClip> Explosions = new List<AudioClip>();

    public float lifetime = 1;

    private float _timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        Source.clip = Explosions[UnityEngine.Random.Range(0, Explosions.Count)];
        Source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
