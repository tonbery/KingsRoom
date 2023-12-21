using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct ResourceDisplayData
{  
    public EResourceType ResourceType;
    public int Cost;
    public bool IsGaining;
    public bool IsTimed;

    public ResourceDisplayData(EResourceType newResourceType, int newCost, bool newIsGaining, bool newIsTimed)
    {
        ResourceType = newResourceType;
        Cost = newCost;
        IsGaining = newIsGaining;
        IsTimed = newIsTimed;
    }
}
public class IndividualCostDisplay : MonoBehaviour
{
    [SerializeField] private Sprite[] resourceSprites;
    [SerializeField] private Color gainColor = Color.white;
    [SerializeField] private Color spendColor = Color.red;

    [SerializeField] private Image resourceImage;
    [SerializeField] private Image timeImage;
    [SerializeField] private TextMeshProUGUI costText;
    
    public void SetData(ResourceDisplayData resourceData)
    {
        resourceImage.color = resourceData.IsGaining ? gainColor : spendColor;
        timeImage.color = resourceData.IsGaining ? gainColor : spendColor;
        costText.color = resourceData.IsGaining ? gainColor : spendColor;
        
        costText.SetText(resourceData.Cost.ToString());
        
        timeImage.gameObject.SetActive(resourceData.IsTimed);

        resourceImage.sprite = resourceSprites[(int)resourceData.ResourceType];
    }
}
