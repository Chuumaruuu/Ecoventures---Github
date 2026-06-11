using UnityEngine;

public class SellingMode : MonoBehaviour
{
    [SerializeField] private GameObject sellingModeUI;
    private UI_Manager uiManager;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        if (UI_Manager.Instance != null)
        {
            UI_Manager.Instance.EnterSellingMode();
        }
    }
}
