using UnityEngine;
using UnityEngine.UI;

public class HintDisplayPanel : MonoBehaviour
{
    [SerializeField] private Image hintImage;

    public void Setup(Sprite hintSprite)
    {
        if (hintImage == null)
        {
            return;
        }

        hintImage.sprite = hintSprite;
        hintImage.enabled = hintImage.sprite != null;
    }
}