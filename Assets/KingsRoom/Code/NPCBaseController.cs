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
    
    protected virtual void Start()
    {
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
}
