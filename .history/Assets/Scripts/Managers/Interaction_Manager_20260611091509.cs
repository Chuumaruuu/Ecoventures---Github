using UnityEngine;
using System;

public class Interaction_Manager : MonoBehaviour
{
    public static Interaction_Manager Instance { get; private set; }

    [SerializeField] private Player_Input playerInput;

    private IInteractable currentInteractable;

    public bool HasCurrentInteractable => currentInteractable != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (playerInput == null)
        {
            playerInput = FindFirstObjectByType<Player_Input>();
        }

        if (playerInput != null)
        {
            playerInput.OnInteractAction += PlayerInput_OnInteractAction;
            playerInput.OnInteractAlternateAction += PlayerInput_OnInteractAlternateAction;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.OnInteractAction -= PlayerInput_OnInteractAction;
            playerInput.OnInteractAlternateAction -= PlayerInput_OnInteractAlternateAction;
        }

        ClearCurrentInteractable(null);
    }

    private void PlayerInput_OnInteractAction(object sender, EventArgs e)
    {
        currentInteractable?.Interact(this);
    }

    private void PlayerInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        currentInteractable?.InteractAlternate(this);
    }

    public void SetCurrentInteractable(IInteractable interactable)
    {
        if (ReferenceEquals(currentInteractable, interactable))
        {
            return;
        }

        currentInteractable?.OnFocusExit(this);
        currentInteractable = interactable;
        currentInteractable?.OnFocusEnter(this);
    }

    public void ClearCurrentInteractable(IInteractable interactable)
    {
        if (interactable != null && !ReferenceEquals(currentInteractable, interactable))
        {
            return;
        }

        if (currentInteractable != null)
        {
            currentInteractable.OnFocusExit(this);
        }

        currentInteractable = null;
    }
}
