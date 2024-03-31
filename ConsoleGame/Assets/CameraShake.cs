using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shakeForce = 1f;
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cvc;
    private float shakeTimer;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        //cvc = GetComponent<CinemachineVirtualCamera>();
    }

    public void CamShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(shakeForce);
    }

    //public void ShakeCamera(float intensity, float timer )
    //{
    //    CinemachineBasicMultiChannelPerlin cvcPerlin = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    //    cvcPerlin.m_AmplitudeGain = intensity;
    //    shakeTimer = timer;
    //}

    //private void Update()
    //{
    //    if (shakeTimer > 0)
    //    {
    //        shakeTimer -= Time.deltaTime;
    //        if (shakeTimer < 0)
    //        {
    //            CinemachineBasicMultiChannelPerlin cvcPerlin = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    //            cvcPerlin.m_AmplitudeGain = 0f;
    //        }
    //    }
    //}
}
