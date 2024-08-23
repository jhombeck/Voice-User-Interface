using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VoiceVisualizationProcessor : MonoBehaviour
{

    public VisualisationManager visMan;
    public MicrophoneStreamingBehavior mic_streaming;
    private static float rotation_speed = 1;
    public SceneType sceneType;

    private void Update()
    {
        //Mute And Unmute Mic
        if (SteamVR_Actions._default.InteractUI.GetStateDown(SteamVR_Input_Sources.LeftHand))
        {
            MicrophoneBehavior.MuteMic(false);
        }
        if (SteamVR_Actions._default.InteractUI.GetStateUp(SteamVR_Input_Sources.LeftHand))
        {
            MicrophoneBehavior.MuteMic(true);
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            MicrophoneBehavior.MuteMic(false);
            Debug.Log("Listining...");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            MicrophoneBehavior.MuteMic(true);
            Debug.Log("Stoped.");
        }
    }
    void Start()
    {
        SetOriginalPosition();
        MicrophoneBehavior.MuteMic(true);

        SetModiForVoiceModel(sceneType);

        Debug.Log("init DONE");
    }
    bool first = true;
    private void FixedUpdate()
    {
        if (first)
        {
            first = false;
        }

    }

    private void SetModiForVoiceModel(SceneType voiceTasks)
    {
        switch (voiceTasks)
        {
            case SceneType.Analysis:
                StartCoroutine(mic_streaming.set_locomotion_mode("AnalysisScenario"));
                break;
            case SceneType.Navigation:
                StartCoroutine(mic_streaming.set_locomotion_mode("NavigationScenario"));
                break;
            case SceneType.Properties:
                StartCoroutine(mic_streaming.set_locomotion_mode("PropertiesScenario"));
                break;
            default:
                break;
        }

    }

    //=================================================================================================
    // Object Manipulation 
    //=================================================================================================

    // Rotate the current object around its X-axis by a specified angle.
    // Params:
    // - rotationAngleX: The angle of rotation around the X-axis.
    // Examples:
    // - INPUT: Rotate the object around the X-axis by 90 degrees.
    //   OUTPUT: Method: RotateX, Params: [90]
    // - INPUT: Rotate the object around the X-axis by -45 degrees.
    //   OUTPUT: Method: RotateX, Params: [-45]
    public void RotateX(float rotationAngleX)
    {
        GameObject obj = GetGameObject().transform.parent.gameObject;
        Transform cameraTransform = Camera.main.transform;
        //Quaternion rotation = Quaternion.AngleAxis(rotationAngleX, cameraTransform.right);
        Quaternion rotation = Quaternion.AngleAxis(rotationAngleX, Vector3.right);
        obj.transform.rotation = rotation * obj.transform.rotation;
    }

    // Rotate the current object around its Y-axis by a specified angle.
    // Params:
    // - rotationAngleY: The angle of rotation around the Y-axis.
    // Examples:
    // - INPUT: Rotate the object around the Y-axis by 90 degrees.
    //   OUTPUT: Method: RotateY, Params: [90]
    // - INPUT: Rotate the object around the Y-axis by -45 degrees.
    //   OUTPUT: Method: RotateY, Params: [-45]
    public void RotateY(float rotationAngleY)
    {
        GameObject obj = GetGameObject().transform.parent.gameObject;
        Quaternion rotation = Quaternion.AngleAxis(rotationAngleY, Vector3.up);
        obj.transform.rotation = rotation * obj.transform.rotation;
    }

    // Rotate the current object around its Z-axis by a specified angle.
    // Params:
    // - rotationAngleZ: The angle of rotation around the Z-axis.
    // Examples:
    // - INPUT: Rotate the object around the Z-axis by 90 degrees.
    //   OUTPUT: Method: RotateY, Params: [90]
    // - INPUT: Rotate the object around the Z-axis by -45 degrees.
    //   OUTPUT: Method: RotateY, Params: [-45]
    public void RotateZ(float rotationAngleZ)
    {
        GameObject obj = GetGameObject().transform.parent.gameObject;
        Transform cameraTransform = Camera.main.transform;
        Quaternion rotation = Quaternion.AngleAxis(rotationAngleZ, Vector3.forward);
        obj.transform.rotation = rotation * obj.transform.rotation;
    }



    // Rotate the current object around its X-axis continuously.
    // Params: None
    // Examples:
    // - INPUT: Start continuous rotation around the X-axis.
    //   OUTPUT: Method: RotateXContinuously, Params: []
    // - INPUT: None
    //   OUTPUT: None
    private Coroutine rotateCO;
    public void RotateXContinuously()
    {
        GameObject obj = GetGameObject().transform.parent.gameObject;
        Transform cameraTransform = Camera.main.transform;
        if (rotateCO == null)
            rotateCO = StartCoroutine(RotateXCo(Vector3.right, obj));
    }
    // Rotate the current object around its Y-axis continuously.
    // Params: None
    // Examples:
    // - INPUT: Start continuous rotation around the Y-axis.
    //   OUTPUT: Method: RotateYContinuously, Params: []
    // - INPUT: None
    //   OUTPUT: None
    public void RotateYContinuously()
    {
        GameObject obj = GetGameObject().transform.parent.gameObject;
        Transform cameraTransform = Camera.main.transform;
        if (rotateCO == null)
            rotateCO = StartCoroutine(RotateXCo(Vector3.up, obj));
    }
    // Rotate the current object around its Z-axis continuously.
    // Params: None
    // Examples:
    // - INPUT: Start continuous rotation around the Z-axis.
    //   OUTPUT: Method: RotateZContinuously, Params: []
    // - INPUT: None
    //   OUTPUT: None
    public void RotateZContinuously()
    {
        GameObject obj = GetGameObject().transform.parent.gameObject;
        Transform cameraTransform = Camera.main.transform;
        if (rotateCO == null)
            rotateCO = StartCoroutine(RotateXCo(Vector3.forward, obj));
    }




    // Adjust the rotation speed of the current object.
    // Params:
    // - speedModifier: The speed modifier to adjust the rotation speed. Use SpeedModifier.Faster to increase the speed and SpeedModifier.Slower to decrease it.
    // Examples:
    // - INPUT: Increase the rotation speed.
    //   OUTPUT: Method: AdjustRotationSpeed, Params: [SpeedModifier.Faster]
    // - INPUT: Decrease the rotation speed.
    //   OUTPUT: Method: AdjustRotationSpeed, Params: [SpeedModifier.Slower]
    public void AdjustRotationSpeed(SpeedModifier speedModifier)
    {
        float speedDelta = 0f;
        switch (speedModifier)
        {
            case SpeedModifier.Faster:
                speedDelta = 2f; // Increase speed by 2 units per second
                break;
            case SpeedModifier.Slower:
                speedDelta = -2f; // Decrease speed by 2 unit per second
                break;
        }
        rotation_speed += speedDelta;
        rotation_speed = Mathf.Max(rotation_speed, 0); // Ensure rotation speed is not negative
    }

    // Stop the rotation of the current object.
    // Examples:
    // - INPUT: Stop the rotation of the object.
    //   OUTPUT: Method: StopRotation, Params: []
    // - INPUT: Stop.
    //   OUTPUT: Method: StopRotation, Params: []
    public void StopRotation()
    {
        StopCoroutine(rotateCO);
        rotateCO = null;
    }
    // Set the view of the current object to a specified canonical direction.
    // Params:
    // - view: The canonical view direction to set.
    // Examples:
    // - INPUT: Set the view of the object to the front view.
    //   OUTPUT: Method: SetView, Params: [CanonicalViewDirection.Front]
    // - INPUT: Change view to top.
    //   OUTPUT: Method: SetView, Params: [CanonicalViewDirection.Top]
    public void SetView(CanonicalViewDirection view)
    {
        // Get the desired rotation based on the view direction
        Quaternion targetRotation = Quaternion.identity; // Default rotation

        switch (view)
        {
            case CanonicalViewDirection.Front:
                targetRotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case CanonicalViewDirection.Back:
                targetRotation = Quaternion.Euler(0f, -180f, 0f);
                break;
            case CanonicalViewDirection.Left:
                targetRotation = Quaternion.Euler(0f, 90f, 0f);
                break;
            case CanonicalViewDirection.Right:
                targetRotation = Quaternion.Euler(0f, -90f, 0f);
                break;
            case CanonicalViewDirection.Top:
                targetRotation = Quaternion.Euler(-90f, 0f, 0f);
                break;
            case CanonicalViewDirection.Bottom:
                targetRotation = Quaternion.Euler(90f, 0f, 0f);
                break;
        }
        GameObject obj = GetGameObject().transform.parent.gameObject;
        // Apply the rotation to the object
        obj.transform.rotation = targetRotation;
    }

    // Restore the current object to its original position, orientation, and zoom level.
    // Examples:
    // - INPUT: Reset the object to its original state.
    //   OUTPUT: Method: Reset, Params: []
    // - INPUT: Undo any modifications made to the object during inspection.
    //   OUTPUT: Method: Reset, Params: []

    private Transform original_transform;
    public void Reset()
    {
        if (rotateCO != null)
        {
            StopCoroutine(rotateCO);
            rotateCO = null;
        }


        GameObject obj = GetGameObject().transform.parent.gameObject;
        //obj.transform.position = Vector3.zero; // Reset position
        obj.transform.rotation = Quaternion.identity; // Reset rotation
                                                      // obj.transform.localScale = Vector3.one; // Reset scale
    }


    //=================================================================================================
    // VISUALIZATION 
    //=================================================================================================


    // Switch the visualization of the current object based on the specified type.
    // Params:
    // - vis: The type of visualization to switch to.
    // Examples:
    // - INPUT: Switch the current objects visualization to Heatmap.
    //   OUTPUT: Method: SwitchVisualization, Params: VisualisationType.Heatmap
    // - INPUT: Change the visualization to use Phong shading.
    //   OUTPUT: Method: SwitchVisualization, Params: VisualisationType.Phong
    public void SwitchVisualization(VisualisationType vis)
    {
        switch (vis)
        {
            case VisualisationType.Heatmap:
                visMan.SwitchVisualisationByType(VisualisationType.Heatmap);
                break;
            case VisualisationType.Phong:
                visMan.SwitchVisualisationByType(VisualisationType.Phong);
                break;
            case VisualisationType.Unknown:
                break;
            default:
                break;
        }

    }
    // Enable or disable isolines for the scalar field visualization.
    // Params:
    // - enable: Set to true to enable isolines, or false to disable them.
    // Examples:
    // - INPUT: Enable isolines .
    //   OUTPUT: Method: EnableIsolines, Params: [true]
    // - INPUT: Disable isolines.
    //   OUTPUT: Method: EnableIsolines, Params: [false]
    public void EnableIsolines(bool enable)
    {
        GameObject obj = GetGameObject();
        Material material = Helper.GetMaterial(obj);

        var name = material.name;

        if (material.name != "Mat_Heatmap (Instance)")
        {
            Debug.Log("Cant enable isolines without heatmap active.");
        }

        material.SetInt("_IsolinesAmount", 8);

        float enable_isolines = enable ? 1 : 0;
        material.SetFloat("_IsolinesEnabled", enable_isolines);

    }

    // Set the range of values for the heatmap color mapping.
    // Params:
    // - range: The new range value for the heatmap color mapping.
    // Examples:
    // - INPUT: Set the heatmap range to 50.
    //   OUTPUT: Method: SetHeatmapRange, Params: [50]
    // - INPUT: Set the heatmap range to 100.
    //   OUTPUT: Method: SetHeatmapRange, Params: [100]
    public void SetHeatmapRange(float range)
    {
        GameObject obj = GetGameObject();
        Material material = Helper.GetMaterial(obj);

        var name = material.name;

        if (material.name != "Mat_Heatmap (Instance)")
        {
            Debug.Log("Cant enable isolines without heatmap active.");
        }

        material.SetFloat("_HeatmapRadius", range / 100);
    }

    // Set the main color for visualization elements.
    // Params:
    // - color: The new main color to set as (R, G, B).
    // Examples:
    // - INPUT: Set the color to red.
    //   OUTPUT: Method: SetMainColor, Params: [(1, 0, 0)]
    // - INPUT: Set the main color to blue.
    //   OUTPUT: Method: SetMainColor, Params: [(0, 0, 1)]
    public void SetMainColor(Colors col)
    {
        GameObject obj = GetGameObject();
        Material material = Helper.GetMaterial(obj);

        Color newColor = Helper.GetColorFromEnum(col);

        material.SetColor("_Color", newColor);

    }

    // Set the secondary color for visualization elements.
    // Params:
    // - color: The new secondary color to set as (R, G, B).
    // Examples:
    // - INPUT: Set the secondary color to green.
    //   OUTPUT: Method: SetSecondaryColor, Params: [(0, 1, 0)]
    // - INPUT: Change the secondary color to yellow.
    //   OUTPUT: Method: SetSecondaryColor, Params: [(1, 1, 0)]
    public void SetSecondaryColor(Colors col)
    {
        GameObject obj = GetGameObject();
        Material material = Helper.GetMaterial(obj);

        Color newColor = Helper.GetColorFromEnum(col);

        material.SetColor("_ColorCenter", newColor);

    }



    //=================================================================================================
    // Analytics 
    //=================================================================================================

    // Show the values, based on the provided value type (min, max, or average).
    // Params:
    // - val: The type of value (min, max, or average) to display.
    // Examples:
    // - INPUT: Display the minimum value.
    //   OUTPUT: Method: ShowValuesAt, Params: [Value.Min]
    // - INPUT: Show average.
    //   OUTPUT: Method: ShowValuesAt, Params: [Value.Average]
    public void ShowValuesAt(MinMaxValue val)
    {
        GameObject obj = GetGameObject();
        Material material = Helper.GetMaterial(obj);

        if (!obj.TryGetComponent<VertexDataLoader>(out VertexDataLoader vertexDataLoader))
        {
            Debug.LogWarning("No VertexDataLoader found on object.");
        }

        float minValue = vertexDataLoader.min;
        float maxValue = vertexDataLoader.max;
        float enableThreshold = 0;
        float percentage = 0.40f;

        switch (val)
        {
            case MinMaxValue.Min:
                minValue = vertexDataLoader.min;
                maxValue = minValue + (Math.Abs(vertexDataLoader.max - vertexDataLoader.min) * percentage);
                enableThreshold = 1;
                break;
            case MinMaxValue.Max:
                maxValue = vertexDataLoader.max;
                minValue = maxValue - (Math.Abs(vertexDataLoader.max - vertexDataLoader.min) * percentage);
                enableThreshold = 1;
                break;
            case MinMaxValue.Average:
                float avg = (vertexDataLoader.max + vertexDataLoader.min) / 2f;
                maxValue = avg + (Math.Abs(vertexDataLoader.max - vertexDataLoader.min) * (percentage / 2));
                minValue = avg - (Math.Abs(vertexDataLoader.max - vertexDataLoader.min) * (percentage / 2));
                enableThreshold = 1;
                break;
            default:
                break;
        }

        material.SetFloat("_ThresholdMinValue", minValue);
        material.SetFloat("_ThresholdMaxValue", maxValue);
        material.SetFloat("_EnableThresholdDisplay", enableThreshold);


    }


    // Switch the currently displayed scalar field to a new DataVis.
    // Params:
    // - dataVis: The new DataVis representing the scalar field to switch to.
    // Examples:
    // - INPUT: Switch to a new DataVis representing current data.
    //   OUTPUT: Method: SwitchScalarField, Params: [DataVis.Current]
    // - INPUT: Switch to a new DataVis representing pressure data.
    //   OUTPUT: Method: SwitchScalarField, Params: [DataVis.Pressure]
    public void SwitchScalarField(ScalarValues dataVis)
    {
        visMan.SetScalarFieldData(dataVis);

    }


    // Display the range in percentage around a specified value (min, max, or average) and color it accordingly.
    // Params:
    // - val: The type of value (min, max, or average) to display.
    // - color: The color to use for displaying the range.
    // - percentage: The percentage of the range to display around the value.
    // Examples:
    // - INPUT: Display the range around the average value in red with a range of 10%.
    //   OUTPUT: Method: ShowSplitOfData, Params: [Value.Average, red, 10]
    // - INPUT: Display the 5% range around the minimum value in green.
    //   OUTPUT: Method: ShowSplitOfData, Params: [Value.Min, green, 5]
    public void ShowSplitOfData(MinMaxValue val, Colors col, float percentage)
    {


        Color newColor = Helper.GetColorFromEnum(col);

        GameObject obj = GetGameObject();
        Material material = Helper.GetMaterial(obj);

        if (!obj.TryGetComponent<VertexDataLoader>(out VertexDataLoader vertexDataLoader))
        {
            Debug.LogWarning("No VertexDataLoader found on object.");
        }

        float minValue = vertexDataLoader.min;
        float maxValue = vertexDataLoader.max;
        float enableThreshold = 0;
        percentage = percentage / 100;

        switch (val)
        {
            case MinMaxValue.Min:
                minValue = vertexDataLoader.min;
                maxValue = minValue + (Math.Abs(vertexDataLoader.max - vertexDataLoader.min) * percentage);
                enableThreshold = 1;
                break;
            case MinMaxValue.Max:
                maxValue = vertexDataLoader.max;
                minValue = maxValue - (Math.Abs(vertexDataLoader.max - vertexDataLoader.min) * percentage);
                enableThreshold = 1;
                break;
            case MinMaxValue.Average:
                float avg = (vertexDataLoader.max + vertexDataLoader.min) / 2f;
                maxValue = avg + (Math.Abs(vertexDataLoader.max - vertexDataLoader.min) * (percentage / 2));
                minValue = avg - (Math.Abs(vertexDataLoader.max - vertexDataLoader.min) * (percentage / 2));
                enableThreshold = 1;
                break;
            default:
                break;
        }


        material.SetFloat("_ThresholdMinValue", minValue);
        material.SetFloat("_ThresholdMaxValue", maxValue);
        material.SetFloat("_EnableThresholdDisplay", enableThreshold);
        material.SetColor("_ColorMax", newColor);
    }

    // Change the timestep of a scalar field to a new integer value.
    // Params:
    // - step: The new integer timestep value to set for the scalar field.
    // Examples:
    // - INPUT: Change the timestep to 1 for the scalar field.
    //   OUTPUT: Method: ChangeTimeStep, Params: [1]
    // - INPUT: Show step to 2.
    //   OUTPUT: Method: ChangeTimeStep, Params: [2]
    public void ChangeTimeStep(int step)
    {
        visMan.SetScalarStep(step);
    }



    //=========================
    //Helper function, no need to Call by voice 
    //=========================
    IEnumerator RotateXCo(Vector3 cameraTransform, GameObject obj)
    {
        while (true)
        {
            Quaternion rotation = Quaternion.AngleAxis(rotation_speed, cameraTransform);
            obj.transform.rotation = rotation * obj.transform.rotation;
            yield return new WaitForSeconds(0.01f);
        }
    }
    private GameObject GetGameObject()
    {
        // Obtain the current GameObject from VisualisationManager.CurrentGameObject
        // Adjust the code here based on how you access the current GameObject
        GameObject obj = VisualisationManager.CurrentGameObject;
        return obj;
    }

    private void SetOriginalPosition()
    {
        original_transform = GetGameObject().transform;
    }
}
//=================================================================================================
// Enums 
//=================================================================================================

// Enum representing different value options for data visualization
public enum MinMaxValue
{
    Min,
    Max,
    Average
}
//  Enum representing canonical Views,

public enum CanonicalViewDirection
{
    Front,
    Back,
    Left,
    Right,
    Top,
    Bottom
}
public enum SpeedModifier
{
    Faster,
    Slower
}
public enum Colors
{
    green,
    yellow,
    blue,
    red,
    orange,
    purple,
    pink,
    brown,
    black,
    white,
    grey,
    magenta,
    cyan,
    silver,
    gold
}
public enum SceneType
{
    Analysis,
    Navigation,
    Properties
}