using UnityEngine;

[CreateAssetMenu(fileName = "Recipe_Data", menuName = "Scriptable Objects/Recipe_Data")]
public class Recipe_Data : ScriptableObject
{
    public string recipeName;
    public int recipeID;
    public Sprite[] recipeSprites;
}