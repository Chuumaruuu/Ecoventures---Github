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
        // trigger WorkshopIntro dialogue only when this is the first time loading this scene (pwede ka gumawa ng bools sa Dialogue_Progress na script. mag ccarry over data non kahit after runtime)
        Scene_Manager.Instance.OnSceneFadeComplete += IntroDialogue;
    }

    private void IntroDialogue()
    {
        if (!_dialogueProgress._workshopIntroDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("WorkshopIntro");
            if (dialogueData != null)
            {
                _dialogueProgress._workshopIntroDone = true;
                CallDialogue(dialogueData);
            }
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