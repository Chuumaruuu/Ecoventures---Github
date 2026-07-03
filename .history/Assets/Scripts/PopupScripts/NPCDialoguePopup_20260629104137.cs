using UnityEngine;
using TMPro;

public class NPCDialoguePopup : MonoBehaviour
{
    [SerializeField] GameObject dialoguePanelRoot;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] int stageID;
    private RandomNPCDialogue_Data randomNPCDialogueData;

    private void Start()
    {
        randomNPCDialogueData = Resources.Load<RandomNPCDialogue_Data>("RandomNPCDialogue_Data");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        dialoguePanelRoot.SetActive(true);
        SetDialogueText();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        dialoguePanelRoot.SetActive(false);
        ClearDialogueText();
    }

    private void SetDialogueText()
    {
        if (randomNPCDialogueData != null && randomNPCDialogueData._stageID == stageID)
        {
            string[] dialogues = randomNPCDialogueData._randomDialogues;
            if (dialogues.Length > 0)
            {
                int randomIndex = Random.Range(0, dialogues.Length);
                dialogueText.text = dialogues[randomIndex];
            }
        }
    }

    private void ClearDialogueText()
    {
        dialogueText.text = string.Empty;
    }
}
