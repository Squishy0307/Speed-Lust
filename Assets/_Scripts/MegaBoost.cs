using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MegaBoost : MonoBehaviour
{
    [SerializeField] float boostDuration = 10f;
    [SerializeField] float speedIncreaseRate = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ShipController>(out ShipController ship))
        {
            if (ship.lapNumber == "Lap 3")
            {
                ship.MegaBoostInitiated(boostDuration, speedIncreaseRate);
            }
        }
    }
}