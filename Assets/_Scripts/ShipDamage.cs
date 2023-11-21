using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDamage : MonoBehaviour
{
    private Rigidbody rb;

    float timer;

	void Start()
    {
		rb = GetComponent<Rigidbody>();
	}
	

    void OnCollisionStay(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 20) //10
        {
            //Vector3 upwardForceFromCollision = Vector3.Dot(collision.impulse, transform.up) * transform.up;
            //rb.AddForce(-upwardForceFromCollision, ForceMode.Impulse);
        }

        if(collision.gameObject.CompareTag("Walls"))
        {
            if (gameObject.CompareTag("AI"))
            {
                timer += Time.deltaTime;

                if (timer >= 4f)
                {
                    transform.GetComponent<ShipAI>().recalculateNeareastWaypoint();
                    Debug.Log("New Waypoint");
                    timer = 0;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Walls"))
        {
            if (gameObject.CompareTag("Player") && rb.velocity.magnitude >= 30f)
            {
                CameraShaker.Instance.ShakeNow(2, 0.1f, true);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //if (collision.gameObject.CompareTag("Walls"))
        //{
            //timer = 0;
            //Debug.Log("Wall Exit");
        //}


    }
}
