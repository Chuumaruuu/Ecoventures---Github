using UnityEngine;

public class SellingModeIntera : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject sellingModeUI;
    private UI_Manager uiManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (Interaction_Manager.Instance != null)
        {
            Interaction_Manager.Instance.SetCurrentInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (Interaction_Manager.Instance != null)
        {
            Interaction_Manager.Instance.ClearCurrentInteractable(this);
        }
    }

    public void Interact(Interaction_Manager interactionManager)
    {
        InteractAlternate(interactionManager);
    }

    public void InteractAlternate(Interaction_Manager interactionManager)
    {
        if (uiManager == null)
        {
            uiManager = FindFirstObjectByType<UI_Manager>();
        }

        if (uiManager != null && sellingModeUI != null)
        {
            uiManager.EnterSellingMode();
        }
    }

    public void OnFocusEnter(Interaction_Manager interactionManager)
    {
        // Optional: Add visual feedback for focus enter
    }

    public void OnFocusExit(Interaction_Manager interactionManager)
    {
        // Optional: Add visual feedback for focus exit
    }
}
