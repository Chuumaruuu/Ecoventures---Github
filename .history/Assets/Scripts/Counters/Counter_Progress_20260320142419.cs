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
    private ProgressRecipe_Data _progressRecipeData;


    private void Start()
    {
        _currentState = ProgressState.Idle;
    }
    private void Update()
    {
        if (HasItem()) // counter has item
        {
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                _progressTimerNormalized = (float)_progressTimer / _progressRecipeData._timerMax
            });

            switch (_currentState)
            {
                case ProgressState.Idle:
                    StopSmeltingAnimations();
                    break;
                case ProgressState.Cooking:
                    StartSmeltingAnimations();
                    _progressTimer += Time.deltaTime;
                    // cooking audio loop
                    SoundManager.Instance.PlayLoop(SoundManager.Instance.cookingLoopClip);
                    if (_progressTimer > _progressRecipeData._timerMax) //item is done
                    {
                        // cooking audio loop stop
                        // burnt audio oneshot
                        // hindi ba to dapat good feedback oneshot? kasi may overcooked pa naman
                        SoundManager.Instance.StopLoop();
                        SoundManager.Instance.PlaySFX(SoundManager.Instance.goodFeedbackClip);
                        Debug.Log("Smelting");
                        this.GiveItem().DestroySelf();

                        Item_Base.SpawnItem(_progressRecipeData._finishedItem, this);
                        _currentState = ProgressState.Burning;
                        _progressTimer = 0;
                        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                        {
                            _progressTimerNormalized = (float)_progressTimer / _progressRecipeData._timerMax
                        });
                    }          
                    break;
                case ProgressState.Burning:

                    
                    StopSmeltingAnimations();
                    _burnTimer += Time.deltaTime;

                    if (_burnTimer > _progressRecipeData._timerMax) //item is burnt
                    {
                        // burnt audio oneshot
                        SoundManager.Instance.PlaySFX(SoundManager.Instance.burnClip);
                        this.GiveItem().DestroySelf();

                        Item_Base.SpawnItem(_progressRecipeData._overcookedItem, this);
                        _currentState = ProgressState.Overcooked;
                    }
                    break;
                case ProgressState.Overcooked:
                    StopSmeltingAnimations();
                    _burnTimer = 0f;
                    _currentState = ProgressState.Idle;
                    break;
            }
            Debug.Log("State");
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
                    SoundManager.Instance.PlaySFX(SoundManager.Instance.dropClip);
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

                // player pick up item audio oneshot
                SoundManager.Instance.PlaySFX(SoundManager.Instance.pickupClip);
                this.GiveItem().SetItemParent(_player);
            }
        }
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
