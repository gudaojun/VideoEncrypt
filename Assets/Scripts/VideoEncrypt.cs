using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class VideoEncrypt : MonoBehaviour
{
    public string videoPath;
    //private bool isDecode =false;

    private void Start()
    {
        videoPath = Application.streamingAssetsPath + "/logo.mp4";
        Debug.Log(videoPath);
        FileOffsetTools.Encode(videoPath);
       // FileOffsetTools.Decode(videoPath);
    }
}