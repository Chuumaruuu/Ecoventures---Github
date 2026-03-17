using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager Instance { get; private set; }

    [Header("Startup")]
    [SerializeField] private string firstSceneToLoad;
    [SerializeField] private bool autoLoadFirstScene = true;

    [Header("Fade")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.8f;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image loadingBarFill;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private TextMeshProUGUI percentageText;
    [SerializeField] private float fillSpeed = 2f;
    [SerializeField] private float minimumLoadingTime = 1f;

    [Header("Auto-Hide Scene Canvases")]
    [SerializeField] private bool autoHideSceneCanvases = true;

    private bool isTransitioning;
    private Canvas transitionCanvas;
    private Coroutine currentTransition;

    // Static properties for scene loaders to update
    public static float CustomLoadingProgress { get; set; } = 0f;
    public static bool IsSceneReady { get; set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            transitionCanvas = GetComponent<Canvas>();
            if (transitionCanvas != null)
            {
                transitionCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
                transitionCanvas.sortingOrder = 9999;
            }

            InitializeUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (autoLoadFirstScene && !string.IsNullOrEmpty(firstSceneToLoad))
        {
            LoadScene(firstSceneToLoad);
        }
    }

    private void InitializeUI()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            fadeCanvasGroup.blocksRaycasts = true;
            fadeCanvasGroup.gameObject.SetActive(true);
        }

        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }

        if (loadingBarFill != null)
        {
            loadingBarFill.fillAmount = 0f;
        }
    }

    public void LoadScene(string sceneName)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("Scene transition already in progress!");
            return;
        }

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is empty!");
            return;
        }

        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }

        currentTransition = StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        isTransitioning = true;

        // Reset loading state
        CustomLoadingProgress = 0f;
        IsSceneReady = false;

        if (transitionCanvas != null)
        {
            transitionCanvas.sortingOrder = 9999;
            transitionCanvas.gameObject.SetActive(true);
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = true;
            fadeCanvasGroup.gameObject.SetActive(true);
        }

        if (autoHideSceneCanvases)
        {
            HideAllSceneCanvases();
        }

        // Fade out before loading
        yield return FadeOut();

        // Show loading UI
        if (loadingPanel != null)
            loadingPanel.SetActive(true);

        if (loadingBarFill != null)
            loadingBarFill.fillAmount = 0f;

        UpdateLoadingUI("Loading Scene...", 0f);

        float startTime = Time.time;
        float displayProgress = 0f;

        // Load scene in background
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        // PHASE 1: Load scene (0% → 90%)
        while (operation.progress < 0.9f)
        {
            float targetProgress = Mathf.Clamp01(operation.progress / 0.9f) * 0.5f;
            displayProgress = Mathf.MoveTowards(displayProgress, targetProgress, fillSpeed * Time.deltaTime);
            UpdateLoadingUI("Loading Scene...", displayProgress);
            yield return null;
        }

        // PHASE 2: Wait for custom scene loading (50% → 100%)
        UpdateLoadingUI("Loading Environment...", 0.5f);
        float timeout = 10f;
        float timeoutStart = Time.time;

        while (!IsSceneReady && Time.time - timeoutStart < timeout)
        {
            float targetProgress = 0.5f + (CustomLoadingProgress * 0.5f);
            displayProgress = Mathf.MoveTowards(displayProgress, targetProgress, fillSpeed * Time.deltaTime);
            UpdateLoadingUI("Loading Environment...", displayProgress);
            yield return null;
        }

        if (!IsSceneReady)
            Debug.LogWarning("Scene loader timed out. Proceeding anyway.");

        // Ensure minimum loading time
        float elapsed = Time.time - startTime;
        if (elapsed < minimumLoadingTime)
            yield return new WaitForSeconds(minimumLoadingTime - elapsed);

        // 🎯 ACTIVATE SCENE ONLY AFTER ALL LOADING IS DONE
        operation.allowSceneActivation = true;

        while (!operation.isDone)
            yield return null;

        // Fade in after scene is fully loaded
        yield return FadeIn();

        if (loadingPanel != null)
            loadingPanel.SetActive(false);

        isTransitioning = false;
        currentTransition = null;
    }

    private void UpdateLoadingUI(string text, float progress)
    {
        if (loadingBarFill != null)
            loadingBarFill.fillAmount = progress;

        if (percentageText != null)
            percentageText.text = Mathf.RoundToInt(progress * 100f) + "%";

        if (loadingText != null)
            loadingText.text = text;
    }

    private void HideAllSceneCanvases()
    {
        Canvas[] allCanvases = Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
        foreach (Canvas canvas in allCanvases)
        {
            if (canvas != transitionCanvas)
                canvas.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeOut()
    {
        if (fadeCanvasGroup == null) yield break;

        fadeCanvasGroup.gameObject.SetActive(true);
        fadeCanvasGroup.blocksRaycasts = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;
    }

    private IEnumerator FadeIn()
    {
        if (fadeCanvasGroup == null) yield break;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    public bool IsTransitioning => isTransitioning;
}
