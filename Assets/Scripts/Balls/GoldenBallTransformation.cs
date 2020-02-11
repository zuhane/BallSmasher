using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenBallTransformation : MonoBehaviour
{
    public Sprite bronzeBall;
    public Gradient bronzeGradient;
    public Sprite silverBall;
    public Gradient silverGradient;
    public Sprite goldenBall;
    public Gradient goldenGradient;

    private Ball ball;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;

    public int numElectrifications = 0;
    private bool waitForElectrification = false;
    // Start is called before the first frame update
    void Start()
    {
        ball = gameObject.GetComponent<Ball>();
        spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        trailRenderer = gameObject.GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!waitForElectrification)
        {
            if (ball.electrified)
            {
                numElectrifications += 1;
                transformBall();
                waitForElectrification = true;
            }
        }
        else
        {
            if (!ball.electrified)
            {
                waitForElectrification = false;
            }
        }
    }

    void transformBall()
    {
        switch (numElectrifications)
        {
            case 1:
                spriteRenderer.sprite = silverBall;
                gameObject.transform.localScale *= 0.75f;
                trailRenderer.startWidth *= 0.75f;
                trailRenderer.endWidth *= 0.75f;
                trailRenderer.colorGradient = silverGradient;
                ball.goalPoints = 2;
                break;
            case 2:
                spriteRenderer.sprite = goldenBall;
                gameObject.transform.localScale *= 0.75f;
                trailRenderer.startWidth *= 0.75f;
                trailRenderer.endWidth *= 0.75f;
                trailRenderer.colorGradient = goldenGradient;
                ball.goalPoints = 3;
                break;
            default:
                break;
        }
    }
}
