using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
    public static Vector3 Rotate(this Vector3 vector, float angle, Vector3 axis)
    {
        return Quaternion.AngleAxis(angle, axis) * vector;
    }    
    
    public static float Distance(this Vector3 point1, Vector3 point2)
    {
        return Vector3.Distance(point1, point2);
    }  
    
    public static bool IsClose(this Vector3 point1, Vector3 point2, float tolerance)
    {
        return point1.Distance(point2) <= tolerance;
    } 
}
