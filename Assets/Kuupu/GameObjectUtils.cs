using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtils
{
   public static bool RaycastForward(this Camera component, out RaycastHit hit, float distance, int layerMask)
   {
      var t = component.transform;
      return Physics.Raycast(t.position, t.forward, out hit, distance, layerMask);
   }
   public static bool RaycastForward(this MonoBehaviour component, out RaycastHit hit, float distance, int layerMask)
   {
      var t = component.transform;
      return Physics.Raycast(t.position, t.forward, out hit, distance, layerMask);
   }
   public static bool RaycastForward(this GameObject gameObject, out RaycastHit hit, float distance, int layerMask)
   {
      var t = gameObject.transform;
      return Physics.Raycast(t.position, t.forward, out hit, distance, layerMask);
   }
   public static bool RaycastForward(this Transform transform, out RaycastHit hit, float distance, int layerMask)
   {
      return Physics.Raycast(transform.position, transform.forward, out hit, distance, layerMask);
   }

   public static T GetComponent<T>(this MonoBehaviour component)
   {
      return component.gameObject.GetComponent<T>();
   }
   
   public static T GetComponent<T>(this RaycastHit hit)
   {
      if (hit.collider == null) return default;
      return hit.collider.gameObject.GetComponent<T>();
   }
   
   public static GameObject GameObject(this RaycastHit hit)
   {

      return hit.collider.gameObject;
   }
}
