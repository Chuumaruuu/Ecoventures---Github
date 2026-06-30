using UnityEngine;
using UnityEngine.UI;

public class AchievementTrigger : MonoBehaviour
{
    [SerializeField] private Image achievementImage;

    void Start()
    {
        if (achievementImage != null)
        {
            achievementImage.color = new Color(0f, 0f, 0f, achievementImage.color.a);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetImageColorToWhite();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetImageColorToWhite();
        }
    }

    private void SetImageColorToWhite()
    {
        if (achievementImage != null)
        {
            achievementImage.color = new Color(1f, 1f, 1f, achievementImage.color.a);
        }
    }
}
