using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputModule : StandaloneInputModule 
{
    private Vector2 _cursorPos;
    private readonly MouseState _mouseState = new MouseState();
  
    public override void UpdateModule()
    {
        _cursorPos.x = Screen.width / 2;
        _cursorPos.y = Screen.height / 2;
    }

    /*protected bool GetPointerData(int id, out PointerEventData data, bool create)
    {
        if (!m_PointerData.TryGetValue(id, out data) && create)
        {
            data = new PointerEventData(eventSystem)
            {
                pointerId = id,
            };
            m_PointerData.Add(id, data);
            return true;
        }
        return false;
    }*/

    /*protected override MouseState GetMousePointerEventData()
    {
        var created = GetPointerData( kMouseLeftId, out var leftData, true );
 
        leftData.Reset();
 
        if (created) leftData.position = _cursorPos;
 
        Vector2 pos = _cursorPos;
        leftData.delta = pos - leftData.position;
        leftData.position = pos;
        leftData.scrollDelta = Input.mouseScrollDelta;
        leftData.button = PointerEventData.InputButton.Left;
        eventSystem.RaycastAll(leftData, m_RaycastResultCache);
        var raycast = FindFirstRaycast(m_RaycastResultCache);
        leftData.pointerCurrentRaycast = raycast;
        m_RaycastResultCache.Clear();

        GetPointerData(kMouseRightId, out var rightData, true);
        CopyFromTo(leftData, rightData);
        rightData.button = PointerEventData.InputButton.Right;

        GetPointerData(kMouseMiddleId, out var middleData, true);
        CopyFromTo(leftData, middleData);
        middleData.button = PointerEventData.InputButton.Middle;
 
        _mouseState.SetButtonState(PointerEventData.InputButton.Left, StateForMouseButton(0), leftData);
        _mouseState.SetButtonState(PointerEventData.InputButton.Right, StateForMouseButton(1), rightData);
        _mouseState.SetButtonState(PointerEventData.InputButton.Middle, StateForMouseButton(2), middleData);
 
        return _mouseState;
    }*/
    
    protected override MouseState GetMousePointerEventData(int id) {
        var lockState = Cursor.lockState;
        var visibleState = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        var mouseState = base.GetMousePointerEventData(id);
        Cursor.lockState = lockState;
        Cursor.visible = visibleState;
        return mouseState;
    }
 
    protected override void ProcessMove(PointerEventData pointerEvent) {
        var lockState = Cursor.lockState;
        var visibleState = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        base.ProcessMove(pointerEvent);
        Cursor.lockState = lockState;
        Cursor.visible = visibleState;
    }
 
    protected override void ProcessDrag(PointerEventData pointerEvent) {
        var lockState = Cursor.lockState;
        var visibleState = Cursor.visible;
        Cursor.lockState = CursorLockMode.None;
        base.ProcessDrag(pointerEvent);
        Cursor.lockState = lockState;
        Cursor.visible = visibleState;
    }
}
