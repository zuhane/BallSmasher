using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    //[HideInInspector] public Color colour;

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        SpriteRenderer ballRenderer = transform.Find("AttackOrb").GetComponentInChildren<SpriteRenderer>();

        Material instancedMaterial = spriteRenderer.material;
        Material instancedMaterial2 = ballRenderer.material;

        switch (playerManager.team)
        {
            case 1:
                //instancedMaterial.SetVector("_HSLAAdjust", new Vector4(0f, 0, 0, 0));
                //instancedMaterial2.SetVector("_HSLAAdjust", new Vector4(0f, 0, 0, 0));
                break;
            case 2:
                instancedMaterial.SetVector("_HSLAAdjust", new Vector4(0.3f, 0, 0, 0));
                instancedMaterial2.SetVector("_HSLAAdjust", new Vector4(0.3f, 0, 0, 0));
                break;
                //case 3:
                //    spriteRenderer.color = Color.magenta;
                //    break;
                //case 4:
                //    spriteRenderer.color = Color.blue;
                //    break;
        }

        //colour = spriteRenderer.color;
    }

    public void SetColour()
    {

    }

}
