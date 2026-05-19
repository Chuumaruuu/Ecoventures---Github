using System;
using UnityEngine;

public class Counter_Progress : Counter_Base
{
    [HideInInspector] public bool _isSmelting;
    public event EventHandler OnProgressCooking;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float _progressTimerNormalized;
    }
    private enum ProgressState
    {
        Idle, //when nothing is on top
        Cooking, //when item is being cooked
        Burning,  //when item is cooked but can be burnt
        Overcooked //burned state
    }
    
    [SerializeField] private ProgressRecipe_Data[] _progressRecipeArray;

    private ProgressState _currentState;
    private float _progressTimer;
    private float _burnTimer;
    private AudioSource _progressSFXLoop;
    private ProgressRecipe_Data _progressRecipeData;


    private void Start()
    {
        _currentState = ProgressState.Idle;
    }
    private void Update()
    {
        if (!HasItem()) return;

        

        switch (_currentState)
        {
            case ProgressState.Cooking:
                StartSmeltingAnimations();
                _progressTimer += Time.deltaTime;

                // Start cooking SFX only once
                if (_progressSFXLoop == null)
                    _progressSFXLoop = AudioManager.Instance.PlayLoopedSFX(_counterAudio._cookingSFX);

                if (_progressTimer > _progressRecipeData._timerMax)
                {
                    // Stop loop
                    AudioManager.Instance.StopLoopedSFX(_progressSFXLoop);
                    _progressSFXLoop = null;

                    // Process finished item
                    this.GiveItem().DestroySelf();
                    Item_Base.SpawnItem(_progressRecipeData._finishedItem, this);
                    _currentState = ProgressState.Burning;
                    _progressTimer = 0;
                }

                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                {
                    _progressTimerNormalized = _progressTimer / _progressRecipeData._timerMax
                });
                break;

            case ProgressState.Burning:
                StopSmeltingAnimations();
                _burnTimer += Time.deltaTime;

                if (_progressSFXLoop == null)
                    _progressSFXLoop = AudioManager.Instance.PlayLoopedSFX(_counterAudio._burningSFX);

                if (_burnTimer > _progressRecipeData._timerMax)
                {
                    AudioManager.Instance.StopLoopedSFX(_progressSFXLoop);
                    _progressSFXLoop = null;

                    this.GiveItem().DestroySelf();
                    Item_Base.SpawnItem(_progressRecipeData._overcookedItem, this);
                    _currentState = ProgressState.Overcooked;
                }
                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                {
                    _progressTimerNormalized = _burnTimer / _progressRecipeData._timerMax
                });
                break;

            case ProgressState.Overcooked:
                AudioManager.Instance.PlaySFX(_counterAudio._overcookedSFX);
                StopSmeltingAnimations();
                _burnTimer = 0f;
                _currentState = ProgressState.Idle;
                break;

            case ProgressState.Idle:
                StopSmeltingAnimations();
                break;
        }
    }
    public override void Interact(Player_Base _player)
    {
        if (!HasItem()) //counter has nothing
        {
            if (_player.HasItem()) //player has an item
            {
                if (HasRecipeWithInput(_player.GiveItem().GetItemData())) //player is carrying an item with a progress recipe
                {
                    // player drop item oneshot

                    _player.GiveItem().SetItemParent(this);
                    _progressRecipeData = GetProgressRecipeDataWithInput(this.GiveItem().GetItemData());
                    
                    _currentState = ProgressState.Cooking;
                    _progressTimer = 0f;
                }
            }
            else
            {
                //player has nothing, counter has nothing
            }
        }
        else //counter has an item
        {
            if (_player.HasItem())
            {
                //player has an item
            }
            else // player has nothing 
            {
                _currentState = ProgressState.Idle;

                AudioManager.Instance.StopLoopedSFX(AudioManager.Instance.GetSFXSource());
                
                this.GiveItem().SetItemParent(_player);
            }
        }
    }
    public bool isBurning()
    {
        return _currentState == ProgressState.Burning;
    }
    
    private bool HasRecipeWithInput(Item_Data _inputItemData)
    {
        ProgressRecipe_Data _progressRecipeData = GetProgressRecipeDataWithInput(_inputItemData);
        return _progressRecipeData != null;
    }

    private Item_Data GetOutputForInput(Item_Data _inputItemData)
    {
        ProgressRecipe_Data _progressRecipeData = GetProgressRecipeDataWithInput(_inputItemData);
        if(_progressRecipeData != null)
        {
            return _progressRecipeData._finishedItem;
        }
        else
        {
            return null;
        }
    }

    private ProgressRecipe_Data GetProgressRecipeDataWithInput(Item_Data _inputItemData)
    {
        foreach (ProgressRecipe_Data _progressRecipeData in _progressRecipeArray)
        {
            if (_progressRecipeData._unfinishedItem == _inputItemData)
            {
                return _progressRecipeData;
            }
        }
        return null;
    }

    private void StartSmeltingAnimations()
    {
        _isSmelting = true;
        OnProgressCooking?.Invoke(this, EventArgs.Empty);
    }

    private void StopSmeltingAnimations()
    {
        _isSmelting = false;
        OnProgressCooking?.Invoke(this, EventArgs.Empty);
    }
}
