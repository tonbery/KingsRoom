using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class NPCBaseController : MonoBehaviour
{
    [SerializeField] protected Canvas UICanvas;
    protected NPCCanvas _NPCCanvas;
    //public UnityEvent<EBuildingType, EDirection> OnRequestConstruction = new UnityEvent<EBuildingType, EDirection>();
    private PlayerController _playerController;
    private bool canShowUI;
    private bool wantShowUI = true;
    
    protected virtual void Start()
    {
        //HideUI();
        _playerController = GameMode.Instance.PlayerController;
        _NPCCanvas = UICanvas.GetComponent<NPCCanvas>();
        UICanvas.worldCamera = _playerController.MainCamera;
        _NPCCanvas.OnAccepted.AddListener(OnAcceptedButtonClicked);
    }

    protected virtual void OnAcceptedButtonClicked()
    {
        //OnRequestConstruction.Invoke(EBuildingType.NUM, EDirection.NUM);
    }

    protected virtual void Update()
    {
        if (UICanvas)
        {
            var camForward = _playerController.MainCamera.transform.forward;
            camForward.y = 0;
            UICanvas.transform.forward = camForward;
        }
    }

    public void SetUIVisibilityState(bool newState)
    {
        canShowUI = newState;
        UpdateUIState();
    }

    public void SetUIVisibilityRequest(bool newShow)
    {
        wantShowUI = newShow;
        UpdateUIState();
    }

    void UpdateUIState()
    {
        if (canShowUI && wantShowUI)
        {
            ShowUI();
        }
        else HideUI();
    }
    
    void ShowUI()
    {
        UICanvas.gameObject.SetActive(true);
    }

    void HideUI()
    {
        UICanvas.gameObject.SetActive(false);
    }
}
