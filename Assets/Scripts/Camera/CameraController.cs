using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera _virtualCamera;
    private CinemachineBasicMultiChannelPerlin _virtualCameraBMCP;
    private float shakeTimer;
    private float totalShakeTime;
    private float startingIntensity;
    
    // Start is called before the first frame update
    void Start()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _virtualCameraBMCP = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            _virtualCameraBMCP.m_AmplitudeGain = Mathf.Lerp(0F, startingIntensity, shakeTimer / totalShakeTime);
        }
    }

    public void ShakeCamera(float intensity, float time)
    {
        print("SHAKING! " + intensity + " " + time);
        startingIntensity = intensity;
        shakeTimer = time;
        totalShakeTime = time;
    }
}
