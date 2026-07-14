using System.Collections.Generic;
using UnityEngine;

public class HintInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject hintPanelRoot;
    [SerializeField] private HintDisplayPanel hintDisplayPanel;
    [SerializeField] private Hints_Data hintData;

    private bool isOpen;
    private Sprite selectedHintSprite;

    private static readonly Dictionary<Hints_Data, Queue<Sprite>> shuffledSpritePools = new Dictionary<Hints_Data, Queue<Sprite>>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetPools()
    {
        // Ensures pools don't carry over stale state between play sessions
        // when domain reload is disabled in the editor.
        shuffledSpritePools.Clear();
    }

    private void Awake()
    {
        selectedHintSprite = GetNextSprite(hintData);
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

        CloseHint();

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

        if (hintDisplayPanel != null)
        {
            hintDisplayPanel.Setup(selectedHintSprite);
        }

        GameObject targetPanel = ResolveHintPanel();
        if (targetPanel == null)
        {
            return;
        }

        UI_Manager.Instance.OpenHint(targetPanel);
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
            UI_Manager.Instance.EnterExploreMode();
        }

        isOpen = false;
    }

    private GameObject ResolveHintPanel()
    {
        if (hintPanelRoot != null)
        {
            return hintPanelRoot;
        }

        if (hintDisplayPanel != null)
        {
            return hintDisplayPanel.gameObject;
        }

        return null;
    }

    private static Sprite GetNextSprite(Hints_Data data)
    {
        if (data == null || data._hintSprites == null || data._hintSprites.Length == 0)
        {
            return null;
        }

        if (!shuffledSpritePools.TryGetValue(data, out Queue<Sprite> pool) || pool.Count == 0)
        {
            pool = BuildShuffledQueue(data._hintSprites);
            shuffledSpritePools[data] = pool;
        }

        return pool.Dequeue();
    }

    private static Queue<Sprite> BuildShuffledQueue(Sprite[] sprites)
    {
        List<Sprite> list = new List<Sprite>(sprites);

        // Fisher-Yates shuffle
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        return new Queue<Sprite>(list);
    }
}