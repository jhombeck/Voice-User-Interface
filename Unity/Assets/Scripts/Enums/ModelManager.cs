using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Model
{
    public ModelManager.Type name;

    public GameObject go;

}
[System.Serializable]
public class ModelManager : MonoBehaviour
{
    public enum Type
    {
        Liver,
        Vessel,
        Aneurysm
    }
    public List<Model> _all_models;
    private static List<Model> all_models;

    private void Awake()
    {
        all_models = _all_models;
    }

    public static GameObject GetModel(ModelManager.Type model_type)
    {
        foreach (var item in all_models)
        {
            if (item.name == model_type)
            {
                return item.go;
            }
        }
        return null;
    }


}
