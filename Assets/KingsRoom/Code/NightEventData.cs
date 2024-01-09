using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class NightEventData : ScriptableObject
{
    [SerializeField] int startDay;
    [SerializeField] bool canBeDefended;
    [SerializeField, ShowIf("canBeDefended")] int damage;
    [SerializeField] ResourceCost[] cost;
    [SerializeField, ResizableTextArea()] string description;

    public int StartDay => startDay;
    public bool CanBeDefended => canBeDefended;
    public int Damage => damage;
    public ResourceCost[] Cost => cost;
    public string Description => description;
}
