using UnityEngine;

public class WorkshopDialogue_Manager : MonoBehaviour
{
    public static WorkshopDialogue_Manager Instance { get; private set; }

    [SerializeField] private Dialogue_Data[] _workshopDialogues;
    [SerializeField] private Dialogue_Progress _dialogueProgress;
    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (!_dialogueProgress._workshopIntroDone)
        {
            CallDialogue(GetDialogueData("WorkshopIntro"));
            _dialogueProgress._workshopIntroDone = true;
        }
    }

    public Dialogue_Data GetDialogueData(string title)
    {
        foreach (Dialogue_Data data in _workshopDialogues)
        {
            if (data._dialogueTitle == title)
            {
                return data;
            }
        }

        Debug.LogWarning($"Dialogue_Data with title '{title}' not found.");
        return null;
    }

    public void CallDialogue(Dialogue_Data dialogue_Data)
    {
        Dialogue_Manager.Instance.StartDialogue(dialogue_Data);
    }

}