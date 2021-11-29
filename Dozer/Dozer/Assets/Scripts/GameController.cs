using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float power;
    private void Awake()
    {

        var paintableObjs = FindObjectsOfType<ColorChanger>().OfType<IColorChangerRandomly>().ToList();
        foreach (var paintableObj in paintableObjs)
        {
            paintableObj.SelectColorRandomly();
        }
    }

    private void Update()
    {
        
        Camera.main.gameObject.transform.position += new Vector3(0,0,1) * Time.deltaTime * power;
    }
}
