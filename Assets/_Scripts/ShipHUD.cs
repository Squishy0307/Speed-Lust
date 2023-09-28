using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipHUD : MonoBehaviour
{
    [SerializeField] private Canvas HUD;

    private float speed;
    private Text speedText;

	void Start()
    {
        if (!HUD) return;

        foreach (Transform child in HUD.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name == "Speed")
            {
                speedText = child.GetComponent<Text>();
            }
        }
    }
	
	void Update()
    {
        UpdateHUD();
	}

    void UpdateHUD()
    {
        if (!HUD) return;

        // default 3.6f
        float currentSpd = speed * 30f;
        if (currentSpd > 0.5f)
        {
            speedText.text = currentSpd.ToString("F0") + " KPH";
        }
        else
        {
            speedText.text =  "0 KPH";
        }
    }

    public void UpdateSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
