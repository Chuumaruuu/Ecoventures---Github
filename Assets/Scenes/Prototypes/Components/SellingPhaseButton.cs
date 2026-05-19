using System;
using UnityEngine;

public class SellingPhaseButton : MonoBehaviour
{
    bool isPlayerInside;    
    [SerializeField] private GameObject SellingCamera;
    [SerializeField] private GameObject ExploreCamera;
    [SerializeField] private GameObject SellingUI;
    [SerializeField] private GameObject ExploreUI;


    private void Start() 
    {
        Player_Input playerInput = FindFirstObjectByType<Player_Input>();
        if (playerInput != null) 
        {
            playerInput.OnInteractAlternateAction += PlayerInput_OnInteractAlternateAction;
        }
    }

    private void PlayerInput_OnInteractAlternateAction(object sender, EventArgs e) 
    {
        if (isPlayerInside) 
        {
            SellPhase();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            isPlayerInside = false;
            ExplorePhase();
        }
    }

    public void SellPhase()
    {
        ExploreCamera.SetActive(false);
        SellingCamera.SetActive(true);
        ExploreUI.SetActive(false);
        SellingUI.SetActive(true);
    }

    public void ExplorePhase()
    {
        SellingCamera.SetActive(false);
        ExploreCamera.SetActive(true);
        SellingUI.SetActive(false);
        ExploreUI.SetActive(true);
    }
}

