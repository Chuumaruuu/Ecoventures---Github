using UnityEngine;

[CreateAssetMenu(fileName = "Achievement_Progress", menuName = "Scriptable Objects/Achievement_Progress")]
public class Achievement_Progress : ScriptableObject
{ 
    
    public bool CURIOUS_JED;
    public bool ECO_VENTURED;
    public bool FOR_REAL;
    public bool INFINITE_SOLUTIONS;
    public bool QUIZ_RUSH;

    // for tracker testing
    private int _testCounter;

    public void Reset()
    {
        CURIOUS_JED=
        ECO_VENTURED=
        FOR_REAL=
        INFINITE_SOLUTIONS=
        QUIZ_RUSH=

        false;
    }

    public void AddCount() //tas eto nalang ccall mo pag magdadagdag ka
    {
        _testCounter++;
    }
}