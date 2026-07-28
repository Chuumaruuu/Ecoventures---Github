using UnityEngine;

[CreateAssetMenu(fileName = "Achievement_Progress", menuName = "Scriptable Objects/Achievement_Progress")]
public class Achievement_Progress : ScriptableObject
{ 
    
    public bool CURIOUS_JED;
    public bool ECO_VENTURED;
    public bool FOR_REAL;
    public bool INFINITE_SOLUTIONS;
    public bool QUIZ_RUSH;

    // Achievement Status
        private int itemsUnlocked;
        private int itemsSold;
        private int hintsViewed;
        private int itemsCrafted;
        private int itemsThrown;

    public void Reset()
    {
        CURIOUS_JED=
        ECO_VENTURED=
        FOR_REAL=
        INFINITE_SOLUTIONS=
        QUIZ_RUSH=

        false;
    }

    public void AddItemsUnlocked(int value)
    {
        itemsUnlocked += value;
    }

    public int CheckItemsUnlocked()
    {
        return itemsUnlocked;
    }

    public void AddItemsSold(int value)
    {
        itemsSold += value;
    }

    public int CheckItemsSold()
    {
        return itemsSold;
    }

    public void AddHintsViewed(int value)
    {
        hintsViewed += value;
    }

    public int CheckHintsViewed()
    {
        return hintsViewed;
    }

    public void AddItemsCrafted(int value)
    {
        itemsCrafted += value;
    }

    public int CheckItemsCrafted()
    {
        return itemsCrafted;
    }

    public void AddItemsThrown(int value)
    {
        itemsThrown += value;
    }

    public int CheckItemsThrown()
    {
        return itemsThrown;
    }


}