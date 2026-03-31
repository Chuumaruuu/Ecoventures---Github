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
        if (!_dialogueProgress._workshopIntroDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("WorkshopIntro");
            if (dialogueData != null)
            {
                CallDialogue(dialogueData);
                _dialogueProgress._workshopIntroDone = true;
            }
        }

        // trigger BasicTutorial dialogue only when WorkshopIntro is finished (turo mo lang kay player ano ginagawa ng each button tas turo mo nalang saan yung guidebook. yaan mo na sya mag figure out ng recipes)
        // if (_dialogueProgress._workshopIntroDone && !_dialogueProgress._basicTutorialDone)
        // {
        //     Dialogue_Data dialogueData = GetDialogueData("BasicTutorial");
        //     if (dialogueData != null)
        //     {
        //         CallDialogue(dialogueData);
        //         _dialogueProgress._basicTutorialDone = true;
        //     }
        // }
    }

    public void WorkshopTimerDone()
    {
        // trigger WorkshopOutro only when this is the first time the workshop timer has finished
        if (!_dialogueProgress._workshopOutroDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("WorkshopOutro");
            if (dialogueData != null)
            {
                CallDialogue(dialogueData);
                _dialogueProgress._workshopOutroDone = true;
            }
        }
    }

    public void Level1Intro()
    {
        // trigger Level1Intro only when this is the first time the player starts level 1 (pwede mo rin naman i trigger sya sa WorkshopTimerDone, depende sa flow na gusto mo)
        if (!_dialogueProgress._level1IntroDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("Level1Intro");
            if (dialogueData != null)
            {
                CallDialogue(dialogueData);
                _dialogueProgress._level1IntroDone = true;
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