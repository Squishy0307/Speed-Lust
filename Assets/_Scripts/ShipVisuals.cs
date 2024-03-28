using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
    private ShipComponents shipComponents;

    public int shipID;

    [Header("VFX")]
    [SerializeField] GameObject sparks;
    [SerializeField] Material spdBlur;
    [SerializeField] Material spdDistortion;
    [SerializeField] Material spdLines;
    [SerializeField] MeshRenderer wind;
    [SerializeField] ParticleSystem boostSonicBurst;
    [SerializeField] ParticleSystem electric;

    public Ease burstCurve;

    private Material ThrustersMat1;
    private Material ThrustersMat2;
    private Material windMat;

    float burstValue = 1;
    float enginePower = 0;
    float screenEffectIntensity = 0;
    float windEffect = 0;

    private void Awake()
    {
        cam = Camera.main;
        camStartFOV = cam.fieldOfView;

        isPlayer = gameObject.CompareTag("Player");
        vehicle = GetComponent<VehicleMovement>();

        setupShip();

        shipComponents = transform.GetChild(shipID).GetComponent<ShipComponents>();

        ThrustersMat1 =  shipComponents.Thrusters1.material;
        ThrustersMat2 =  shipComponents.Thrusters2.material;
        windMat = wind.material;

    }

    private void Update()
    {

        if (vehicle.GetCurrentSpeed() > 160)
        {
            windEffect = Mathf.Lerp(windEffect, 1, Time.deltaTime);
            windMat.SetFloat("_Speed", windEffect);
        }
        else
        {
            windEffect = Mathf.Lerp(windEffect, 0, Time.deltaTime);
            windMat.SetFloat("_Speed", windEffect);
        }

        if (!isPlayer) return;

        if(vehicle.GetCurrentSpeed() > 20) 
        {
            speedParticles.Play();

            screenEffectIntensity = Mathf.Lerp(screenEffectIntensity, 1, Time.deltaTime);

            spdBlur.SetFloat("_Blur_Intensity", screenEffectIntensity);
            spdDistortion.SetFloat("_Mask_Intensity", screenEffectIntensity);
            spdLines.SetFloat("_Effect_Intensity", screenEffectIntensity);
        }
        else
        {
            speedParticles.Stop();

            screenEffectIntensity = Mathf.Lerp(screenEffectIntensity, 0, Time.deltaTime);

            spdBlur.SetFloat("_Blur_Intensity", screenEffectIntensity);
            spdDistortion.SetFloat("_Mask_Intensity", screenEffectIntensity);
            spdLines.SetFloat("_Effect_Intensity", screenEffectIntensity);
        }

            //MAKE IT WORK FOR AI ASS WELL!
        if (ThrustersMat1 != null)
        {
            if (vehicle.GetForwardInput() >= 1)
            {
                enginePower = Mathf.Lerp(enginePower, 1, Time.deltaTime * 2);
            }
            else
            {
                enginePower = Mathf.Lerp(enginePower, 0, Time.deltaTime * 5);
            }

            ThrustersMat1.SetFloat("_Burst", burstValue);
            ThrustersMat2.SetFloat("_Burst", burstValue);

            ThrustersMat1.SetFloat("_Engine_Power", enginePower);
            ThrustersMat2.SetFloat("_Engine_Power", enginePower);

        }
    }

    public void playElectricParticles()
    {
        electric.Play();
    }

    public void BoostFOVChange()
    {
        if (isPlayer)
        {
            StartCoroutine(FovChange());
        }
        boostSonicBurst.Play(); 
    }

    IEnumerator FovChange()
    {
        IncreaseFOV(110, 0.3f);
        spdLines.SetFloat("_Line_Amount", 1.7f);
        yield return new WaitForSeconds(0.55f);
        spdLines.SetFloat("_Line_Amount", 3.7f);
        ResetFOV(1.5f);
    }

    public void burst()
    {   
        DOTween.To(() => burstValue, x => burstValue = x, -1, 0.3f).SetEase(burstCurve).OnComplete(resetBurst);
        ThrustersMat1.SetFloat("_Burst_Amplitude", 215);
        ThrustersMat2.SetFloat("_Burst_Amplitude", 215);
    }
    public void megaBoostBurst()
    {
        ThrustersMat1.SetFloat("_Burst_Amplitude", 815);
        ThrustersMat2.SetFloat("_Burst_Amplitude", 815);

        boostSonicBurst.Play();
    }

    public void resetBurst()
    {
        burstValue = 1;
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

                Destroy(shipParts[i],3);
                //shipParts[i].GetComponent<RemoveOnStart>().enabled = true;

                vehicle.GetShipTransform().DOShakePosition(0.3f,0.12f,30,30,false);

                float t = Random.Range(0.7f, 1.8f);
                yield return new WaitForSeconds(t);
            }
        }
    }

    void setupShip()
    {
        if (isPlayer)
        {
            shipID = ShipSelector.Instance.GetSelectedShip();

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name != "Boost_Burst") 
                {
                    if (i == shipID)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }
                    else
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }

                if (transform.GetChild(i).name == "Electric")
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        //else
        //{
        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        if (transform.GetChild(i).gameObject.activeSelf)
        //        {
        //            shipID = i;
        //        }
        //    }
        //}

        ShipComponents comp = transform.GetChild(shipID).GetComponent<ShipComponents>();

        shipModel = comp.ShipBody.transform;
        vehicle.shipBody = shipModel;

        shipParts = new GameObject[comp.shipParts.Length];

        for (int i = 0; i < shipParts.Length; i++)
        {
            shipParts[i] = comp.shipParts[i];
        }

    }

    public void SpawnSparks(Vector3 pos, Vector3 rot)
    {
        GameObject s = Instantiate(sparks, pos, Quaternion.Euler(rot));
        s.transform.parent = transform;
        Destroy(s, 1f);
    }

    private void OnDisable()
    {
        if (spdBlur != null)
        {
            spdBlur.SetFloat("_Blur_Intensity", 0);
            spdDistortion.SetFloat("_Mask_Intensity", 0);
            spdLines.SetFloat("_Effect_Intensity", 0);
            spdLines.SetFloat("_Line_Amount", 3.7f);
        }
    }
}

