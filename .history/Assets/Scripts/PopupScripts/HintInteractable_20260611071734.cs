using UnityEngine;

public class HintInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject hintPanelRoot;
    [SerializeField] private HintDisplayPanel hintDisplayPanel;
    [SerializeField] private Hints_Data hintData;

    private bool isOpen;

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

        CloseHint();

        if (Interaction_Manager.Instance != null)
        {
            Interaction_Manager.Instance.ClearCurrentInteractable(this);
        }
    }

    public void Interact(Interaction_Manager interactionManager)
    {
    }

    public void InteractAlternate(Interaction_Manager interactionManager)
    {
        if (isOpen || UI_Manager.Instance == null)
        {
            return;
        }

        if (hintDisplayPanel != null)
        {
            hintDisplayPanel.Setup(hintData);
        }

        UI_Manager.Instance.OpenHint(hintPanelRoot != null ? hintPanelRoot : hintDisplayPanel.gameObject);
        isOpen = true;
    }

    public void OnFocusEnter(Interaction_Manager interactionManager)
    {
    }

    public void OnFocusExit(Interaction_Manager interactionManager)
    {
        CloseHint();
    }

    public void CloseHint()
    {
        if (!isOpen)
        {
            return;
        }

        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.CloseHint();
        }

        isOpen = false;
    }
}