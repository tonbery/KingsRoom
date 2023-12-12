using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPCCanvas : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private Button buildButton;
    [SerializeField] private UIButtonCollider buttonCollider;

    public UnityEvent OnAccepted = new UnityEvent();
    private void Awake()
    {
        //buildButton.onClick.AddListener(OnAccepted.Invoke);
        buttonCollider.pressEvent.AddListener(OnAccepted.Invoke);
    }

    public void SetText(string message, bool visibleButton)
    {
        questionText.text = message;
        if (!visibleButton) Destroy(buildButton.gameObject);
        
    }
}
