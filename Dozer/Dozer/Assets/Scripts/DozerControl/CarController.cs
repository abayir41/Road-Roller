using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float velocityMultiplier;
    [SerializeField] private float angularVelocityMultiplier;
    private PlayerController _playerController;
    private Rigidbody _rigidbody;
    private ISteerSystem _steerSystem;
    private GameStatus _status;

    public void SetVelocity(int maxGrowPoint)
    {
        velocityMultiplier = (float)_playerController.Score / maxGrowPoint * 20f + 3f;
    }

    private void OnEnable()
    {
        ActionSys.GameStatusChanged += GameStatusChanged;
    }
    
    private void OnDisable()
    {
        ActionSys.GameStatusChanged -= GameStatusChanged;
    }
    
    private void GameStatusChanged(GameStatus status)
    {
        _status = status;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.inertiaTensorRotation = Quaternion.identity;
        _steerSystem = GetComponent<ISteerSystem>();
        _rigidbody.velocity = transform.forward * velocityMultiplier;
    }

    private void Update()
    {
        if (_status == GameStatus.Paused)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            return;
        }
        
        _rigidbody.velocity = transform.forward * velocityMultiplier;
        
        var angle = _steerSystem.Angle;
        var registeredTurns = (int)Math.Abs(angle) / 90;
        var lastAngle = ((int) Math.Abs(angle) % 90) * Utilities.PosOrNeg(angle);
        var playerInput = Mathf.Sin(lastAngle * Mathf.Deg2Rad);
        playerInput += registeredTurns * Utilities.PosOrNeg(angle);
        _rigidbody.angularVelocity = Vector3.up * (angularVelocityMultiplier * playerInput);
    }
}
