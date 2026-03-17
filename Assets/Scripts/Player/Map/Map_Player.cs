using System;
using UnityEngine;

public class Map_Player : MonoBehaviour
{
    public static Map_Player Instance { get; private set; }
    
    public event EventHandler OnPlayerSelectedStage;
    public event EventHandler<OnSelectedStageChangedEventArgs> OnSelectedStageChanged;
    public class OnSelectedStageChangedEventArgs : EventArgs 
    {
        public Map_Stage _selectedStage;
    }

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private Player_Input _playerInput;

    private bool _isWalking;
    private Vector3 _lastInteractDirection;
    private Map_Stage _selectedStage;

    private void Awake() 
    {
        if (Instance != null) 
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start() //set up event listeners for when player inputs 'Interact' and 'InteractAlternate'
    {
        _playerInput.OnInteractAction += PlayerOnInteractAction;
    }

    private void PlayerOnInteractAction(object _sender, System.EventArgs e) //if the player detects a counter in front of it, call out that counter's Interact() and call out that the player has grabbed an object
    {
        if (_selectedStage != null) 
        {
            _selectedStage.Interact(this);
            OnPlayerSelectedStage?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Update() 
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() 
    {
        return _isWalking;
    }

    private void HandleInteractions() 
    {
        Vector2 _inputVector = _playerInput.GetMovementVectorNormalized();
        Vector3 _moveDirection = new Vector3(_inputVector.x, 0f, _inputVector.y);

        if (_moveDirection != Vector3.zero) 
        {
            _lastInteractDirection = _moveDirection;
        }

        float _interactDistance = 2f;
        if (Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, _interactDistance)) 
        {
            if (raycastHit.transform.TryGetComponent(out Map_Stage _stage)) 
            {
                if (_stage != _selectedStage) 
                {
                    SetSelectedStage(_stage);
                }
            } 
            else 
            {
                SetSelectedStage(null);
            }
        } 
        else 
        {
            SetSelectedStage(null);
        }
    }

    private void HandleMovement() 
    {
        Vector2 _inputVector = _playerInput.GetMovementVectorNormalized();
        Vector3 _moveDirection = new Vector3(_inputVector.x, 0f, _inputVector.y);

        float _moveDistance = _moveSpeed * Time.deltaTime;
        float _playerRadius = .7f;
        float _playerHeight = 2f;
        bool _canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight, _playerRadius, _moveDirection, _moveDistance);

        if (!_canMove) 
        {
            // Cannot move towards _moveDirection

            // Attempt only X movement
            Vector3 _moveDirectionX = new Vector3(_moveDirection.x, 0, 0).normalized;
            _canMove = _moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight, _playerRadius, _moveDirectionX, _moveDistance);

            if (_canMove) 
            {
                // Can move only on the X
                _moveDirection = _moveDirectionX;
            } 
            else 
            {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 _moveDirectionZ = new Vector3(0, 0, _moveDirection.z).normalized;
                _canMove = _moveDirection.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight, _playerRadius, _moveDirectionZ, _moveDistance);

                if (_canMove) 
                {
                    // Can move only on the Z
                    _moveDirection = _moveDirectionZ;
                } else 
                {
                    // Cannot move in any direction
                }
            }
        }

        if (_canMove) 
        {
            transform.position += _moveDirection * _moveDistance;
        }

        _isWalking = _moveDirection != Vector3.zero;

        float _rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, _moveDirection, Time.deltaTime * _rotateSpeed);
    }

    private void SetSelectedStage(Map_Stage _newStage) 
    {
        this._selectedStage = _newStage;

        OnSelectedStageChanged?.Invoke(this, new OnSelectedStageChangedEventArgs 
        {
            _selectedStage = _newStage
        });
    }
}
