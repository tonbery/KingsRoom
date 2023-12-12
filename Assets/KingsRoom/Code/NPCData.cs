using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Kings/NPC")]
public class NPCData : ScriptableObject
{
    [SerializeField] private EBuildingType buildingType;
    [SerializeField] private NPCRequesterController prefab;

    public EBuildingType BuildingType => buildingType;
    public NPCRequesterController Prefab => prefab;
}