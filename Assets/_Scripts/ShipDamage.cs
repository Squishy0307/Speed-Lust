﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDamage : MonoBehaviour
{
    //[SerializeField] private GameObject sparks, smallSparks;
    private Rigidbody rb;

	void Start()
    {
		rb = GetComponent<Rigidbody>();
	}
	

    //void OnCollisionEnter(Collision collision)
    //{
    //    foreach (ContactPoint contact in collision.contacts)
    //    {
    //        GameObject newSparks = Instantiate(sparks, transform);
    //        newSparks.transform.position = contact.point;
    //        newSparks.transform.LookAt(transform);
    //        //newSparks.transform.rotation = Quaternion.Euler(contact.normal);
    //        newSparks.GetComponent<ParticleSystem>().startSpeed = 50.0f + collision.relativeVelocity.sqrMagnitude * 0.01f;


    //    }
    //}

    void OnCollisionStay(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 10)
        {
            //foreach (ContactPoint contact in collision.contacts)
            //{
            //    GameObject newSparks = Instantiate(smallSparks, transform);
            //    newSparks.transform.position = contact.point;
            //    newSparks.transform.LookAt(transform);
            //    //newSparks.transform.rotation = Quaternion.Euler(contact.normal);
            //}

            //if (collision.gameObject.CompareTag("Walls"))
            //{
            //    Vector3 upwardForceFromCollision = Vector3.Dot(collision.impulse, transform.up) * transform.up;
            //    rb.AddForce(-upwardForceFromCollision, ForceMode.Impulse);
            //}
        }
    }
}
