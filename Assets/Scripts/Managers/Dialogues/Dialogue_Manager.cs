using System;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Manager : MonoBehaviour
{
    public static Dialogue_Manager Instance { get; private set; }
    public event Action<string> OnStartDialogue;

    public List<Dialogue_Data> _dialogueList;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public void StartDialogue(string _title)
    {
        foreach (Dialogue_Data data in _dialogueList)
        {
            string title = data.GetTitle();

            if (title == _title)
            {
                Dialogue_UI.Instance.SetDialogue(data, data.GetDialogueType());
                Game_Manager.Instance.PauseGame();

                OnStartDialogue?.Invoke(_title);
            }
        }

        
    }
}

