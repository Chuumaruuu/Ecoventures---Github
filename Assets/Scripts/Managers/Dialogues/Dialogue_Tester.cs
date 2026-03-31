using UnityEngine;

public class Dialogue_Tester : MonoBehaviour
{
    public Dialogue_Data _dialogueData;
    public void TryDialogue()
    {
        Dialogue_Manager.Instance.StartDialogue(_dialogueData);
    }
}
