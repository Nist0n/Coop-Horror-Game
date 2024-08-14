using UnityEngine;
using UnityEngine.Rendering;

public class GasContrloller : MonoBehaviour
{
    [SerializeField] private Volume _volume;

    public void GasActivator()
    {
        while (_volume.weight < 1)
        {
            _volume.weight = Mathf.Lerp(_volume.weight, 1, Time.deltaTime * 2);
        }
        
        while (_volume.weight > 0.8f)
        {
            _volume.weight = Mathf.Lerp(_volume.weight, 0.8f, Time.deltaTime * 4);
        }
        
        while (_volume.weight < 1)
        {
            _volume.weight = Mathf.Lerp(_volume.weight, 1, Time.deltaTime * 2);
        }
        
        while (_volume.weight > 0.8f)
        {
            _volume.weight = Mathf.Lerp(_volume.weight, 0.8f, Time.deltaTime * 4);
        }
        
        while (_volume.weight < 1)
        {
            _volume.weight = Mathf.Lerp(_volume.weight, 1, Time.deltaTime * 2);
        }
        
        while (_volume.weight > 0.5f)
        {
            _volume.weight = Mathf.Lerp(_volume.weight, 0.5f, Time.deltaTime * 4);
        }
        
        while (_volume.weight < 1)
        {
            _volume.weight = Mathf.Lerp(_volume.weight, 1, Time.deltaTime * 1);
        }
    }
}
