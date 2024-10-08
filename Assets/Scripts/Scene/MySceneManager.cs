using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public List<SceneManagerEnum> waves;

    //Scene starting delay
    public float onBeginDelay = 3.0f;

    public GameObject warningIndicator;
    private bool _started = false;

    private float _1Timer = 0.0f;
    private float _timer = 0.0f;

    private int _listMaxIndex = 0;
    private int _currentIndex = 0;
    private int _lastIndex = -1;

    private float _currentWaveTimer = 0.0f;

    private EnemyWavesScriptableObject _wave;
    private List<EnemyWave> _currentEnemyWave;

    private bool _waitForObject = true;

    private bool _ended = false;

    private float _endTimer = 0.0f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        _listMaxIndex = waves.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!_started && _1Timer >= onBeginDelay)
        {
            _started = true;
            _1Timer = 0.0f;
        }
        else
        {
            _1Timer += Time.deltaTime;
        }

        if (_started && !_ended)
        {
            if (!_waitForObject)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                _timer = 0;
            }

            if (_currentIndex != _lastIndex)
            {
                _lastIndex = _currentIndex;
                _currentWaveTimer = waves[_currentIndex].TotalWaveTime;
                _wave = waves[_currentIndex].Wave;
                _currentEnemyWave = _wave.EnemyWavesList;
                _waitForObject = true;
                _wave.SetWarningIndicator(warningIndicator);
            }

            if (_timer >= _currentWaveTimer)
            {
                _timer = 0.0f;
                _currentIndex++;
                
                if (_currentIndex > _listMaxIndex)
                {
                    _ended = true;
                    Debug.Log("All scripted scenes played!!");
                    OnSceneFinish();
                    //Destroy(this.gameObject);
                }
            }
            else
            {
                if (_waitForObject)
                {
                    //If this returns true, all waves have been played, continuing to next scene, but first we'll need to wait for delay
                    if (_wave.UpdateEnemyWave(Time.deltaTime))
                    {
                        //The stupid bug was adding + 1 in the _currentIndex, estoy cansado jefe -d
                        //Start counting delay between scenes
                        _waitForObject = false;
                        _timer = 0.0f;
                    }
                }
            }
            
        }else if (_ended)
        {
            OnSceneFinish();
        }
    }
    
    /// <summary>
    /// Handle Post-scene logic here...
    /// This function gets called when the time for the scene has run out.
    /// </summary>
    private void OnSceneFinish()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0) //No Enemies Left
        {
            _endTimer += Time.deltaTime;
            if (_endTimer >= 2)
            {
                //@TODO: Change Win Screen placeholder
                GameObject.FindGameObjectWithTag("Player").SendMessage("SaveData");
                // Disabled Win screen temporarily
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}

[Serializable]
public struct SceneManagerEnum
{
    public EnemyWavesScriptableObject Wave;
    //Total wave time exits to avoid being stuck in a non-functioning wave
    public float TotalWaveTime;
}
