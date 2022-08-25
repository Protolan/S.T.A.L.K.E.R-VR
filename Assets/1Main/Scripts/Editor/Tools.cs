using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Tools
{

    [MenuItem("Tools/GroupByMaterial")]
    public static void GroupByMaterial()
    {
        var selection = Selection.activeTransform; 
        var renderers = selection.GetComponentsInChildren<MeshRenderer>();
        if(renderers == null || renderers.Length == 0) return;
        Undo.RecordObjects(renderers, "ChangeHierarchy");
        var materialGroup = from renderer in renderers
            group renderer by renderer.sharedMaterial;
        foreach (var group in materialGroup)
        {
            var newGroupTransform = new GameObject(group.Key.mainTexture.name);
            newGroupTransform.transform.parent = selection;
            foreach (var meshRenderer in group) 
                meshRenderer.transform.parent = newGroupTransform.transform;
        }
    }
    
    [MenuItem("Tools/GetConvexMeshes")]
    public static void SetCollides()
    {
        var parentObject = Selection.activeTransform;
        var collidersRoot = Object.Instantiate(parentObject, parentObject.position, parentObject.rotation, null);
        collidersRoot.name = parentObject.name + "Colliders";

        foreach (Transform child in collidersRoot.GetComponentsInChildren<Transform>())
        {
            if (child.TryGetComponent(out MeshFilter meshFilter))
            {
                var collider = child.gameObject.AddComponent<MeshCollider>();
                collider.sharedMesh = meshFilter.sharedMesh;
                collider.convex = true;
            }

            foreach (var component in child.GetComponents<Component>())
            {
                if (component is Transform or Collider) continue;
                Object.DestroyImmediate(component);
            }
        }

        EditorUtility.SetDirty(collidersRoot);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    } 
    [MenuItem("Tools/GetMeshColliders")]
    public static void SetMeshCollides()
    {
        var parentObject = Selection.activeTransform;
        var collidersRoot = Object.Instantiate(parentObject, parentObject.position, parentObject.rotation, null);
        collidersRoot.name = parentObject.name + "Colliders";

        foreach (Transform child in collidersRoot.GetComponentsInChildren<Transform>())
        {
            if (child.TryGetComponent(out MeshFilter meshFilter))
            {
                var collider = child.gameObject.AddComponent<MeshCollider>();
                collider.sharedMesh = meshFilter.sharedMesh;
            }

            foreach (var component in child.GetComponents<Component>())
            {
                if (component is Transform or Collider) continue;
                Object.DestroyImmediate(component);
            }
        }

        EditorUtility.SetDirty(collidersRoot);
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }


  
   
}
