using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Cinemachine;

public class Fader : MonoBehaviour
{
    public static Fader Instance;

    private Image fadeImg;
    private float respawnSequenceBreak;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        fadeImg = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F9)) //-----------[Remove Later]-----------
        {
            RespawnFade();
        }
    }


    public void FadeOut()
    {
        fadeImg.DOFade(1, 0.5f);
    }

    public void FadeIn()
    {
        fadeImg.DOFade(0, 0.5f);
    }

    public void RespawnFade()
    {
        respawnSequenceBreak = 0;

        Sequence respawner = DOTween.Sequence();
        respawner.Append(fadeImg.DOFade(1, 0.5f));
        respawner.Append(DOTween.To(() => respawnSequenceBreak, x => respawnSequenceBreak = x, 1, 0.75f));
        respawner.Append(fadeImg.DOFade(0, 0.5f));
    }

}
