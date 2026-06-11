using UnityEngine;
using System;
using System.Collections;

public class Interaction_Manager : MonoBehaviour
{
    public static Interaction_Manager Instance { get; private set; }

    [SerializeField] private Player_Input playerInput;

    private IInteractable currentInteractable;

    private bool isInteracting;

    public bool HasCurrentInteractable => currentInteractable != null;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
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
        if (isInteracting)
            return;

        StartCoroutine(InteractNextFrame());
    }


    private void PlayerInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        if (isInteracting)
            return;

        StartCoroutine(InteractAlternateNextFrame());
    }


    private IEnumerator InteractNextFrame()
    {
        isInteracting = true;

        yield return null;

        if (currentInteractable != null)
        {
            currentInteractable.Interact(this);
        }

        isInteracting = false;
    }


    private IEnumerator InteractAlternateNextFrame()
    {
        isInteracting = true;

        yield return null;

        if (currentInteractable != null)
        {
            currentInteractable.InteractAlternate(this);
        }

        isInteracting = false;
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
        if (interactable != null &&
            !ReferenceEquals(currentInteractable, interactable))
        {
            return;
        }

        currentInteractable?.OnFocusExit(this);

        currentInteractable = null;
    }
}