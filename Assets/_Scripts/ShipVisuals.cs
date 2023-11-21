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
    [SerializeField] AnimationCurve megaBoostFOVIncreaseEase;
    [SerializeField] ParticleSystem speedParticles;
    [SerializeField] Transform shipModel;

    [SerializeField] GameObject[] shipParts;

    private bool isPlayer;
    private bool isBreaking = false;
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
            DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, megaBoostFOV, timeToReachDesireFOV).SetEase(megaBoostFOVIncreaseEase);
        }

        StartCoroutine(slowlyBreakParts());
    }

    public void ResetFOV(float timeToResetFOV)
    {
        if (isPlayer)
        {
            DOTween.To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, camStartFOV, timeToResetFOV);
        }
    }

    IEnumerator slowlyBreakParts()
    {
        if (!isBreaking)
        {
            isBreaking = true;
            yield return new WaitForSeconds(1.5f);

            for (int i = 0; i < shipParts.Length; i++)
            {
                shipParts[i].AddComponent<Rigidbody>();
                Rigidbody rb = shipParts[i].GetComponent<Rigidbody>();
                shipParts[i].transform.parent = null;

                rb.AddForce(-shipParts[i].transform.forward, ForceMode.Impulse);
                rb.AddForce(-shipParts[i].transform.right * 40f, ForceMode.Impulse);
                rb.AddForce(-shipParts[i].transform.up * 50f, ForceMode.Impulse);

                shipParts[i].GetComponent<RemoveOnStart>().enabled = true;

                vehicle.GetShipTransform().DOShakePosition(0.3f,0.12f,30,30,false);

                float t = Random.Range(0.7f, 1.8f);
                yield return new WaitForSeconds(t);
            }
        }
    }
}
