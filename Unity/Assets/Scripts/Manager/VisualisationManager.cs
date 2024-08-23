using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(ModelManager))]
public class VisualisationManager : MonoBehaviour
{
    public static GameObject CurrentGameObject { get; private set; }
    // public GameObject rotator;
    private GameObject OldGameObject;

    public VisualisationType CurrentVisTyp;
    private VisualisationType OldVisTyp;

    [SerializeField]
    private ModelManager.Type currentModel;
    private ModelManager.Type oldModel;
// public Models.Type currentModel { get { return _currentModel; }  set{if(value != _currentModel) UpdateModel(value); } }

    public Material m_Phong;
    public Material m_Toon;
    public Material m_Fresnel;
    public Material m_Heatmap;
    public Material m_Isolines;
    public Material m_ArrowGlyph;
    public Material m_ArrowGlyphBackground;
    public Material m_PCD;
    public Material m_ScalarField;
    public ScalarValues m_ScalarValue;

    private GameObject m_SmallGlyphsMesh;
    private GameObject m_LargeGlyphsMesh;

    public List<GameObject> CenterObjects;

    private bool m_UpdateExternalObject = false;
    private List<VisualisationType> vis = new List<VisualisationType>();

    public List<KeyValuePair<string, int>> myList = new List<KeyValuePair<string, int>>();

    [HideInInspector] // Hides var below
    // Used to switch between vis in demo
    public int demoCount = 0;


    private void Awake()
    {
        vis.Add(VisualisationType.Heatmap);
        vis.Add(VisualisationType.ArrowGlyphs);
        vis.Add(VisualisationType.Fresnel);
        vis.Add(VisualisationType.Isolines);
        vis.Add(VisualisationType.PCD);
        vis.Add(VisualisationType.Phong);
        vis.Add(VisualisationType.Toon);
        vis.Add(VisualisationType.ScalarField);
           
    }

    void Start()
    {
        SwitchGOWithoutDisable(ModelManager.GetModel(currentModel));
        SwitchVisualisationByType(CurrentVisTyp);
        OldVisTyp = CurrentVisTyp;
        OldGameObject = CurrentGameObject;
        oldModel = currentModel;

        //GameObject liver = Models.CreateGameObject(Models.Type.Liver);
        //var rest = 5;
    }

    private void UpdateModel(ModelManager.Type new_model)
    {
        //CurrentGameObject = ModelManager.GetModel(new_model);
        oldModel = new_model;
        SwitchGOWithDisable(ModelManager.GetModel(new_model));
        OldGameObject = CurrentGameObject;
        Debug.Log($"Model changed to {new_model}");

    }
    // private float anglespeed = 0.05f;
    // private float angle = 1f;
    // public bool record = false;
    
    
    // void FixedUpdate()
    // {
    //     if (record)
    //     {
    //         angle -= anglespeed;
    //         if (angle > 45)
    //         {
    //             SwitchVisualisationByType(vis2[0]);
    //             vis2.RemoveAt(0);
    //             anglespeed *= -1;
    //         }
    //
    //         if (angle < 0)
    //         {
    //             SwitchVisualisationByType(vis2[0]);
    //             vis2.RemoveAt(0);
    //             anglespeed *= -1;
    //         }
    //
    //         rotator.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    //     }
    // }
    
    
    void Update()
    {
       if(currentModel != oldModel)
        {
            UpdateModel(currentModel);
        }

        //if ( CurrentGameObject != OldGameObject)
        //{
        //    SwitchGOWithoutDisable(CurrentGameObject);
        //    OldGameObject = CurrentGameObject;
        //}
        
        if (CurrentVisTyp != OldVisTyp)
        {
            SwitchVisualisationByType(CurrentVisTyp);
            OldVisTyp = CurrentVisTyp;
        }
        

        if (m_UpdateExternalObject)
        {
            UpdateExternalObjectPosition();
        }
    }
    private void UpdateExternalObjectPosition()
    {
        Vector4[] CenterPositionArray = new Vector4[CenterObjects.Count];
        for (int i = 0; i < CenterObjects.Count; i++)
        {
            CenterPositionArray[i] = CenterObjects[i].transform.position;
        }

        if(CurrentVisTyp == VisualisationType.ArrowGlyphs)
        {
            Material[] _Material = new Material[2];
            _Material[0] = m_SmallGlyphsMesh.GetComponent<Renderer>().material;
            _Material[1] = m_LargeGlyphsMesh.GetComponent<Renderer>().material;

            foreach (Material mat in _Material)
            {
                mat.SetInt("_CenterPointAmount", CenterObjects.Count);
                mat.SetVectorArray("_CenterPositionArray", CenterPositionArray);
            }
        }
        else
        {
           Material _Material = CurrentGameObject.GetComponent<Renderer>().material;

            _Material.SetInt("_CenterPointAmount", CenterObjects.Count);
            _Material.SetVectorArray("_CenterPositionArray", CenterPositionArray);
        }


    }
    
    private void DisableExternalObject()
    {
        m_UpdateExternalObject = false;
        DisableGlyphObjects();
    }
    private void EnableExternalObject()
    {
        m_UpdateExternalObject = true;
        DisableGlyphObjects();
    }
    private void EnableGlyphObjects()
    {
        if (m_SmallGlyphsMesh)
            m_SmallGlyphsMesh.SetActive(true);
        if (m_LargeGlyphsMesh)
            m_LargeGlyphsMesh.SetActive(true);
    }

