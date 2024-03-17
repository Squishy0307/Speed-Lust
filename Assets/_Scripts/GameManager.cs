using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool RaceStarted = false;

    public GameObject worldUI;

    private int boostOrbs = 1;
    public int MaxOrbsCanBeCollected = 5;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    public float GetGravity()
    {
        return 90;//gravityScalar;
    }

    public void resetOrbs()
    {
        boostOrbs = 1;
    }

    public void obrCollected()
    {
        boostOrbs++;

        if(boostOrbs >= MaxOrbsCanBeCollected)
        {
            boostOrbs = MaxOrbsCanBeCollected;
        }
    }
    public int currentOrbAmount()
    {
        return boostOrbs;
    }

    public int currentOrbAmountPercentage()
    {
        int p = (boostOrbs * 100) / MaxOrbsCanBeCollected;

        return p;
    }

}
