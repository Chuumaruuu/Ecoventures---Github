using Unity.VisualScripting;
using UnityEngine;

public class SceneDialogues_Workshop : MonoBehaviour
{
    public static SceneDialogues_Workshop Instance { get; private set; }

    [SerializeField] private Dialogue_Data[] _workshopDialogues;
    [SerializeField] private Dialogue_Progress _dialogueProgress;
    [SerializeField] private Tutorial_Progress _tutorialProgress;


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
        Scene_Manager.Instance.OnSceneFadeComplete += IntroDialogue;
        Dialogue_UI.Instance.OnDialogueEnd += MoveTutorial;
        Player_Base.Instance.OnContainerCounterSelected += GrabTutorial;
        Player_Base.Instance.OnPlayerGrabbedObject += MoneyTutorial;
        Player_Base.Instance.OnObjectDrop += InteractTutorial;
        //submit
    }

    private void IntroDialogue() // trigger WorkshopIntro dialogue when this is the first time loading this scene
    {
        if (!_dialogueProgress._workshopIntroDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("WorkshopIntro");
            if (dialogueData != null)
            {
                _dialogueProgress._workshopIntroDone = true;
                CallDialogue(dialogueData);
                Scene_Manager.Instance.OnSceneFadeComplete -= IntroDialogue;
            }
        }
    }

    private void MoveTutorial()
    {
        if (_dialogueProgress._workshopIntroDone && !_tutorialProgress._moveTutorialDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("MoveTutorial");
            if (dialogueData != null)
            {
                _tutorialProgress._moveTutorialDone = true;
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= MoveTutorial;
            }
        }
    }

    private void GrabTutorial()
    {
        if (_dialogueProgress._workshopIntroDone && _tutorialProgress._moveTutorialDone && !_tutorialProgress._grabTutorialDone)
            {
                Dialogue_Data dialogueData = GetDialogueData("GrabTutorial");
                if (dialogueData != null)
                {
                    _tutorialProgress._grabTutorialDone = true;
                    CallDialogue(dialogueData);
                    Player_Base.Instance.OnContainerCounterSelected -= GrabTutorial;
                }
            }   
    }

    private void MoneyTutorial(object sender, System.EventArgs e)
    {
        if (!_tutorialProgress._moneyTutorialDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("MoneyTutorial");
            if (dialogueData != null)
            {
                _tutorialProgress._moneyTutorialDone = true;
                CallDialogue(dialogueData);
                Player_Base.Instance.OnPlayerGrabbedObject -= MoneyTutorial;
            }
        }
    }

    private void InteractTutorial(object sender, System.EventArgs e)
    {
        if (!_tutorialProgress._interactTutorialDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("InteractTutorial");
            if (dialogueData != null)
            {
                _tutorialProgress._interactTutorialDone = true;
                CallDialogue(dialogueData);
                Player_Base.Instance.OnObjectDrop -= InteractTutorial;
            }
        }
    }

    public void GuideBookTutorial()
    {
        if (!_tutorialProgress._guidebookTutorialDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookTutorial");
            if (dialogueData != null)
            {
                _tutorialProgress._guidebookTutorialDone = true;
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