using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCanvasVisibility : MonoBehaviour
{
    [SerializeField] private float interactionDistance;
    [SerializeField] private LayerMask interactiveMask;
    [SerializeField] private PlayerController playerController;

    private NPCBaseController _lastNPC;
    private GameObject _lastNPCObject;
    
    private RaycastHit _hit;
    private void FixedUpdate()
    {
        var hitSomething = playerController.MainCamera.RaycastForward(out _hit, interactionDistance, interactiveMask);
        
        Debug.DrawRay(playerController.MainCamera.transform.position, playerController.MainCamera.transform.forward * interactionDistance);

        if (hitSomething)
        {
            SetNPC(_hit.GameObject());
        }
        else
        {
            if (_lastNPCObject)
            {
                float angle = playerController.MainCamera.transform.forward.Angle(_lastNPCObject.transform.position - transform.position);
                Debug.Log(angle);
                if(angle > 45) SetNPC(null);
            }
            else SetNPC(null);
        }
    }

    void SetNPC(GameObject newNPC)
    {
        if (_lastNPCObject != newNPC)
        {
            if (_lastNPC != null)
            {
                _lastNPC.SetOnPlayerVision(false);
            }

            if (newNPC != null)
            {
                _lastNPCObject = newNPC;
                _lastNPC = newNPC.GetComponent<NPCBaseController>();
                _lastNPC.SetOnPlayerVision(true);    
            }
            else
            {
                _lastNPCObject = null;
                _lastNPC = null;
            }
        }
    }
}
