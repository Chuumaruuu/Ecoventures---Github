using UnityEngine;
using System;

public class SceneDialogues_Workshop : MonoBehaviour
{
    public static SceneDialogues_Workshop Instance { get; private set; }

    [SerializeField] private Dialogue_Progress _dialogueProgress;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Scene_Manager.Instance.OnSceneFadeComplete += WorkshopIntro;

        if (!Game_Manager.Instance.DebugModeOn())
        {
            ResetDialogues();
        }
    }

    private void WorkshopIntro() //Workshop Intro
    {
        if (_dialogueProgress.WORKSHOP_INTRO)
        {
            Scene_Manager.Instance.OnSceneFadeComplete -= WorkshopIntro;
            return;
        }
        else
        {
            Debug.Log("Dialogue Code: workshop intro reached");
            Dialogue_Data dialogueData = GetDialogueData("WorkshopIntro");
            if (dialogueData != null)
            {
                _dialogueProgress.WORKSHOP_INTRO = true;

                CallDialogue(dialogueData);
                Scene_Manager.Instance.OnSceneFadeComplete -= WorkshopIntro;
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro;
            }
        }
    }
    
    private void GuidebookIntro(string id) 
    {
        Debug.Log("Dialogue Code: point to guidebook reached");
        if (id == "WorkshopIntro")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro");
            if(dialogueData != null)
            {
                _dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO = true;

                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro;
            }
        }
    }

    public void GuidebookIntro2()
    {
        if (_dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO2)
        {
            return;
        }
        else
        {
            Debug.Log("Dialogue Code: guidebook button clicked reached");
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro2");
            if(dialogueData != null)
            {
                _dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO2 = true;

                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro3;

            }
        }
        
    }

    private void GuidebookIntro3(string id)
    {
        Debug.Log("Dialogue Code: point to stage 1 tab reached");
        if (id == "GuidebookIntro2")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro3");
            if(dialogueData != null)
            {
                _dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO3 = true;

                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro3;
            }
        }
    }

    public void GuidebookIntro4()
    {
        Debug.Log("Dialogue Code: stage 1 tab clicked reached");
        
        Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro4");
        if(dialogueData != null)
        {
            _dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO4 = true;

            CallDialogue(dialogueData);
            Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro5;
        }
    }

    private void GuidebookIntro5(string id)
    {
        Debug.Log("Dialogue Code: point to recipe reached");
        if (id == "GuidebookIntro4")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro5");
            if(dialogueData != null)
            {
                _dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO5 = true;

                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro5;
            }
        }
    }

    public void GuidebookIntro6()
    {
        if (_dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO6)
        {
            return;
        }
        else
        {
            Debug.Log("Dialogue Code: bracelet recipe clicked reached");
        
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro6");
            if(dialogueData != null)
            {
                _dialogueProgress.WORKSHOP_GUIDEBOOK_INTRO6 = true;

                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro7;
            }
        } 
    }

    private void GuidebookIntro7(string id)
    {
        Debug.Log("Dialogue Code: bracelet recipe intro reached");
        if (id == "GuidebookIntro6")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro7");
            if(dialogueData != null)
            {
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro7;
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro8;
            }
        }
    }

    private void GuidebookIntro8(string id)
    {
        Debug.Log("Dialogue Code: point to step 1 reached");
        if (id == "GuidebookIntro7")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro8");
            if(dialogueData != null)
            {
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro8;
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro9;
            }
        }
    }

    private void GuidebookIntro9(string id)
    {
        Debug.Log("Dialogue Code: point to step 2 reached");
        if (id == "GuidebookIntro8")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro9");
            if(dialogueData != null)
            {
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro9;
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro10;
            }
        }
    }

    private void GuidebookIntro10(string id)
    {
        Debug.Log("Dialogue Code: point to step 3 reached");
        if (id == "GuidebookIntro9")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro10");
            if(dialogueData != null)
            {
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro10;
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro11;
            }
        }
    }

    private void GuidebookIntro11(string id)
    {
        Debug.Log("Dialogue Code: point to step 4 reached");
        if (id == "GuidebookIntro10")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro11");
            if(dialogueData != null)
            {
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro11;
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro12;
            }
        }
    }

    private void GuidebookIntro12(string id)
    {
        Debug.Log("Dialogue Code: point to step 5 reached");
        if (id == "GuidebookIntro11")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro12");
            if(dialogueData != null)
            {
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro12;
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookIntro13;
            }
        }
    }

    private void GuidebookIntro13(string id)
    {
        Debug.Log("Dialogue Code: point to step 6 reached");
        if (id == "GuidebookIntro12")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookIntro13");
            if(dialogueData != null)
            {
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro13;
                Dialogue_UI.Instance.OnDialogueEnd += GuidebookClose;
            }
        }
    }

    private void GuidebookClose(string id)
    {
        Debug.Log("Dialogue Code: point to close button reached");
        if (id == "GuidebookIntro13")
        {
            Dialogue_Data dialogueData = GetDialogueData("GuidebookClose");
            if(dialogueData != null)
            {
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GuidebookIntro13;
            }
        }
    }

    public void MovementIntro()
    {
        if (_dialogueProgress.WORKSHOP_MOVEMENT_TUTORIAL)
        {
            return;
        }
        else
        {
            Dialogue_Data dialogueData = GetDialogueData("MovementIntro");
            if (dialogueData != null)
            {
                _dialogueProgress.WORKSHOP_MOVEMENT_TUTORIAL = true;
                
                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd += MovementIntro2;
            }
        }
    }

    private void MovementIntro2(string id)
    {
        Debug.Log("Dialogue Code: show joystick");
        if (id == "MovementIntro")
        {
            Dialogue_Data dialogueData = GetDialogueData("MovementIntro2");
            if(dialogueData != null)
            {
                CallDialogue(dialogueData);

                Player_Base.Instance.OnContainerCounterSelected += GrabIntro;
                Dialogue_UI.Instance.OnDialogueEnd -= MovementIntro2;
            }
        }
    }

    private void GrabIntro(Counter_Container counter)
    {
        if (counter.GetStorageItem().GetItemName() == "ScrapWood")
        {
            Dialogue_Data dialogueData = GetDialogueData("GrabIntro");
            if(dialogueData != null)
            {
                // _dialogueProgress.WORKSHOP_GUIDEBOOK = true;

                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd += GrabIntro2;
                Player_Base.Instance.OnContainerCounterSelected -= GrabIntro;
            }
        }
    }

    private void GrabIntro2(string id)
    {
        Debug.Log("Dialogue Code: show grab button");
        if (id == "GrabIntro")
        {
            Dialogue_Data dialogueData = GetDialogueData("GrabIntro2");
            if(dialogueData != null)
            {

                CallDialogue(dialogueData);
                Dialogue_UI.Instance.OnDialogueEnd -= GrabIntro2;
                Debug.Log("Registering MoneyIntro");
                // Player_Base.Instance.OnObjectPickup += MoneyIntro;
            }
        }
    }

    // private void MoneyIntro()
    // {
    //     Debug.Log("MoneyIntro called");
    //     if(Player_Base.Instance.GiveItem().GetItemData()._objectName != "ScrapWood")
    //     {
    //         Debug.Log("That's not wood");
    //         return;
    //     }
    //     else
    //     {
    //         Debug.Log("Money Intro Starting");
    //         if (_dialogueProgress.WORKSHOP_GRAB_TUTORIAL)
    //         {
    //             return;
    //         }
    //         else
    //         {
    //             Dialogue_Data dialogueData = GetDialogueData("MoneyIntro");
    //             if (dialogueData != null)
    //             {
    //                 _dialogueProgress.WORKSHOP_GRAB_TUTORIAL = true;
                    
    //                 CallDialogue(dialogueData);
    //                 Player_Base.Instance.OnObjectPickup -= MoneyIntro;
    //             }
    //         }
    //     }
    // }

    public Dialogue_Data GetDialogueData(string title)
    {
        foreach (Dialogue_Data data in Dialogue_Manager.Instance._dialogueList)
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
        Dialogue_Manager.Instance.StartDialogue(dialogue_Data.GetTitle());
    }

    private void ResetDialogues()
    {
        _dialogueProgress.Reset();
    }
}