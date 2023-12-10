using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorGroupGameObjects : MonoBehaviour
{
    [MenuItem("Edit/Group %g", false, 3)]
    static void Group()
    {
        var objects = Selection.gameObjects;
        if (objects.Length <= 0) return;
        var newObject = new GameObject();
        Undo.RegisterCreatedObjectUndo(newObject, "Create my GameObject");
        newObject.transform.position = objects[0].transform.position;
        newObject.name = "Group";
        EditorUtility.SetDirty(newObject);
        foreach (var obj in objects)
        {
            Undo.SetTransformParent(obj.transform, newObject.transform, true, "Parenting");
            EditorUtility.SetDirty(obj);
        }
        
        
    }
}
