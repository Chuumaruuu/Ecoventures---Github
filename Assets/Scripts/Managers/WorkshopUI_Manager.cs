using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopUI_Manager : MonoBehaviour
{
    //MAIN UI
    [SerializeField] private GameObject _mainUI;


    //TUTORIAL ARROW
    
    public List<Arrow> _arrowList;
    private GameObject _activeArrow;

    public void HideMainUI()
    {
        _mainUI.SetActive(false);
    }

    public void ShowMainUI()
    {
        _mainUI.SetActive(true);
    }

    public void ShowArrow(string i)
    {
        foreach (Arrow a in _arrowList)
        {
            if (i == a.GetTitle())
            {
                _activeArrow = a.GetArrowUI();
                a.GetArrowUI().SetActive(true);
            }
            else
            {
                Debug.LogError(i + " arrow does not exist");
            }
        }
    }

    public void HideArrow()
    {
        if (_activeArrow != null)
        {
            _activeArrow.SetActive(false);
            _activeArrow = null;
        }
        else
        {
            Debug.Log("No Active arrow");
        }
    }

    [Serializable]
    public class Arrow
    {
        [SerializeField] private string _arrowName;
        [SerializeField] private GameObject _arrowUI;

        public string GetTitle()
        {
            return _arrowName;
        }

        public GameObject GetArrowUI()
        {
            return _arrowUI;
        }
    }

}
