using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private InputReader inputReader;
    
    [FormerlySerializedAs("playerSpeed")]
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 2.0f;
    [SerializeField] private float rotationSpeed = 2.0f;
    
    [Header("Camera")]
    [SerializeField] private CinemachineCamera fpsCam;

    private Vector3 _moveDirection;
    private Vector2 _lookDirection;
    private float _camVerticalAngle = 0f;
    private bool _groundedPlayer;
    private Vector3 _playerVelocity;
    private Transform _camTransform;
    private float _gravityValue = -9.81f;
    

    private void OnEnable()
    {
        inputReader.MoveEvent += Move;
        inputReader.LookEvent += Look;
    }

    private void OnDisable()
    {
        inputReader.MoveEvent -= Move;
        inputReader.LookEvent -= Look;
    }

    private void Start()
    {
        _camTransform = fpsCam.transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        _groundedPlayer = controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }
        
        // Rotate character horizontally, rotate cam vertically
        transform.Rotate(0, _lookDirection.x * rotationSpeed * Time.deltaTime, 0);
        _camVerticalAngle -= _lookDirection.y * rotationSpeed * Time.deltaTime;
        _camVerticalAngle = Mathf.Clamp(_camVerticalAngle, -89f, 89f);
        fpsCam.transform.localRotation = Quaternion.Euler(_camVerticalAngle, 0f, 0f);
        
        Vector3 move = _camTransform.forward * _moveDirection.z + _camTransform.right * _moveDirection.x;
        move.y = 0f;
        controller.Move(Time.deltaTime * movementSpeed * move);

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        controller.Move(_playerVelocity * Time.deltaTime);
        
        //transform.Rotate(-_lookDirection.y, _lookDirection.x, 0f);
    }

    private void Move(Vector2 direction)
    {
        _moveDirection = new Vector3(direction.x, 0f, direction.y);
    }

    private void Look(Vector2 direction)
    {
        _lookDirection = direction;
    }
}
