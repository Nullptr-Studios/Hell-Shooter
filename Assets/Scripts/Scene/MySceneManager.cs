using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySceneManager : MonoBehaviour
{
    public List<SceneManagerEnum> waves;

    public float onBeginDelay = 3.0f;
    private bool _started = false;

    private float _timer = 0.0f;

    private int _listMaxIndex = 0;
    private int _currentIndex = 0;
    private int _lastIndex = -1;

    private float _currentWaveTimer = 0.0f;

    private EnemyWavesScriptableObject _wave;
    private List<EnemyWave> _currentEnemyWave;
    
    // Start is called before the first frame update
    void Start()
    {
        _listMaxIndex = waves.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (!_started && _timer >= onBeginDelay)
        {
            _started = true;
            _timer = 0.0f;
        }

        if (_started)
        {
            if (_currentIndex != _lastIndex)
            {
                _lastIndex = _currentIndex;
                _currentWaveTimer = waves[_currentIndex].TotalWaveTime;
                _wave = waves[_currentIndex].Wave;
                _currentEnemyWave = _wave.EnemyWavesList;
            }
            
            if (_timer >= _currentWaveTimer)
            {
                _timer = 0.0f;
                _currentIndex++;
                
                if (_currentIndex > _listMaxIndex)
                {
                    //@TODO: Change
                    Debug.Log("All scripted scenes played!!");
                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (_wave.UpdateEnemyWave(Time.deltaTime))
                {
                    _timer = _currentWaveTimer;
                }
            }
            
        }
    }
}

[System.Serializable]
public struct SceneManagerEnum
{
    public EnemyWavesScriptableObject Wave;
    public float TotalWaveTime;
}
