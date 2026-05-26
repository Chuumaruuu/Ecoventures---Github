using UnityEngine;

public class Map_PopupUI : MonoBehaviour
{
    [SerializeField] private GameObject _targetUI;
    [SerializeField] private float _detectionRadius = 5f;

    private Animator _mapUIAnimator;

    void Start()
    {
        _mapUIAnimator = _targetUI.GetComponent<Animator>();
    }

    void Update()
    {
        // Create a sphere overlap check
        Collider[] hits = Physics.OverlapSphere(transform.position, _detectionRadius);

        bool playerDetected = false;

        foreach (Collider hit in hits)
        {
            // Check if the object has Map_Player component
            if (hit.GetComponent<Map_Player>() != null)
            {
                playerDetected = true;
                break;
            }
        }

        if (playerDetected)
        {
            _mapUIAnimator.SetBool("Popup", true);
        }
        else
        {
            _mapUIAnimator.SetBool("Popup", false);
        }
    }

    // Draw the detection radius in Scene View
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRadius);
    }
}