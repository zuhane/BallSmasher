using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienDissolve : MonoBehaviour
{

    private Material material;
    private float startDissolve = 1;

    private void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
    }

    public void Update()
    {
        startDissolve -= Time.deltaTime;
        material.SetFloat("DissolveLevel", startDissolve);

    }



}
