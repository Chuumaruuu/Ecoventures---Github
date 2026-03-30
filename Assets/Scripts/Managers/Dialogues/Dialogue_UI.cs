using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue_UI : MonoBehaviour
{
    public static Dialogue_UI Instance { get; private set; }

    public Image _actorAvatar;
    public TextMeshProUGUI _actorName;
    public TextMeshProUGUI _messageText;
    public Animator _dialogueBoxAnimator;
    public GameObject[] _otherUI;

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

    public void OpenDialogue(Dialogue[] messages, Actor[] actors)
    {
        _currentMessageArray = messages;
        _currentActorArray = actors;
        _activeMessage = 0;

        Debug.Log("Starting Conversation: " + messages.Length);
        _dialogueBoxAnimator.SetBool("Active", true);

        foreach (GameObject i in _otherUI)
        {
            i.SetActive(false);
        }
        Time.timeScale = 0; //pause
    }

    public void DisplayMessage()
    {
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
            _dialogueBoxAnimator.SetBool("Active", false);
            Time.timeScale = 1;
            foreach (GameObject i in _otherUI)
            {
                i.SetActive(true);
            }
        }
    }
}


