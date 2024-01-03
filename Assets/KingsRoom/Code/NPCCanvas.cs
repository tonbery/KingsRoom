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
    [SerializeField] private Image directionImage;
    [SerializeField] Sprite[] directionSprites;
    [SerializeField] private CostDisplay costDisplay;
    [SerializeField] private Button buildButton;
    [SerializeField] private UIButtonCollider buttonCollider;

    public UnityEvent OnAccepted = new UnityEvent();
    private void Awake()
    {
        buttonCollider.pressEvent.AddListener(OnAccepted.Invoke);
    }

    public void SetActionData(BuildingData building, EDirection direction)
    {
        actionIcon.sprite = building.Icon;
        costDisplay.SetCostDisplay(building);
        directionImage.gameObject.SetActive(true);
        directionImage.sprite = directionSprites[(int)direction];
    }

    public void HideButton()
    {
        buildButton.gameObject.SetActive(false);
    }
}
