using System;
using System.Collections.Generic;
using UnityEngine;

public class ControllingLightOnScene : MonoBehaviour
{
    //LightOff сделать приватным когда придумаем условие выключения света
    
    private List<GameObject> _lights;

    public bool IsLightOn = true;
    
    private void Start()
    {
        _lights = FindGameObjectsInLayer(8);
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
    
    public void LightOff()
    {
        foreach (var light in _lights)
        {
            light.SetActive(false);
        }

        IsLightOn = false;
    }
}
