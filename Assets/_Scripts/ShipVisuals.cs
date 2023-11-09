using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class ShipVisuals : MonoBehaviour
{
    private Camera cam;
    float camStartFOV;
    float megaBoostFOV = 110;

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] AnimationCurve ease;

    private void Awake()
    {
        cam = Camera.main;
        camStartFOV = cam.fieldOfView;
    }

    public void IncreaseFOV(float FOV, float timeToReachDesireFOV)
    {
        if (gameObject.CompareTag("Player"))
        {
            if (virtualCamera.gameObject.activeSelf)
            {
                DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, FOV, timeToReachDesireFOV);
            }

            else
            {
                DOTween.To(() => cam.fieldOfView, x => cam.fieldOfView = x, FOV, timeToReachDesireFOV);
            }
        }
    }

    public void MegaBoostFOVIncrease(float timeToReachDesireFOV)
    {
        if (transform.gameObject.CompareTag("Player"))
        {
            if (virtualCamera.gameObject.activeSelf)
            {
                DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, megaBoostFOV, timeToReachDesireFOV).SetEase(ease);
            }
            else
            {
                DOTween.To(() => cam.fieldOfView, x => cam.fieldOfView = x, megaBoostFOV, timeToReachDesireFOV);
            }
        }
    }

    public void ResetFOV(float timeToResetFOV)
    {
        if (transform.gameObject.CompareTag("Player"))
        {
            if (virtualCamera.gameObject.activeSelf)
            {
                DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, camStartFOV, timeToResetFOV);
            }
            else
            {
                DOTween.To(() => cam.fieldOfView, x => cam.fieldOfView = x, camStartFOV, timeToResetFOV);
            }
        }
    }
}
