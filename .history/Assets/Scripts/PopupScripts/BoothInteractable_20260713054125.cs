using UnityEngine;
using UnityEngine.UI;

public class BoothInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject boothPanelRoot;
    [SerializeField] private BoothQuizPanel boothQuizPanel;
    [SerializeField] private Questions_Data questionData;
    [SerializeField] private Item_Data unlockableItemData;
    [SerializeField] private Image unlockedItemImage;

    private bool isOpen;

    private void Awake()
    {
        if (unlockableItemData.isUnlocked)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    private void OnEnable()
    {
        if (Unlock_Manager.Instance != null)
        {
            Unlock_Manager.Instance.OnUnlockStateChanged += HandleUnlockStateChanged;
        }
    }

    private void OnDisable()
    {
        if (Unlock_Manager.Instance != null)
        {
            Unlock_Manager.Instance.OnUnlockStateChanged -= HandleUnlockStateChanged;
        }
    }

    // Fires once GameTracker's objectives are met and a previously-pending
    // correct answer finally unlocks. Lets the booth catch up even if the
    // player has already walked away and re-entered explore mode.
    private void HandleUnlockStateChanged(Item_Data item, bool unlocked)
    {
        if (item != unlockableItemData || !unlocked)
        {
            return;
        }

        FinalizeUnlockedVisuals();
    }

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
        InteractAlternate(interactionManager);
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

        isOpen = false;

        if (boothQuizPanel != null)
        {
            boothQuizPanel.LockAnswerButtons();
        }

        if (boothPanelRoot != null)
        {
            boothPanelRoot.SetActive(false);
        }

        if (Unlock_Manager.Instance != null)
        {
            // Answering correctly is necessary but no longer sufficient.
            // RequestUnlock only grants the item immediately if GameTracker's
            // objectives (e.g. the sales goal) are already met; otherwise it
            // queues the item and HandleUnlockStateChanged will finish the
            // job later, once the objective is completed.
            Unlock_Manager.Instance.RequestUnlock(unlockableItemData);

            if (Unlock_Manager.Instance.IsUnlocked(unlockableItemData))
            {
                FinalizeUnlockedVisuals();
                return;
            }
        }

        // Objective not met yet: correct answer is registered, item stays
        // locked, and the booth remains active so the player can come back
        // (or the unlock will resolve automatically via the event above).
        Debug.Log(unlockableItemData != null
            ? unlockableItemData.name + " answered correctly, but is pending until sales objective is met."
            : "Booth answered correctly, but is pending until sales objective is met.");
    }

    private void FinalizeUnlockedVisuals()
    {
        if (unlockedItemImage != null && UI_Manager.Instance != null)
        {
            unlockedItemImage.gameObject.SetActive(true);
            UI_Manager.Instance.RegisterCorrectAnswerImage(unlockedItemImage);
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

        if (unlockedItemImage != null)
        {
            unlockedItemImage.gameObject.SetActive(false);
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

        if (unlockedItemImage != null)
        {
            unlockedItemImage.gameObject.SetActive(false);
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