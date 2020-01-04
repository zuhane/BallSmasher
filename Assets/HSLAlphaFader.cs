using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HSLAlphaFader : MonoBehaviour
{
    private SpriteRenderer alphaRenderer;
    [Range(-1, 0)] public float alpha;

    private void Start()
    {
        alphaRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Material material = alphaRenderer.material;
        material.SetVector("_HSLAAdjust", 
            new Vector4(material.GetVector("_HSLAAdjust").x,
            material.GetVector("_HSLAAdjust").y,
            material.GetVector("_HSLAAdjust").z, alpha));

    }
}
