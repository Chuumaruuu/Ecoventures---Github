using UnityEngine;
using TMPro;

public class NPCDialoguePopup : MonoBehaviour
{
    [SerializeField] GameObject dialoguePanelRoot;
    [SerializeField] TextMeshProUGUI dialogueText;
    [SerializeField] RandomNPCDialogue_Data randomNPCDialogueData;

    private void Update()
    {
        if(randomNPCDialogueData == null)
        {
            Debug.LogWarning("RandomNPCDialogue_Data not found. Please ensure it is placed in the Resources folder.");
        }else
        {
            Debug.Log("RandomNPCDialogue_Data found. Stage ID: " + randomNPCDialogueData._stageID);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Player not detected. Exiting dialogue popup.");
            return;
        }

        dialoguePanelRoot.SetActive(true);
        Debug.Log("Player detected. Displaying dialogue popup.");
        SetDialogueText();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Debug.Log("Player not detected. Exiting dialogue popup.");
            return;
        }

        dialoguePanelRoot.SetActive(false);
        Debug.Log("Player exited. Hiding dialogue popup.");
        ClearDialogueText();
    }

    private void SetDialogueText()
    {
        Debug.Log("Setting dialogue text.");
        if (randomNPCDialogueData != null)
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
