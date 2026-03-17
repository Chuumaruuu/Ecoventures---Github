using UnityEngine;
using UnityEngine.UI;

public class CustomerOrderUI : MonoBehaviour
{
    public Vector2 screenOffset = new Vector2(0f, 100f);
    private Customer customer;
    private RectTransform rectTransform;
    private Image iconImage;
    private Canvas canvas;

    [Header("Patience UI")]
    public Slider patienceSlider;
    public Image fillImage;

    [Header("Patience Colors")]
    public Color fullColor = Color.green;
    public Color midColor = Color.yellow;
    public Color emptyColor = Color.red;

    public void Setup(Customer targetCustomer)
    {
        customer = targetCustomer;
        rectTransform = GetComponent<RectTransform>();
        iconImage = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();

        if (customer.desiredItem != null && customer.desiredItem._itemSprite != null)
        {
            iconImage.sprite = customer.desiredItem._itemSprite;
            iconImage.enabled = true;
        }
        else
        {
            iconImage.enabled = false;
        }

        if (patienceSlider != null)
        {
            patienceSlider.maxValue = customer.maxWaitTime;
            patienceSlider.value = customer.maxWaitTime;
        }

        if (fillImage == null && patienceSlider != null)
        {
            fillImage = patienceSlider.fillRect.GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (customer == null)
        {
            Destroy(gameObject);
            return;
        }

        if (patienceSlider != null && customer.state == CustomerState.Waiting)
        {
            patienceSlider.value = customer.waitTimer;

            float t = 1f - (customer.waitTimer / customer.maxWaitTime);
            if (t < 0.5f)
                fillImage.color = Color.Lerp(fullColor, midColor, t * 2f);
            else
                fillImage.color = Color.Lerp(midColor, emptyColor, (t - 0.5f) * 2f);
        }

        Vector3 worldPos = customer.transform.position + Vector3.up * 2f;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        rectTransform.position = screenPos + new Vector3(screenOffset.x * canvas.scaleFactor, screenOffset.y * canvas.scaleFactor, 0);
    }
}