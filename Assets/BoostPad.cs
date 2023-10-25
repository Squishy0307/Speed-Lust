using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float speedIncrease = 10;
    Transform shipModel;
    [Range(0,100)]
    public float percentBoost;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ShipController>(out ShipController ship))
        {
            
            
            foreach (Transform child in ship.GetComponentsInChildren<Transform>())
            {
                if (child.gameObject.name == "ShipModel")
                {
                    shipModel = child;
                }
            }

            other.gameObject.GetComponent<Rigidbody>().AddForce(shipModel.transform.forward *(speedIncrease + ship.GetMaxSpeed() * (0.01f * percentBoost)), ForceMode.VelocityChange);
            Debug.Log("Boosted");
        }
    }
}
