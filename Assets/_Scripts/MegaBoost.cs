using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBoost : MonoBehaviour
{
    [SerializeField] float boostDuration = 10f;
    [SerializeField] float speedIncreaseRate = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out VehicleMovement vehicle))
        {
            Debug.Log("<b><color=green> MEGA BOOST!!</color></b>");
            vehicle.MegaBoostInitiated(boostDuration,speedIncreaseRate);
        }
    }
}