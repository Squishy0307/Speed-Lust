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
    [SerializeField] ParticleSystem speedParticles;

    private bool isPlayer;
    private VehicleMovement vehicle;

    private void Awake()
    {
        cam = Camera.main;
        camStartFOV = cam.fieldOfView;

        isPlayer = gameObject.CompareTag("Player");
        vehicle = GetComponent<VehicleMovement>();
    }

    private void Update()
    {
        if (!isPlayer) return;

        if(vehicle.GetCurrentSpeed() > 20) 
        {
            speedParticles.Play();
        }
        else
        {
            speedParticles.Stop();
        }
    }

    public void IncreaseFOV(float FOV, float timeToReachDesireFOV)
    {
        if (isPlayer)
        {
            DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, FOV, timeToReachDesireFOV);
        }
    }

    public void MegaBoostFOVIncrease(float timeToReachDesireFOV)
    {
        if (isPlayer)
        {
            DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, megaBoostFOV, timeToReachDesireFOV).SetEase(ease);
        }
    }

    public void ResetFOV(float timeToResetFOV)
    {
        if (isPlayer)
        {
            DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, camStartFOV, timeToResetFOV);
        }
    }
}
