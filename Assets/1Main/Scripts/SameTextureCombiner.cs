using System.IO;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class SameTextureCombiner: MonoBehaviour
{
    public MeshRenderer[] Renderers;
    [FolderPath] public string materialFolderPath;

    [Button]
    public void Combine()
    {
        var textureGroup = from renderer in Renderers
            group renderer by renderer.sharedMaterial.mainTexture;

        foreach (var group in textureGroup)
        {
            var material = new Material(Shader.Find("Standard"));
            material.color = Color.white;
            material.mainTexture = group.Key;
            material = SaveMaterial(material, group.Key.name + "material");
            foreach (var meshRenderer in group) 
                meshRenderer.sharedMaterial = material;
        }
        AssetDatabase.Refresh();
    }

    private Material SaveMaterial(Material material, string name)
    {
        var materialPath = materialFolderPath + "/" + name + ".mat";
        AssetDatabase.CreateAsset(material, materialPath);
        return (Material)AssetDatabase.LoadAssetAtPath(materialPath, typeof(Material));
    }
        
   

   
    
   

}