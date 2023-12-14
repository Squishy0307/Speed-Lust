using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turbrotate : MonoBehaviour

{
    public float rotationSpeed = 30f;

    void Update()
    {
      
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}

