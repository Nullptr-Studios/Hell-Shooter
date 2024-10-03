using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitController : MonoBehaviour
{
    private Transform _tr;

    // Start is called before the first frame update
    private void Awake()
    {
        _tr = transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (_tr.localScale.x <= .1f)
        {
            Destroy(gameObject);
            return;
        }
        
        float l = Mathf.Lerp(_tr.localScale.x, 0, 8f * Time.deltaTime);
        _tr.localScale = new Vector3(l,l,l) ;
        
    }
}
