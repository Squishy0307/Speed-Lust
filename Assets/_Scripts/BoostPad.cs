using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BoostPad : MonoBehaviour
{
    public float speedIncrease = 10;
    Transform shipModel;
    [Range(0,100)]
    public float percentBoost;
    public GameObject BoostTxtPrefab;
    public Transform TextSpawnPoint;
    private TextMeshProUGUI boostAmtTxt;
    public Transform hoverObj;

    private int p;

    private float startBoostSpd;

    void Start()
    {
        boostAmtTxt = Instantiate(BoostTxtPrefab, TextSpawnPoint.position, hoverObj.rotation, GameManager.Instance.worldUI.transform).GetComponent<TextMeshProUGUI>();
        hoverObj.transform.DOLocalMoveY(hoverObj.transform.localPosition.y + 2f, 0.5f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
        startBoostSpd = speedIncrease;
    }


    void Update()
    {
        speedIncrease = startBoostSpd + GameManager.Instance.currentOrbAmount() * 2;
        boostAmtTxt.text = GameManager.Instance.currentOrbAmountPercentage().ToString("0") + "%";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<VehicleMovement>(out VehicleMovement ship))
        {
            shipModel = ship.transform.GetChild(0).transform;

            other.gameObject.GetComponent<Rigidbody>().AddForce(shipModel.transform.forward *(speedIncrease + ship.GetMaxSpeed() * (0.01f * percentBoost)), ForceMode.VelocityChange);
            Debug.Log("Boosted");

            if (other.gameObject.GetComponent<VehicleMovement>().isPlayer)
            {
                AudioManager.Instance.Play("boost", 1, 1);
                Vibration_Manager.Instance.VibrateNow(0.3f, 0.3f, 0.5f);
                GameManager.Instance.resetOrbs();
            }

            ship.GetComponent<ShipVisuals>().burst();
            ship.GetComponent<ShipVisuals>().BoostFOVChange();

            
        }
    }
}
