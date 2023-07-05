using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Http;
using static System.Net.WebRequestMethods;
using System;
using System.Numerics;
using System.Net;
using UnityEngine.Experimental.PlayerLoop;

public class Request : MonoBehaviour
{

    public string url="http://localhost:5000/getHPandSpeed";
    public int numberPreset;
    // Use this for initialization
    void Awake()
    {
        StartCoroutine(SendRequest());  
    }

    private IEnumerator SendRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError)
        {
            Debug.Log("Error While Sending: " + request.error);
        }
        else
        {
            string json = "{\"presets\":" + request.downloadHandler.text + "}";
            Responce responce = JsonUtility.FromJson<Responce>(json);
            PlayerHealthController.instance.maxHealth = responce.presets[numberPreset].healthplayerID;
            PlayerController.instance.moveSpeed = responce.presets[numberPreset].speedplayerID;
            Debug.Log(responce.presets[numberPreset].healthplayerID);
        }

    }
}
[System.Serializable]
public struct Preset
{
    public int presetID;
    public int healthplayerID;
    public int speedplayerID;
    public bool healthplayer;
    public bool speedplayer;
}
[System.Serializable]
public struct Responce
{
    public Preset[] presets; 
}
