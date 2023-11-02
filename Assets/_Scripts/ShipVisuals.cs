using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipVisuals : MonoBehaviour
{
    private Camera cam;
    float camStartFOV;
    float megaBoostFOV = 110;

    private void Awake()
    {
        cam = Camera.main;
        camStartFOV = cam.fieldOfView;
    }

    public void increaseFOV(float timeToReachDesireFOV,float FOV)
    {
        DOTween.To(() => cam.fieldOfView, x => cam.fieldOfView = x, FOV, timeToReachDesireFOV);
    }

    public void MegaBoostFOVIncrease(float timeToReachDesireFOV)
    {
        if (transform.gameObject.CompareTag("Player"))
        {
            DOTween.To(() => cam.fieldOfView, x => cam.fieldOfView = x, megaBoostFOV, timeToReachDesireFOV);
        }
    }

    public void ResetFOV(float timeToResetFOV)
    {
        if (transform.gameObject.CompareTag("Player"))
        {
            DOTween.To(() => cam.fieldOfView, x => cam.fieldOfView = x, camStartFOV, timeToResetFOV);
        }
    }
}
