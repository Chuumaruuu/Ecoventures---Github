using UnityEngine;

public class ContainerCounter_Animation : MonoBehaviour
{
    private const string INTERACT = "Interact";

    [SerializeField] private Counter_Container _containerCounter;

    private Animator _containerAnimator;

    private void Awake()
    {
        _containerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _containerCounter.OnPlayerGrabbedObject += ContainerCounterOnPlayerGrabbedObject;
    }

    private void ContainerCounterOnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        _containerAnimator.SetTrigger(INTERACT);
    }
}
