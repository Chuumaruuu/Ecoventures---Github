using UnityEngine.SceneManagement;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{
    [SerializeField] private string _stageName;
    [SerializeField] private Dialogue_Progress _dialogueProgress;
    [SerializeField] private Dialogue_Data[] _mapDialogues;

    public void Start()
    {
        // trigger MapIntro dialogue only when this is the first time the player enters the map scene
        if (!_dialogueProgress._mapIntroDone)
        {
            Dialogue_Data dialogueData = GetDialogueData("MapIntro");
            if (dialogueData != null)
            {
                Dialogue_Manager.Instance.StartDialogue(dialogueData);
                _dialogueProgress._mapIntroDone = true;
            }
        }
    }
    public virtual void Interact(Map_Player _player)
    {
        SceneManager.LoadScene(_stageName);
    }

    public virtual void InteractAlternate(Map_Player _player)
    {
        // nothing happens
    }
    
    public Dialogue_Data GetDialogueData(string title)
    {
        foreach (Dialogue_Data data in _mapDialogues)
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
