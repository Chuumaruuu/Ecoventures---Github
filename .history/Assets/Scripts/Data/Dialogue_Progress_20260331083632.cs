using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue_Progress", menuName = "Scriptable Objects/Dialogue_Progress")]
public class Dialogue_Progress : ScriptableObject
{
    // workshop
    public bool _workshopIntroDone;
    public bool _basicTutorialDone;
    public bool _workshopOutroDone;
    public bool _level1IntroDone;
    
    public void Start()
    {
        _workshopIntroDone = false;
        _basicTutorialDone = false;
        _workshopOutroDone = false;
        _level1IntroDone = false;
    }
}