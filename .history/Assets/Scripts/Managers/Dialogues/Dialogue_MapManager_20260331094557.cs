using UnityEngine;

public class Dialogue_MapManager : MonoBehaviour
{
    [SerializeField] private Dialogue_Progress _dialogueProgress;
    [SerializeField] private Dialogue_Data[] _mapDialogues;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
