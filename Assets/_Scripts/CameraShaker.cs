using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraShaker : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    private CinemachineBasicMultiChannelPerlin _NoiseSetting;

    public static CameraShaker Instance;
    private bool isShaking = false; 

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        _cam = GetComponent<CinemachineVirtualCamera>();   
        _NoiseSetting = _cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeNow(float ShakeIntensity,float ShakeTime, bool isPlayer)
    {
        if (isPlayer && !isShaking)
        {
            StartCoroutine(shaker(ShakeIntensity, ShakeTime));
        }
        
    }

    IEnumerator shaker(float ShakeIntensity, float ShakeTime)
    {
        isShaking = true;
        DOTween.To(() => _NoiseSetting.m_AmplitudeGain, x => _NoiseSetting.m_AmplitudeGain = x, ShakeIntensity, ShakeTime);
        
        yield return new WaitForSecondsRealtime(ShakeTime + 0.05f);
        
        DOTween.To(() => _NoiseSetting.m_AmplitudeGain, x => _NoiseSetting.m_AmplitudeGain = x, 0, 0.5f);
        isShaking = false;
    }
}
