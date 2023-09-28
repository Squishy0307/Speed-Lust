using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Track Settings")]
    [SerializeField] private float gravityScalar = 19.8f;

  

    void Start()
    {

    }

    void Update()
    {
       
    }

    public float GetGravity()
    {
        return gravityScalar;
    }

}
