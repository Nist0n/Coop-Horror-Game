using System;
using Unity.Netcode;
using UnityEngine;

public class CameraRotating : MonoBehaviour
{
    [SerializeField] private Transform orientation;
    [SerializeField] private float senX;
    [SerializeField] private float senY;

    private float _xRotation;
    private float _yRotation;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * senX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * senY;

        _yRotation += mouseX;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        
        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
