using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;
using System.Globalization;

public enum ScalarValues
{
    Pressure,
    Stress,
    WSS
}
[System.Serializable]
public class ScalarField
{
    public ScalarValues Type;
    public TextAsset data;


}
[System.Serializable]
public class VertexDataLoader : MonoBehaviour
{

    // Save which ScalarValue should be displayed 
    public ScalarValues field;
    public ScalarValues currentField;
    // Step of Scalar Field
    [Range(0, 21)]
    public int step;
    private int currentStep;

    public List<ScalarField> data;

    public float min;
    public float max;
    public bool AutomaticBounds = false;


    private Mesh mesh;
    private Renderer renderer;
    private List<List<float>> scalarField;

    private void UpdateScalarField(int step)
    {
        Vector2[] pressureVAO = GetValueAtStep(step);
        currentStep = step;
        // Set Variables for shader
        SetVAO(pressureVAO);
    }


    private void SetMinMax(float min, float max)
    {
        renderer.material.SetFloat("_MinValue", min);
        renderer.material.SetFloat("_MaxValue", max);
    }

    private void SetVAO(Vector2[] pressureVAO)
    {
        var test = mesh.vertexCount;
        var firstvert = mesh.vertices;
        mesh.SetUVs(2, pressureVAO);

    }


    private List<List<float>> ProcessData(ScalarValues activeField)
    {

        TextAsset data = GetScalarField(activeField);


        string inputString = data.text;

        List<List<float>> linesValues = new List<List<float>>();

        string[] lines = inputString.Split('\n', (char)StringSplitOptions.RemoveEmptyEntries);
        foreach (string line in lines)
        {
            string[] values = line.Split(' ');
            List<float> floatValues = new List<float>();

            foreach (string value in values)
            {
                // Replace '.' with ',' before parsing to handle the correct delimiter.
                // Replace('.', ',')
                if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out float floatValue))
                {
                    floatValues.Add(floatValue);
                }
            }

            linesValues.Add(floatValues);
        }
        return linesValues;
    }

    private TextAsset GetScalarField(ScalarValues activeField)
    {
        foreach (var item in data)
        {
            if(item.Type == activeField)
            {
                return item.data;
            }
        }


        Debug.Log("No Matching Scalar Field Found");
        return null;
    }

    Vector2[] GetValueAtStep(int step)
    {
        Vector2[] results = new Vector2[scalarField.Count-1];

        for (int i = 0; i < scalarField.Count-1; i++)
        {

            results[i].x = scalarField[i][step];
           
        }
        return results;
    }
    public (float minValue, float maxValue) FindMinMax(List<List<float>> scalarField)
    {
        if (scalarField == null || scalarField.Count == 0)
            throw new ArgumentException("The scalarField list is empty or null.");

        float minValue = float.MaxValue;
        float maxValue = float.MinValue;

        foreach (var row in scalarField)
        {
            if (row == null || row.Count == 0)
                continue;

            foreach (var value in row)
            {
                if (value < minValue)
                    minValue = value;

                if (value > maxValue)
                    maxValue = value;
            }
        }

        return (minValue, maxValue);
    }
    private void UpdateMinMax()
    {

            (min, max) = FindMinMax(scalarField);
            SetMinMax(min, max);

    }
    private void UpdateDataSet()
    {
        currentField = field;
        // Load Vector Field 
        scalarField = ProcessData(field);
        currentStep = step;
        UpdateScalarField(step);
        // Update Bounds if enabled
        if (AutomaticBounds)
        {
            UpdateMinMax();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Setup Variables
        mesh = transform.GetComponent<MeshFilter>().mesh;
        renderer = GetComponent<Renderer>();

        UpdateDataSet();

    }

    private void Update()
    {

        // When Data Set is changed
        if (currentField != field)
        {
            UpdateDataSet();

        }
        // When step is changed 
        if (currentStep != step )
        {
            currentStep = step;
            UpdateScalarField(step);

        }

    }
}
