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
            Debug.Log("Player outside hint trigger.");
            return;
        }

        if (Interaction_Manager.Instance != null)
        {
            Debug.Log("Setting current interactable to hint object" + gameObject.name);
            Interaction_Manager.Instance.SetCurrentInteractable(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Player exited hint trigger.");
            return;
        }

        CloseHint();

        if (Interaction_Manager.Instance != null)
        {
            Debug.Log("Clearing current interactable from hint.");
            Interaction_Manager.Instance.ClearCurrentInteractable(this);
        }
    }

    public void Interact(Interaction_Manager interactionManager)
    {
        Debug.Log("Player interacted with hint.");
        InteractAlternate(interactionManager);
    }

    public void InteractAlternate(Interaction_Manager interactionManager)
    {
        if (isOpen || UI_Manager.Instance == null)
        {
            Debug.Log("Hint is already open or UI_Manager instance is missing.");
            return;
        }

        if (hintDisplayPanel != null)
        {
            Debug.Log("Setting up hint display panel with hint data.");
            hintDisplayPanel.Setup(hintData);
        }

        GameObject targetPanel = ResolveHintPanel();
        if (targetPanel == null)
        {
            Debug.Log("No valid hint panel found to open.");
            return;
        }

        Debug.Log("Opening hint panel.");
        UI_Manager.Instance.OpenHint(targetPanel);
        isOpen = true;
    }

    public void OnFocusEnter(Interaction_Manager interactionManager)
    {
        Debug.Log("Player focused on hint.");
    }

    public void OnFocusExit(Interaction_Manager interactionManager)
    {
        Debug.Log("Player lost focus from hint.");
        CloseHint();
    }

    public void CloseHint()
    {
        if (!isOpen)
        {
            Debug.Log("Hint is not open, no need to close.");
            return;
        }

        if (UI_Manager.Instance != null)
        {
            Debug.Log("Closing hint panel.");
            UI_Manager.Instance.CloseHint();
        }

        Debug.Log("Hint closed.");
        isOpen = false;
    }

    private GameObject ResolveHintPanel()
    {
        if (hintPanelRoot != null)
        {
            Debug.Log("Using hint panel root as target panel.");
            return hintPanelRoot;
        }

        if (hintDisplayPanel != null)
        {
            Debug.Log("Using hint display panel as target panel.");
            return hintDisplayPanel.gameObject;
        }

        Debug.LogWarning("No hint panel root or display panel assigned for hint interactable.");
        return null;
    }
}