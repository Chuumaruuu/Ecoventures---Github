using UnityEngine;

public class Dialogue_Manager : MonoBehaviour
{
    public static Dialogue_Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void StartDialogue(Dialogue_Data _dialogueData)
    {
        Dialogue_UI.Instance.OpenDialogue(_dialogueData);
    }
}