    private void DisableGlyphObjects()
    {
        if(m_SmallGlyphsMesh)
            m_SmallGlyphsMesh.SetActive(false);
        if(m_LargeGlyphsMesh)
            m_LargeGlyphsMesh.SetActive(false);
    }
    private void EnableToon()
    {
        CurrentGameObject.GetComponent<MeshRenderer>().material = m_Toon;
        DisableExternalObject();
        CurrentVisTyp = VisualisationType.Toon;
    }
    private void EnablePhong()
    {
        CurrentGameObject.GetComponent<MeshRenderer>().material = m_Phong;
        DisableExternalObject();
        CurrentVisTyp = VisualisationType.Phong;
    }
    private void EnableFresnel()
    {
        CurrentGameObject.GetComponent<MeshRenderer>().material = m_Fresnel;
        DisableExternalObject();
        CurrentVisTyp = VisualisationType.Fresnel;
    }
    private void EnableHeatmap()
    {
        CurrentGameObject.GetComponent<MeshRenderer>().material = m_Heatmap;
        EnableExternalObject();
        CurrentVisTyp = VisualisationType.Heatmap;
    }
    private void EnablePCD()
    {
        CurrentGameObject.GetComponent<MeshRenderer>().material = m_PCD;
        DisableExternalObject();
        CurrentVisTyp = VisualisationType.PCD;

        // Update unifrom variables

        Renderer rend = CurrentGameObject.GetComponent<Renderer>();
        Material mat = CurrentGameObject.GetComponent<Renderer>().material;

 
        // A sphere that fully encloses the bounding box.
        Vector3 center = rend.bounds.center;
        float radius = rend.bounds.extents.magnitude;
        Vector3 min = rend.bounds.min;
        Vector3 max = rend.bounds.max;


        mat.SetVector("bb_min", min);
        mat.SetVector("bb_max", max);

    }
    private void EnableIsolines()
    {
        CurrentGameObject.GetComponent<MeshRenderer>().material = m_Isolines;
        EnableExternalObject();
        CurrentVisTyp = VisualisationType.Isolines;
    }
    private void EnableArrowGylphs()
    {
        CurrentGameObject.GetComponent<MeshRenderer>().material = m_ArrowGlyphBackground;
        EnableExternalObject();
        EnableGlyphObjects();
        CurrentVisTyp = VisualisationType.ArrowGlyphs;
    }
    private void EnableScalarField()
    {
        if (CurrentGameObject.TryGetComponent<VertexDataLoader>(out VertexDataLoader scalarSettings))
        {
            CurrentGameObject.GetComponent<MeshRenderer>().material = m_ScalarField;
            DisableExternalObject();
            CurrentVisTyp = VisualisationType.ScalarField;
        }
        else
        {
            Debug.LogWarning("GO has no VertexDataLoader. Cant load Scalar Visualization");
        }
    }
    public void SetScalarStep(int step)
    {
        if (CurrentGameObject.TryGetComponent<VertexDataLoader>(out VertexDataLoader scalarSettings))
        {
            scalarSettings.step = step;

        }
        else
        {
            Debug.LogWarning("GO has no VertexDataLoader. Cant load Scalar Visualization");
        }
    }
    public void SetScalarFieldData(ScalarValues scalarData)
    {
        if (CurrentGameObject.TryGetComponent<VertexDataLoader>(out VertexDataLoader scalarSettings))
        {
            scalarSettings.field = scalarData;

        }
        else
        {
            Debug.LogWarning("GO has no VertexDataLoader. Cant load Scalar Visualization");
        }
    }

    public void SwitchGOWithDisable(GameObject go)
    {
        CurrentGameObject.SetActive(false);
        CurrentGameObject = go;
        CurrentGameObject.SetActive(true);

        OldGameObject = CurrentGameObject;
        CurrentVisTyp = VisualisationType.Unknown;
        OldVisTyp = CurrentVisTyp;

        SwitchSamplePoints(go);

    }
    public void SwitchGOWithoutDisable(GameObject go)
    {
        CurrentGameObject = go;
        CurrentGameObject.SetActive(true);

        OldGameObject = CurrentGameObject;
        CurrentVisTyp = VisualisationType.Unknown;
        OldVisTyp = CurrentVisTyp;

        SwitchSamplePoints(go);

    }

    public void SwitchSamplePoints(GameObject go)
    {

        // Switch the SamplePoints when change model
        if (go.transform.childCount == 2)
        {
            m_SmallGlyphsMesh = go.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject;
            m_LargeGlyphsMesh = go.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        }
        else
        {
            m_SmallGlyphsMesh = null;
            m_LargeGlyphsMesh = null;
            Debug.LogError("GameObject has no Childs for Arrow Glyph Vis");

        }
    }
    public void SwitchRefSpeherWithDisable(GameObject refSphere)
    {
        
        foreach (GameObject sphere in CenterObjects)
        {
            sphere.SetActive(false);
        }

        CenterObjects.Clear();

        refSphere.SetActive(true);
        CenterObjects.Add(refSphere);

    }

    //Switch the Vis for Demo scene
    public void SwitchVisForDemo(int i)
    {
        if (demoCount == 0)
        {
            demoCount = 7;
        }
        demoCount = demoCount + i;
        CurrentVisTyp = vis[demoCount % 7];

    }

    public void SwitchVisualisationByType(VisualisationType visType)
    {
        switch (visType)
        {
            case VisualisationType.Toon:
                EnableToon();
                break;
            case VisualisationType.Phong:
                EnablePhong();
                break;
            case VisualisationType.Fresnel:
                EnableFresnel();
                break;
            case VisualisationType.Heatmap:
                EnableHeatmap();
                break;
            case VisualisationType.Isolines:
                EnableIsolines();
                break;
            case VisualisationType.ArrowGlyphs:
                EnableArrowGylphs();
                break;
            case VisualisationType.PCD:
                EnablePCD();
                break;
            case VisualisationType.ScalarField:
                EnableScalarField();
                break;
            default:
                Debug.Log("The Visualisation is unknown.");
                break;

        }
    }


}
