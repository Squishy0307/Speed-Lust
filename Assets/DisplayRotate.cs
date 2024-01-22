using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRotate : MonoBehaviour
{
    public float rotationSpeed = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 1, 0) * rotationSpeed * Time.deltaTime;
    }
}
