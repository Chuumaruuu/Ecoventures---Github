using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_UI : MonoBehaviour
{
    public static Dialogue_UI Instance { get; private set; }

    public event Action OnDialogueEnd;
    

    private Animator _dialogueBoxAnimator;

    [SerializeField] private GameObject _dialogueBoxUI;
    [SerializeField] private GameObject[] _mainUI;

    [SerializeField] private Image _actorAvatar;
    [SerializeField] private TextMeshProUGUI _actorName;
    [SerializeField] private TextMeshProUGUI _messageText;

    private Dialogue[] _currentMessageArray;
    private Actor[] _currentActorArray;
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
    }

    public void SetDialogue(Dialogue_Data _dialogueData)
    {
        _currentMessageArray = _dialogueData._messages;
        _currentActorArray = _dialogueData._actors;
        _activeMessage = 0;
        
        _messageText.text = "";
        _actorName.text = "";
        _actorAvatar.color = new Color(0,0,0,0);
        foreach (GameObject i in _mainUI)
        {
            i.SetActive(false);
        }

        Debug.Log("Starting Conversation: " + _dialogueData);
        DisplayMessage();
        _dialogueBoxUI.SetActive(true);
    }

    public void DisplayMessage()
    {
        Game_Manager.Instance.PauseGame();//pause

        _actorAvatar.color = new Color(255,255,255,255); 

        Dialogue _messageToDisplay = _currentMessageArray[_activeMessage];
        Actor _actorToDisplay = _currentActorArray[_messageToDisplay._actorID];

        _messageText.text = _messageToDisplay._message;
        _actorName.text = _actorToDisplay._actorName;
        _actorAvatar.sprite = _actorToDisplay._avatar;
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
            Game_Manager.Instance.ResumeGame();
            _dialogueBoxAnimator.SetTrigger("Active");
            
            foreach (GameObject i in _mainUI)
            {
                i.SetActive(true);
            }
        }
    }

    public void EndDialogue()
    {
        _dialogueBoxUI.SetActive(false);
        OnDialogueEnd?.Invoke();
    }
}


