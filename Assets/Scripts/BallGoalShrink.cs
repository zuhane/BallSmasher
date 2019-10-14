using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallGoalShrink : MonoBehaviour
{
    private bool startShrinking = false;
    private Vector3 originalPos;
    private float shrinkRate = 0.04f;
    private int team;
    private Vector3 goalCentre;

    public void ShrinkScoredBall(int team, Vector3 goalCentre)
    {
        this.team = team;
        this.goalCentre = goalCentre;
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<CircleCollider2D>());
        startShrinking = true;
        originalPos = transform.position;
    }

    private void Update()
    {
        if (startShrinking)
        {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, goalCentre.x, 0.05f),
            Mathf.Lerp(transform.position.y, goalCentre.y, 0.05f), 0);
            //transform.position = originalPos + new Vector3(Random.Range(-0.1f, 0.1f),
            //    Random.Range(-0.1f, 0.1f), 0);

            transform.localScale = transform.localScale - new Vector3(shrinkRate, shrinkRate, 0);

            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
                Instantiate(Resources.Load<GameObject>("Effects/GoalScore"), transform.position, Quaternion.identity);
                GameObject.Find("GoalManager").GetComponent<GoalManager>().ScoreGoal(team);
            }
        }
    }

}
