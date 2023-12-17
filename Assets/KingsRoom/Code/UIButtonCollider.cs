using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class UIButtonCollider : MonoBehaviour
{
    public UnityEvent pressEvent = new UnityEvent();
    public void Press()
    {
        pressEvent.Invoke();
    }
}
