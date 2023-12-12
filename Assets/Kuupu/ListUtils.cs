using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUtils<T>
{
    public static T GetRandomElement(List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    
    public static T GetRandomElement(T[] list)
    {
        return list[Random.Range(0, list.Length)];
    }
    
}
