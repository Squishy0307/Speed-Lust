using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPickup : MonoBehaviour
{
    private Collider col;
    private MeshRenderer rend;
    public float respawnAgainAfter = 2f;

    private void Start()
    {
        col = GetComponent<Collider>();
        rend = GetComponent<MeshRenderer>();
        transform.DOMoveY(transform.position.y + 1f, 0.5f).SetEase(Ease.OutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 6)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.obrCollected();
                Vibration_Manager.Instance.VibrateNow(0.3f, 0.3f, 0.5f);
                AudioManager.Instance.Play("collect", 1, 1);
            }

            other.gameObject.GetComponent<ShipVisuals>().playElectricParticles();

            StartCoroutine(disableMe());
        }
    }

    IEnumerator disableMe()
    {
        col.enabled = false;
        rend.enabled = false;

        yield return new WaitForSeconds(respawnAgainAfter);

        col.enabled = true;
        rend.enabled = true; 
    }
}
