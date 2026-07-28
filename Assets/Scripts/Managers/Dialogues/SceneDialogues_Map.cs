// using UnityEngine;

// public class SceneDialogues_Map : MonoBehaviour
// {
//     [SerializeField] private Dialogue_Progress _dialogueProgress;
//     [SerializeField] private Dialogue_Data[] _mapDialogues;
    
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         Scene_Manager.Instance.OnSceneFadeComplete += TriggerMapIntro;
//     }

//     private void TriggerMapIntro()
//     {
//         // trigger MapIntro dialogue only when this is the first time the player enters the map scene
//         if (!_dialogueProgress._mapIntroDone)
//         {
//             Dialogue_Data _dialogueData = GetDialogueData("MapIntro");
//             if (_dialogueData != null)
//             {
//                 Dialogue_Manager.Instance.StartDialogue(_dialogueData);
//                 _dialogueProgress._mapIntroDone = true;
//             }
//         }
//     }

//     public Dialogue_Data GetDialogueData(string title)
//     {
//         foreach (Dialogue_Data data in _mapDialogues)
//         {
//             if (data._dialogueTitle == title)
//             {
//                 return data;
//             }
//         }

//         Debug.LogWarning($"Dialogue_Data with title '{title}' not found.");
//         return null;
//     }
// }
