﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatmapSetup : MonoBehaviour
{
    Renderer _Rend;
    Material _Material;
    public List<GameObject> CenterObjects;
    // Start is called before the first frame update
    void Start()
    {
        _Rend = gameObject.GetComponent<Renderer>();
        _Material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update() 
    {
        Vector4[] CenterPositionArray = new Vector4[CenterObjects.Count];
        for (int i = 0; i < CenterObjects.Count; i++)
        {
            CenterPositionArray[i] = CenterObjects[i].transform.position;
        }


        //Debug.Log(CenterPoints[0].transform.position);
        _Material.SetInt("_CenterPointAmount", CenterObjects.Count);
        _Material.SetVectorArray("_CenterPositionArray", CenterPositionArray);
    }
}
