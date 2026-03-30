using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialogue_Manager : MonoBehaviour
{
    public Image _actorAvatar;
    public TMP_Text _actorName;
    public TMP_Text _messageText;
    public GameObject _dialogueBox;
    
    private Animator _dialogueBoxAnimator;

    private void Start()
    {
        _dialogueBoxAnimator = GetComponent<Animator>();
    }

    public void StartDialogue(string _dialogueID)
    {
        _dialogueBoxAnimator.SetBool("Active", true);
    }

    public void EndtDialogue(string _dialogueID)
    {
        _dialogueBoxAnimator.SetBool("Active", false);
    }
}


