using System.Collections.Generic;
using NaughtyAttributes;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Collider collider;
    [SerializeField] private List<Transform> points;
    [SerializeField] private float distance = 3;
    [SerializeField] private float radius = 8;

    public List<Transform> Points => points;

    #if UNITY_EDITOR
    [Button]
    private void GeneratePoints()
    {
        float scale = transform.localScale.x;
        
        foreach (var point in points)
        {
            if(point) DestroyImmediate(point.gameObject);
        }
        
        points.Clear();

        for (int i = 0; i < 20; i++)
        {
            if(points.Count >= 5) break;
            
            Vector3 position = VectorUtils.RandomPointAround(Vector3.zero, distance * 2 * scale, distance * 2 * scale);

            bool isValid = true;
            
            foreach (var point in points)
            {
                if (point.transform.localPosition.Distance(position) <= radius*2*scale)
                {
                    isValid = false;
                    break;
                }
            }

            if (!isValid) continue;
            
            var nGO = new GameObject();
            nGO.name = "Constuction Point " + points.Count.ToString();
            nGO.transform.SetParent(gameObject.transform);
            nGO.transform.localPosition = position;
            points.Add(nGO.transform);
        }
        EditorUtility.SetDirty(gameObject);
        EditorUtility.SetDirty(this);
        
    }
    #endif

    
    private void OnDrawGizmos()
    {
        float scale = transform.localScale.x;
        
        foreach (var point in points)
        {
            if (point)
            {
                Gizmos.color = new Color(1f, 1f, 1f, 0.5f);
                Gizmos.DrawSphere(point.position, radius*scale);
                Gizmos.color = Color.white;    
            }
        }
    }

    public void ProcessPoints()
    {
        float scale = transform.localScale.x;
        
        string myLayer = LayerMask.LayerToName(gameObject.layer);
        LayerMask mask = LayerMask.GetMask(myLayer);
        Collider[] colliders = new Collider[20];

        for (int i = points.Count - 1; i >= 0; i--)
        {
            var hits = Physics.OverlapSphereNonAlloc(points[i].position, radius * scale, colliders, mask);
            if (hits > 0)
            {
                for (int j = 0; j < hits; j++)
                {
                    if (colliders[j].gameObject != gameObject)
                    {
                        Destroy(points[i].gameObject);
                        points.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }
}
