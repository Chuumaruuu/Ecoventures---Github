using UnityEngine;

[CreateAssetMenu(fileName = "Tutorial_Progress", menuName = "Scriptable Objects/Tutorial_Progress")]
public class Tutorial_Progress : ScriptableObject
{ 
    private bool _tutorialDone;

    // WORKSHOP
    private bool _workshopTutorialDone;
    public bool _moveTutorialDone;
    public bool _grabTutorialDone;
    public bool _moneyTutorialDone;
    public bool _guidebookTutorialDone;
    public bool _interactTutorialDone;
    
    //Map
    public bool _workshopMapDone;

    //Stage
    public bool _workshopStageDone;

    public void ConditionDone()
    {
        _workshopMapDone = CheckWorkshopStatus();
        _tutorialDone = CheckTutorialStatus();
    }

    private bool CheckTutorialStatus()
    {
        return _workshopTutorialDone && _workshopMapDone && _workshopStageDone;
    }

    private bool CheckWorkshopStatus()
    {
        return _moveTutorialDone && _grabTutorialDone && _moneyTutorialDone && _guidebookTutorialDone && _interactTutorialDone;
    }

    private bool CheckMapStatus()
    {
        return false; //to be implemented
    }

    private bool CheckStageStatus()
    {
        return false; //to be implemented
    }
}