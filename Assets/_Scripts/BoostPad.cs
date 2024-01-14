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
        if (other.TryGetComponent<VehicleMovement>(out VehicleMovement ship))
        {
            shipModel = ship.transform.GetChild(0).transform;

            other.gameObject.GetComponent<Rigidbody>().AddForce(shipModel.transform.forward *(speedIncrease + ship.GetMaxSpeed() * (0.01f * percentBoost)), ForceMode.VelocityChange);
            Debug.Log("Boosted");

            if (other.gameObject.GetComponent<VehicleMovement>().isPlayer)
                AudioManager.Instance.Play("boost", 1, 1);

            ship.GetComponent<ShipVisuals>().burst();
        }
    }
}
