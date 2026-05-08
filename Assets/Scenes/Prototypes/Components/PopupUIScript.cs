using System;
using UnityEngine;

public class PopupUIScript : MonoBehaviour
{
    bool isPlayerInside;    
    [SerializeField] private GameObject MainPanel;
    [SerializeField] private GameObject PopupPanel;

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
            OpenPopup();
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
            ClosePopup();
        }
    }

    public void OpenPopup()
    {
        MainPanel.SetActive(false);
        PopupPanel.SetActive(true);
    }

    public void ClosePopup()
    {
        MainPanel.SetActive(true);
        PopupPanel.SetActive(false);
    }
}
