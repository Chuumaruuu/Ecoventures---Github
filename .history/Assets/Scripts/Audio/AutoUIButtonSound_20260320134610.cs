using UnityEngine;
using UnityEngine.UI;

public class AutoUIButtonSound : MonoBehaviour
{
    private void Start()
    {
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() =>
            {
                if (SoundManager.Instance != null && SoundManager.Instance.buttonClickClip != null)
                {
                    SoundManager.Instance.PlaySFX(SoundManager.Instance.buttonClickClip);
                }
            });
        }
    }
}