using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBoost : MonoBehaviour
{
    [SerializeField] float boostDuration = 10f;
    [SerializeField] float speedIncreaseRate = 1f;
    [SerializeField] private Material portalMat;
    [SerializeField] private Material innerRingMat;
    [SerializeField] private Material outerRingMat;
    [SerializeField] private Material orangeRingMat;

    private bool EffectsEnabled = false;

    private void Start()
    {
        orangeRingMat.SetFloat("_Speed", 0);
        outerRingMat.SetFloat("_Speed", 0);
        innerRingMat.SetFloat("_Speed", 0);

        portalMat.SetFloat("_InnerFade", 1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out VehicleMovement vehicle))
        {

            if (vehicle.lapNumber == "Lap 2" && vehicle.CompareTag("Player"))
            {
                StartCoroutine(enableEffects());
            }

            else if (vehicle.lapNumber == "Lap 3")
            {
                Debug.Log("<b><color=green> MEGA BOOST!!</color></b>");

                //TO-DO: HIDE ALL UI 

                vehicle.MegaBoostInitiated(boostDuration, speedIncreaseRate);
            }
        }
    }

    IEnumerator enableEffects()
    {
        if(!EffectsEnabled)
        {
            EffectsEnabled = true;

            yield return new WaitForSeconds(1f);

            orangeRingMat.SetFloat("_Speed", 1);
            outerRingMat.SetFloat("_Speed", 0.5f);
            innerRingMat.SetFloat("_Speed", 0.5f);

            portalMat.SetFloat("_InnerFade", 0.67f);
        }
    }
}