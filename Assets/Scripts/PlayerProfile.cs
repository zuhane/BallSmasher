using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    [HideInInspector] public Color colour;

    private PlayerManager playerManager;

    private void Start()
    {

    }

    public void SetColour()
    {
        playerManager = GetComponent<PlayerManager>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        switch (playerManager.team)
        {
            case 1:
                spriteRenderer.color = Color.green;
                break;
            case 2:
                spriteRenderer.color = Color.cyan;
                break;
            //case 3:
            //    spriteRenderer.color = Color.magenta;
            //    break;
            //case 4:
            //    spriteRenderer.color = Color.blue;
            //    break;
        }

        colour = spriteRenderer.color;
    }

}
