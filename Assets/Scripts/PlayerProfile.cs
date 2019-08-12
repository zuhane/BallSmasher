using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile : MonoBehaviour
{

    [SerializeField] [Range(1, 4)] public int playerNumber = 1;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color colour;

    [SerializeField] private PlayerManager movement;
    

    // Start is called before the first frame update
    void Start()
    {
        movement.playerNumber = playerNumber;

        switch (playerNumber)
        {
            case 1:

                break;

            case 2:
                spriteRenderer.color = Color.green;
                break;
        }
    }

}
