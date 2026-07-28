using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_Progress", menuName = "Scriptable Objects/Dialogue_Progress")]
public class Dialogue_Progress : ScriptableObject
{ 
    [Header("WORKSHOP DIALOGUES")]
    // WORKSHOP DIALOGUES

    public bool WORKSHOP_INTRO; 
    public bool WORKSHOP_GUIDEBOOK_INTRO; 
    public bool WORKSHOP_GUIDEBOOK_INTRO2;
    public bool WORKSHOP_GUIDEBOOK_INTRO3;
    public bool WORKSHOP_GUIDEBOOK_INTRO4; 
    public bool WORKSHOP_GUIDEBOOK_INTRO5;
    public bool WORKSHOP_GUIDEBOOK_INTRO6;
    public bool WORKSHOP_GUIDEBOOK_INTRO7;
    public bool WORKSHOP_MOVEMENT_TUTORIAL;
    public bool WORKSHOP_GRAB_TUTORIAL;

    

    [Header("MAP DIALOGUES")]
    // MAP DIALOGUES
    public bool MAP_INTRO; //introduce map
    public bool MAP_TOSTAGE1; //prompts player to go to stage 1 (pointer towards stage 1)
    public bool MAP_DEMANDINTRO; //explain the items currently in demand for that stage (pointer towards items in demand UI)
    public bool MAP_OUTRO; //prompt player to interact with stage 1
    public bool MAP_BACKTOWORKSHOP; //direct player back to workshop (pointer towards workshop)



    [Header("STAGE 1 DIALOGUES")]
    // STAGE 1 DIALOGUES
    public bool STAGE1_INTRO; //introduce stage 1



    public void Reset()
    {
        WORKSHOP_INTRO=  
        WORKSHOP_GUIDEBOOK_INTRO=
        WORKSHOP_GUIDEBOOK_INTRO2=
        WORKSHOP_GUIDEBOOK_INTRO3=
        WORKSHOP_GUIDEBOOK_INTRO4=
        WORKSHOP_GUIDEBOOK_INTRO5=
        WORKSHOP_GUIDEBOOK_INTRO6=
        WORKSHOP_GUIDEBOOK_INTRO7=
        WORKSHOP_MOVEMENT_TUTORIAL=
        WORKSHOP_GRAB_TUTORIAL=

        false;
    }
}