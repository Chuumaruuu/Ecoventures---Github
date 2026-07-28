using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_UI : MonoBehaviour
{
    public static Dialogue_UI Instance { get; private set; }

    public event Action<string> OnDialogueEnd;
    

    private Animator _dialogueBoxAnimator;
    private Canvas _dialogueCanvas;
    private DialogueType _currentDialogueType;

    [SerializeField] private GameObject _dialogueBoxUI;
    [SerializeField] private Image _actorAvatar;
    [SerializeField] private TextMeshProUGUI _actorName;
    [SerializeField] private TextMeshProUGUI _messageText;

    private Dialogue[] _currentMessageArray;
    private Actor[] _currentActorArray;
    private string _currentDialogue;
    private int _activeMessage = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        _dialogueBoxAnimator = GetComponent<Animator>();
        _dialogueCanvas = GetComponent<Canvas>();
    }

    public void SetDialogue(Dialogue_Data _dialogueData, DialogueType _dialogueType)
    {
        _dialogueCanvas.sortingOrder = 5;
        _currentDialogueType = _dialogueType;
        _currentDialogue = _dialogueData.GetTitle();
        _currentMessageArray = _dialogueData._messages;
        _currentActorArray = _dialogueData._actors;
        ClearUI();
        
        WorkshopUI_Manager.Instance.HideMainUI();


        Debug.Log("Starting Conversation: " + _dialogueData.GetTitle());

        Game_Manager.Instance.PauseGame();
        _dialogueBoxUI.SetActive(true);
        StartDialogueViaDialogueType();
    }

    public void DisplayMessage()
    {
        _actorAvatar.color = new Color(255,255,255,255); 

        Dialogue _messageToDisplay = _currentMessageArray[_activeMessage];
        Actor _actorToDisplay = _currentActorArray[_messageToDisplay._actorID];

        _messageText.text = _messageToDisplay._message;
        _actorName.text = _actorToDisplay._actorName;
        _actorAvatar.sprite = _actorToDisplay._avatar;
    }

    public void StartDialogueViaDialogueType()
    {
        Debug.Log("Starting dialogue type: "+ _currentDialogueType);
        switch (_currentDialogueType)
        {
            case DialogueType.standard:
            _dialogueBoxAnimator.SetTrigger("Pop");
            Debug.Log("animator triggered: pop");
            break;

            case DialogueType.start:
            _dialogueBoxAnimator.SetTrigger("Pop");
            Debug.Log("animator triggered: pop");
            break;

            case DialogueType.middle:
            DisplayMessage();
            _dialogueBoxAnimator.SetTrigger("Idle");
            Debug.Log("animator triggered: idle");
            break;

            case DialogueType.end:
            DisplayMessage();
            _dialogueBoxAnimator.SetTrigger("Idle");
            Debug.Log("animator triggered: idle");
            break;
        }
    }

    public void ClearUI()
    {
        _activeMessage = 0;
        _messageText.text = "";
        _actorName.text = "";
        _actorAvatar.color = new Color(0,0,0,0);
    }

    public void NextLine()
    {
        _activeMessage++;
        if (_activeMessage < _currentMessageArray.Length)
        {
            DisplayMessage();
        }

        else
        {
            if (_currentDialogueType == DialogueType.end || _currentDialogueType == DialogueType.standard)
            {
                Game_Manager.Instance.ResumeGame();
                _dialogueBoxAnimator.SetTrigger("Drop");
                Debug.Log("animator triggered: drop");
            }
            else
            {
                OnDialogueEnd?.Invoke(_currentDialogue);
                WorkshopUI_Manager.Instance.ShowMainUI();
            }
        }
    }

    public void EndDialogue()
    {
        WorkshopUI_Manager.Instance.ShowMainUI();
        _dialogueBoxUI.SetActive(false);
        OnDialogueEnd?.Invoke(_currentDialogue);
        _dialogueCanvas.sortingOrder = 0;
    }
}


