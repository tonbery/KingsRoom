using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Input = UnityEngine.Input;

public class PlayerController : MonoBehaviour
{
    [Header("Self References")] 
    [SerializeField] private Camera mainCamera;
    
    [Header("Properties")]
    [SerializeField] private float gravity = 10;
    [SerializeField] private float lookSpeed = 3;
    [SerializeField] private float walkSpeed = 7;
    
    private Vector3 _inputMove;
    private Vector2 _inputLook;
    private Vector3 _finalVelocity;
    private float _verticalSpeed;

    private Vector3 _characterRotation;
    private Vector3 _cameraRotation;

    private CharacterController _characterController;

    public Camera MainCamera => mainCamera;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _inputMove.x = Input.GetAxis("Horizontal");
        _inputMove.z = Input.GetAxis("Vertical");
        
        _inputLook.y = Input.GetAxis("Mouse X");
        _inputLook.x = -Input.GetAxis("Mouse Y");
        
        _characterRotation.y += _inputLook.y * Time.fixedDeltaTime * lookSpeed;
        _cameraRotation.x += _inputLook.x * Time.fixedDeltaTime * lookSpeed;
        _cameraRotation.x = Mathf.Clamp(_cameraRotation.x, -50, 50);

        transform.eulerAngles = _characterRotation;
        mainCamera.transform.localEulerAngles = _cameraRotation;
    }
    
    void FixedUpdate()
    {
        var cameraTransform = mainCamera.transform;
        _finalVelocity = cameraTransform.forward * _inputMove.z + cameraTransform.right * _inputMove.x;
        if (!_characterController.isGrounded) _verticalSpeed -= gravity * Time.fixedDeltaTime;
        else _verticalSpeed = -1;
        _finalVelocity.y = _verticalSpeed;

        _characterController.Move(_finalVelocity * (walkSpeed * Time.fixedDeltaTime));
    }
}
