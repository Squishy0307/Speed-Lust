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

    private void Awake()
    {
        cam = Camera.main;
        camStartFOV = cam.fieldOfView;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            MegaBoostFOVIncrease(1.2f);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ResetFOV(1f);
        }
    }

    public void increaseFOV(float timeToReachDesireFOV,float FOV)
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

    public void MegaBoostFOVIncrease(float timeToReachDesireFOV)
    {
        if (transform.gameObject.CompareTag("Player"))
        {
            if (virtualCamera.gameObject.activeSelf)
            {
                DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, megaBoostFOV, timeToReachDesireFOV);
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
