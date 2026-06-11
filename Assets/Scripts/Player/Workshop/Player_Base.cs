using System;
using UnityEngine;

public class Player_Base : MonoBehaviour, IItemParent
{
    //sex
    public static Player_Base Instance { get; private set; }
    
    public event EventHandler OnPlayerGrabbedObject;
    public event EventHandler OnObjectPickup;
    public event EventHandler OnObjectDrop;
    public event Action OnContainerCounterSelected;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs 
    {
        public Counter_Base _selectedCounter;
    }

    [SerializeField] private float _moveSpeed = 7f;
    [SerializeField] private Player_Input _playerInput;
    [SerializeField] private Player_Audio _playerAudio;
    [SerializeField] private LayerMask _countersLayerMask;
    [SerializeField] private Transform _itemHolder;

    private bool _isWalking = false;
    private Vector3 _lastInteractDirection;
    private Counter_Base _selectedCounter;
    private Item_Base _productionItem;

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
        _playerInput.OnInteractAlternateAction += PlayerOnInteractAlternateAction;
    }

    private void PlayerOnInteractAction(object _sender, EventArgs e) //if the player detects a counter in front of it, call out that counter's Interact() and call out that the player has grabbed an object
    {
        if (DetectsACounter()) 
        {
            _selectedCounter.Interact(this);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);

            if (HasItem())
            {
                OnObjectPickup?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                OnObjectDrop?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void PlayerOnInteractAlternateAction(object _sender, EventArgs e) //
    {
        if (Interaction_Manager.Instance != null && Interaction_Manager.Instance.HasCurrentInteractable)
        {
            return;
        }

        if (DetectsACounter()) 
        {
            _selectedCounter.InteractAlternate(this);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
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
        if (Physics.Raycast(transform.position, _lastInteractDirection, out RaycastHit raycastHit, _interactDistance, _countersLayerMask)) 
        {
            if (raycastHit.transform.TryGetComponent(out Counter_Base _baseCounter)) 
            {
                if (_baseCounter != _selectedCounter) 
                {
                    SetSelectedCounter(_baseCounter);

                    if (_baseCounter.GetComponent<Counter_Container>())
                    {
                        OnContainerCounterSelected?.Invoke();
                    }
                }
            } 
            else 
            {
                SetSelectedCounter(null);
            }
        } 
        else 
        {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement() 
    {
        Vector2 _inputVector = _playerInput.GetMovementVectorNormalized();
        Vector3 _moveDirection = new Vector3(_inputVector.x, 0f, _inputVector.y);

        float _moveDistance = _moveSpeed * Time.deltaTime;
        float _playerRadius = .7f;
        float _playerHeight = 2f;
        bool _canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight, _playerRadius, _moveDirection, _moveDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

        if (!_canMove) 
        {
            // Cannot move towards _moveDirection

            // Attempt only X movement
            Vector3 _moveDirectionX = new Vector3(_moveDirection.x, 0, 0).normalized;
            _canMove = _moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight, _playerRadius, _moveDirectionX, _moveDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

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
                _canMove = _moveDirection.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * _playerHeight, _playerRadius, _moveDirectionZ, _moveDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);

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

    private void SetSelectedCounter(Counter_Base _newSelectedCounter) 
    {
        this._selectedCounter = _newSelectedCounter;
        
        Debug.Log("Selected Counter Changed to " + _newSelectedCounter);
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs 
        {
            _selectedCounter = _newSelectedCounter
        });
        
    }

    public Transform GetItemFollowTransform() // 
    {
        return _itemHolder;
    }

    public void SetItem(Item_Base _newProductionItem) 
    {
        this._productionItem = _newProductionItem;
    }

    public Item_Base GiveItem() // prepares the player's currently held item to be transferred to a different IItemParent
    {
        return _productionItem;
    }

    public Counter_Base SelectedCounter()
    {
        return _selectedCounter;
    }

    public void ClearItem() // removes the item from player's posession
    {
        _productionItem = null;
    }

    public bool HasItem() // checks if player has an item on their hand
    {
        return _productionItem != null;
    }

    public bool DetectsACounter()
    {
        return _selectedCounter != null;
    }
}
