using System;
using UnityEngine;

public class FixLight : MonoBehaviour
{
    [SerializeField] private GameObject enableSign;
    
    private ControllingLightOnScene _controllingLight;
    private PlayerMovement _player;
    private readonly float _timeToEnd = 4f;
    private float _timer;
    private bool _isButtonPressed = false;

    private void Start()
    {
        _controllingLight = FindAnyObjectByType<ControllingLightOnScene>().GetComponent<ControllingLightOnScene>();
    }

    private void Update()
    {
        if (_isButtonPressed)
        {
            _timer += Time.deltaTime;
            if (_timer >= _timeToEnd)
            {
                _controllingLight.LightOn();
                enableSign.SetActive(false);
                _timer = 0;
                _isButtonPressed = false;
                _player.IsActioning = false;
            }
        }
        else
        {
            _timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_controllingLight.IsLightOn && !enableSign.activeSelf)
        {
            if (other.CompareTag("Player"))
            {
                _player = other.GetComponent<PlayerMovement>();
                enableSign.SetActive(true);
            }
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_controllingLight.IsLightOn)
            {
                if (_player == null)
                {
                    _player = other.GetComponent<PlayerMovement>();
                }
                
                if (!enableSign.activeSelf)
                {
                    enableSign.SetActive(true);
                }

                if (Input.GetKey(KeyCode.E))
                {
                    _isButtonPressed = true;
                    _player.IsActioning = true;
                }
                else
                {
                    _isButtonPressed = false;
                    _player.IsActioning = false;
                }
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (enableSign.activeSelf)
        {
            if (other.CompareTag("Player"))
            {
                enableSign.SetActive(false);
            }
        }
    }
}
