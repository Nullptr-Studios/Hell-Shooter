using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WarningIndicatorLogic : MonoBehaviour
{
    public float destroyTime = 1;

    private float _timer = 0.0f;

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= destroyTime)
        {
            Destroy(this.GameObject());
        }
    }
}
