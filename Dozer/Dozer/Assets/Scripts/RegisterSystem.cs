using System;
using UnityEngine;


public class RegisterSystem : MonoBehaviour , IRegisterSystem
{
    public void SaveData(string key, int value)
    {
        PlayerPrefs.SetInt(key,value);
    }

    public void SaveData(string key, float value)
    {
        PlayerPrefs.SetFloat(key,value);
    }

    public void SaveData(string key, string value)
    {
        PlayerPrefs.SetString(key,value);
    }

    public int GetDataAsInt(string key)
    {
        return PlayerPrefs.GetInt(key);
    }

    public float GetDataAsFloat(string key)
    {
        return PlayerPrefs.GetFloat(key);
    }

    public string GetDataAsString(string key)
    {
        return PlayerPrefs.GetString(key);
    }
}
