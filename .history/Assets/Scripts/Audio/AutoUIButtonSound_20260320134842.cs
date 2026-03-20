using UnityEngine;
using UnityEngine.UI;

public class AutoUIButtonSound : MonoBehaviour
{
    private void Start()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsSortMode.None);
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