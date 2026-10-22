using System;
using UnityEngine;

public enum CameraStatus
{
    Static,
    Follow
}

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance { get; private set; }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    
    
}
