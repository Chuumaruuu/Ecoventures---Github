using UnityEngine;

public class BinCounter_Animation : MonoBehaviour
{
    private const string INTERACT = "Interact";

    [SerializeField] private Counter_Bin _containerCounter;

    private Animator _containerAnimator;

    private void Awake()
    {
        _containerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _containerCounter.OnPlayerThrewObject += BinCounterOnPlayerThrewObject;
    }

    private void BinCounterOnPlayerThrewObject(object sender, System.EventArgs e)
    {
        _containerAnimator.SetTrigger(INTERACT);
    }
}
