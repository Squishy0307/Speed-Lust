using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopRotate : MonoBehaviour
{
    public float speed;

    public Vector3 RotationDirection;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(RotationDirection, 1 * speed * Time.deltaTime);
    }
}
