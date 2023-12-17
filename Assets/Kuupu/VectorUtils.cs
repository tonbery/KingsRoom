using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorUtils
{
    public static Vector3 Rotate(this Vector3 vector, float angle, Vector3 axis)
    {
        return Quaternion.AngleAxis(angle, axis) * vector;
    } 
    
    public static float Angle(this Vector3 vector, Vector3 vector2)
    {
        return Vector3.Angle(vector, vector2);
    }  
    
    public static float Distance(this Vector3 point1, Vector3 point2)
    {
        return Vector3.Distance(point1, point2);
    }  
    
    public static bool IsClose(this Vector3 point1, Vector3 point2, float tolerance)
    {
        return point1.Distance(point2) <= tolerance;
    } 
    public static Vector3 RandomDirection()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    } 
    public static Vector3 RandomPointInRange(Vector3 point, float maxRange, float minRange = 0)
    {
        return point + RandomDirection() * Random.Range(minRange, maxRange);
    } 
    
    public static Vector3 RandomPointAround(this Vector3 point, float maxRange, float minRange = 0)
    {
        return RandomPointInRange(point, maxRange, minRange);
    } 
}
