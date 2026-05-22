using UnityEngine;

public class Guide_Manager : MonoBehaviour
{
    private int _currentGuideIndex = 0;
    
    private void setGuideIndex(int index) 
    {
        _currentGuideIndex = index;
    }
}
