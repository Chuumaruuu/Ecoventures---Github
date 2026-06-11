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

        hintImage.sprite = hintsData != null ? hintsData._hintSprite : null;
        hintImage.enabled = hintImage.sprite != null;
    }
}