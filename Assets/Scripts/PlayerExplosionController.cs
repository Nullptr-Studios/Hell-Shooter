using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosPlayerionController : MonoBehaviour
{
    public AudioSource Source;

    public List<AudioClip> Explosions = new List<AudioClip>();

    public SpriteRenderer spr;

    public float lifetime = 1;
    public float timeToSceneChange = 3;

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
            //Destroy(gameObject);
            spr.color = Color.clear;
            if (_timer > timeToSceneChange)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        
    }
}
