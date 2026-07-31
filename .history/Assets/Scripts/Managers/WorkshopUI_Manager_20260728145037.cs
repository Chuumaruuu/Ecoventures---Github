using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopUI_Manager : MonoBehaviour
{
    public static WorkshopUI_Manager Instance {get; private set;}
    //MAIN UI
    [SerializeField] private GameObject[] _mainUI;

    //TUTORIAL Pointers
    public List<Pointer> _pointerList;
    private GameObject _activePointer;
    public event Action OnGuidebookOpened, OnGuidebookClosed, OnGuidebookItemClicked;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; 
    }
    void Start()
    {
        Dialogue_Manager.Instance.OnStartDialogue += ShowPointer;
        Dialogue_UI.Instance.OnDialogueEnd += HidePointer;
    }
    
    public void TriggerGuidebookOpen()
    {
        OnGuidebookOpened?.Invoke();
    }

    public void TriggerGuidebookIconClick()
    {
        OnGuidebookItemClicked?.Invoke();
    }

    public void TriggerGuidebookClose()
    {
        OnGuidebookClosed?.Invoke();
    }

    public void HideMainUI()
    {
        foreach(GameObject i in _mainUI)
        {
            Debug.Log("Hiding UI");
            i.SetActive(false);
        }
    }

    public void ShowMainUI()
    {
        foreach(GameObject i in _mainUI)
        {
            Debug.Log("Showing UI");
            i.SetActive(true);
        }
    }

    public void ShowPointer(string _pointerName)
    {
        foreach (Pointer pointer in _pointerList)
        {
            if (_pointerName == pointer.GetPointerName())
            {
                _activePointer = pointer.GetPointerUI();
                pointer.ShowPointerGameObjects();
                pointer.GetPointerUI().SetActive(true);
            }
        }
    }

    public void HidePointer(string _pointerName)
    {
        if (_pointerList.Count == 0)
        {
            Dialogue_UI.Instance.OnDialogueEnd -= HidePointer;
        }
        else
        {
            foreach (Pointer pointer in _pointerList)
            {
                if (_pointerName == pointer.GetPointerName())
                {
                    Destroy(_activePointer);
                }
            }
        }
        
    }

    [Serializable]
    public class Pointer
    {
        [SerializeField] private string _pointerName; //must be the same name as the dialogue it corresponds to
        [SerializeField] private GameObject _pointerUI;
        [SerializeField] private GameObject[] _activeGameObjects;

        public string GetPointerName()
        {
            return _pointerName;
        }

        public GameObject GetPointerUI()
        {
            return _pointerUI;
        }

        public void ShowPointerGameObjects()
        {
            foreach(GameObject i in _activeGameObjects)
            {
                if (i != null)
                {
                    i.SetActive(true);
                }
                else
                {
                    return;
                }
            }
        }
    }

}
