using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveOnStart : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.0f;

	void Start()
    {
        Destroy(this.gameObject,destroyDelay);
	}
}
