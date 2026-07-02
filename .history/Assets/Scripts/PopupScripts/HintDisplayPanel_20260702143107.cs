using UnityEngine;
using UnityEngine.UI;

public class HintDisplayPanel : MonoBehaviour
{
    [SerializeField] private Image hintImage;

    public void Setup(Hints_Data hintsData)
    {
        if (hintImage == null)
        {
            return;
        }

        hintImage.sprite = GetRandomSprite(hintsData);
        hintImage.enabled = hintImage.sprite != null;
    }

    private Sprite GetRandomSprite(Hints_Data hintsData)
    {
        if (hintsData == null || hintsData._hintSprites == null || hintsData._hintSprites.Length == 0)
        {
            return null;
        }

        int index = Random.Range(0, hintsData._hintSprites.Length);
        return hintsData._hintSprites[index];
    }
}