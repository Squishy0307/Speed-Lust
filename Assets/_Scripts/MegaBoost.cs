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
    private bool testBoost = true;

    private void Start()
    {
        orangeRingMat.SetFloat("_Speed", 0);
        outerRingMat.SetFloat("_Speed", 0);
        innerRingMat.SetFloat("_Speed", 0);

        portalMat.SetFloat("_InnerFade", 1f);

        testBoost = true;
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.F10) && Input.GetKey(KeyCode.F11))
        {
            orangeRingMat.SetFloat("_Speed", 1);
            outerRingMat.SetFloat("_Speed", 0.5f);
            innerRingMat.SetFloat("_Speed", 0.5f);

            portalMat.SetFloat("_InnerFade", 0.67f);

            testBoost = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CheckpointTracker checkpoint))
        {

            if (checkpoint.finishLinePass == 1 && checkpoint.CompareTag("Player"))
            {
                StartCoroutine(enableEffects());

                MusicController.instance.ChangeVolume(0, 0);
                MusicController.instance.ChangeVolume(1, 0.8f);
            }

            else if (checkpoint.finishLinePass == 2)
            {
                Debug.Log("<b><color=green> MEGA BOOST!!</color></b>");

                //TO-DO: HIDE ALL UI 

                //VehicleMovement ship = other.GetComponent<VehicleMovement>();
                //ship.MegaBoostInitiated(boostDuration, speedIncreaseRate);

                //AudioManager.Instance.Play("megaboost", 1, 1);


                VehicleMovement ship = other.GetComponent<VehicleMovement>();
                ship.MegaBoostInitiated(boostDuration, speedIncreaseRate);

                if (other.gameObject.CompareTag("Player"))
                {
                    AudioManager.Instance.Play("megaboost", 1, 1);
                    //MusicController.instance.ChangeVolume(0, 0);
                    //MusicController.instance.ChangeVolume(1, 0.8f);
                }


            }
        }

        //if (other.gameObject.CompareTag("Player"))
        //{
        //    if (testBoost)
        //    {
        //        VehicleMovement ship = other.GetComponent<VehicleMovement>();
        //        ship.MegaBoostInitiated(boostDuration, speedIncreaseRate);
        //        AudioManager.Instance.Play("megaboost", 1, 1);
        //        MusicController.instance.ChangeVolume(0, 0);
        //        MusicController.instance.ChangeVolume(1, 0.8f);
        //    }
        //}
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