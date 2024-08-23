using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Helper : MonoBehaviour
{

    public static SpeedModifier GetSpeedModifierFromString(string enm)
    {

        return (SpeedModifier)Enum.Parse(typeof(SpeedModifier), enm, true);
    }
    public static CanonicalViewDirection GetCanonicalViewDirectionFromString(string enm)
    {

        return (CanonicalViewDirection)Enum.Parse(typeof(CanonicalViewDirection), enm, true);
    }
    public static VisualisationType GetVisualisationTypeFromString(string enm)
    {

        return (VisualisationType)Enum.Parse(typeof(VisualisationType), enm, true);
    }
    public static Colors GetColorsFromString(string enm)
    {

        return (Colors)Enum.Parse(typeof(Colors), enm, true);
    }
    public static MinMaxValue GetMinMaxValueFromString(string enm)
    {

        return (MinMaxValue)Enum.Parse(typeof(MinMaxValue), enm, true);
    }
    public static ScalarValues GetScalarFieldDataFromString(string enm)
    {

        return (ScalarValues)Enum.Parse(typeof(ScalarValues), enm, true);
    }

    public static GameObject GetGOFromID(GameObject[,,] grid, int location_number)
    {

        foreach (var go in grid)
        {
            if (location_number == GetIDFromGO(go))
            {
                Debug.Log("Found at" + go.transform.position);
                return go;

            }
        }
        return null;
    }
    public static GameObject GetGOFromID(GameObject[] grid, int location_number)
    {

        foreach (var go in grid)
        {
            if (go)
            {
                if (location_number == GetIDFromGO(go.transform.GetChild(0).gameObject))
                {
                    Debug.Log("Found at" + go.transform.position);
                    return go;

                }
            }
        }
        return null;
    }
    public static int GetIDFromGO(GameObject teleport_location)
    {

        return int.Parse(teleport_location.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text);

    }

    public static float GetHeightAtPosition(float x, float z)
    {

        float y = -100;
        RaycastHit[] hits = Physics.RaycastAll(new Vector3(x, 100f, z), Vector3.down, 1000);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider != null && !hit.collider.isTrigger)
            {
                y = Mathf.Max(y, hit.point.y);
            }
        }

        return y;
    }

    public static void SetHeigt(GameObject go)
    {

        float xForHieght = go.transform.position.x;
        float zForHieght = go.transform.position.z;
        go.transform.position = new Vector3(xForHieght, GetHeightAtPosition(xForHieght, zForHieght), zForHieght);

    }

    public static Vector3 GetIntersection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return new Vector3(float.NaN, float.NaN, float.NaN);

    }


    public static int ReduceToTwoDigits(int value)
    {

        int sum = 0;

        for (int i = 1; i < 100; i *= 10)
        {

            sum += value % 10 * i;
            value /= 10;

        }
        return sum;

    }


    // Shader Helper
    public static Material GetMaterial(GameObject go)
    {
        return go.GetComponent<Renderer>().material;


    }

    // Colors
    public static Color GetColorFromEnum(Colors color)
    {
        Color newColor;

        switch (color)
        {
            case Colors.green:
                newColor = Color.green;
                break;
            case Colors.yellow:
                newColor = Color.yellow;
                break;
            case Colors.blue:
                newColor = Color.blue;
                break;
            case Colors.red:
                newColor = Color.red;
                break;
            case Colors.orange:
                newColor = new Color(1f, 0.5f, 0f);
                break;
            case Colors.purple:
                newColor = new Color(0.5f, 0f, 0.5f);
                break;
            case Colors.pink:
                newColor = new Color(1f, 0.5f, 0.5f);
                break;
            case Colors.brown:
                newColor = new Color(0.6f, 0.4f, 0.2f);
                break;
            case Colors.black:
                newColor = Color.black;
                break;
            case Colors.white:
                newColor = Color.white;
                break;
            case Colors.grey:
                newColor = Color.grey;
                break;
            case Colors.magenta:
                newColor = Color.magenta;
                break;
            case Colors.cyan:
                newColor = Color.cyan;
                break;
            case Colors.silver:
                newColor = new Color(0.75f, 0.75f, 0.75f);
                break;
            case Colors.gold:
                newColor = new Color(1f, 0.843f, 0f);
                break;
            default:
                newColor = Color.clear;
                break;
        }

        return newColor;
    }


}
