using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ControllingLightOnScene : MonoBehaviour
{
    private List<GameObject> _lights;
    private float _timer;
    private float _timeToEvent;

    public bool IsLightOn = true;
    
    private void Start()
    {
        RandomEventTime();
        _lights = FindGameObjectsInLayer(8);
    }

    private void Update()
    {
        if (IsLightOn)
        {
            _timer += Time.deltaTime;
            if (_timer >= +_timeToEvent)
            {
                LightOff();
                RandomEventTime();
            }
        }
    }

    /// <summary>
    /// 8 - номер светового слоя
    /// </summary>
    private List<GameObject> FindGameObjectsInLayer(int layer)
    {
        var goArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        var goList = new List<GameObject>();
        
        if (goArray != null)
        {
            for (int i = 0; i < goArray.Length; i++)
            {
                if (goArray[i].layer == layer)
                {
                    goList.Add(goArray[i]);
                }
            }
        }
        
        if (goList.Count == 0)
        {
            Debug.Log("Пусто");
            return null;
        }
        
        return goList;
    }

    public void LightOn()
    {
        foreach (var light in _lights)
        {
            light.SetActive(true);
        }

        IsLightOn = true;
    }
    
    private void LightOff()
    {
        foreach (var light in _lights)
        {
            light.SetActive(false);
        }

        IsLightOn = false;
        _timer = 0;
    }

    private void RandomEventTime()
    {
        float time = Random.Range(240, 360);
        _timeToEvent = time;
    }
}
