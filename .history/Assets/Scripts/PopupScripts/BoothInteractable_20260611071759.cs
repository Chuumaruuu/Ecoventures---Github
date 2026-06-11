using UnityEngine;

public class BoothInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject boothPanelRoot;
    [SerializeField] private BoothQuizPanel boothQuizPanel;
    [SerializeField] private Questions_Data questionData;
    [SerializeField] private Item_Data unlockableItemData;

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

        CloseBooth();

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

        if (boothQuizPanel != null)
        {
            boothQuizPanel.Setup(questionData, unlockableItemData, this);
        }

        GameObject targetPanel = ResolveBoothPanel();
        if (targetPanel == null)
        {
            return;
        }

        UI_Manager.Instance.OpenBooth(targetPanel);
        isOpen = true;
    }

    public void OnFocusEnter(Interaction_Manager interactionManager)
    {
    }

    public void OnFocusExit(Interaction_Manager interactionManager)
    {
        CloseBooth();
    }

    public void RightAnswer()
    {
        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.ShowBoothResult(true);
        }

        if (Unlock_Manager.Instance != null)
        {
            Unlock_Manager.Instance.Unlock(unlockableItemData);
        }

        isOpen = false;

        if (boothQuizPanel != null)
        {
            boothQuizPanel.LockAnswerButtons();
        }

        if (boothPanelRoot != null)
        {
            boothPanelRoot.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    public void WrongAnswer()
    {
        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.ShowBoothResult(false);
        }

        if (Unlock_Manager.Instance != null)
        {
            Unlock_Manager.Instance.Lock(unlockableItemData);
        }

        isOpen = false;

        if (boothQuizPanel != null)
        {
            boothQuizPanel.LockAnswerButtons();
        }

        if (boothPanelRoot != null)
        {
            boothPanelRoot.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    public void ContinueToWarehouse()
    {
        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.ShowContinuePanel();
        }
    }

    public void CloseBooth()
    {
        if (!isOpen)
        {
            return;
        }

        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.CloseBooth();
        }

        isOpen = false;
    }

    private GameObject ResolveBoothPanel()
    {
        if (boothPanelRoot != null)
        {
            return boothPanelRoot;
        }

        if (boothQuizPanel != null)
        {
            return boothQuizPanel.gameObject;
        }

        return null;
    }
}