using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TaskOpener : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform taskPanel;    // The panel that will slide
    public Button openButton;          // The button that triggers slide
    public float slideDuration = 0.3f; // Duration of slide
    public Vector2 hiddenPosition;    // Offscreen position
    public Vector2 shownPosition;     // Onscreen position

    private bool isOpen = false;

    private void Start()
    {
        // Assign button listener
        openButton.onClick.AddListener(TogglePanel);

        // Start with panel hidden
        taskPanel.anchoredPosition = hiddenPosition;
    }

    public void TogglePanel()
    {
        StopAllCoroutines();
        StartCoroutine(SlidePanel(isOpen ? hiddenPosition : shownPosition));
        isOpen = !isOpen;

        // Optional: Rotate button arrow
        openButton.transform.rotation = Quaternion.Euler(0, 0, isOpen ? 180 : 0);
    }

    private IEnumerator SlidePanel(Vector2 targetPosition)
    {
        Vector2 startPos = taskPanel.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < slideDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / slideDuration);
            // Smooth interpolation
            taskPanel.anchoredPosition = Vector2.Lerp(startPos, targetPosition, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        taskPanel.anchoredPosition = targetPosition;
    }
}