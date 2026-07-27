// using UnityEngine;

// public class SceneDialogues_Level1 : MonoBehaviour
// {
//     [SerializeField] private Dialogue_Progress _dialogueProgress;
//     [SerializeField] private Dialogue_Data[] _level1Dialogues;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         if (!_dialogueProgress._level1IntroDone)
//         {
//             Dialogue_Data dialogueData = GetDialogueData("Level1Intro");
//             if (dialogueData != null)
//             {
//                 Dialogue_Manager.Instance.StartDialogue(dialogueData);
//                 _dialogueProgress._level1IntroDone = true;
//             }
//         }
//     }

//     public Dialogue_Data GetDialogueData(string title)
//     {
//         foreach (Dialogue_Data data in _level1Dialogues)
//         {
//             if (data._dialogueTitle == title)
//             {
//                 return data;
//             }
//         }

//         Debug.LogWarning($"Dialogue_Data with title '{title}' not found.");
//         return null;
//     }

//     public void CallDialogue(Dialogue_Data dialogue_Data)
//     {
//         Dialogue_Manager.Instance.StartDialogue(dialogue_Data);
//     }
// }
