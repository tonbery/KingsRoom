using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPCCanvas : MonoBehaviour
{
    [SerializeField] private Image actionIcon;
    [SerializeField] private CostDisplay costDisplay;
    [SerializeField] private Button buildButton;
    [SerializeField] private UIButtonCollider buttonCollider;

    public UnityEvent OnAccepted = new UnityEvent();
    private void Awake()
    {
        buttonCollider.pressEvent.AddListener(OnAccepted.Invoke);
    }

    public void SetActionData(BuildingData building)
    {
        actionIcon.sprite = building.Icon;
        costDisplay.SetCostDisplay(building);
    }

    public void HideButton()
    {
        buildButton.gameObject.SetActive(false);
    }
}
