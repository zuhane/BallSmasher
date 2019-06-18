using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashBrosCamera : MonoBehaviour
{
    private Transform[] playerPositions;

    private float minDistance = 5f;

    private float xMin, xMax, yMin, yMax;
    private float margin = 2.0f;
    private float zoomFactorX = 2f, zoomFactorY = 6f;

    //Interpolation stuff
    private float interpVelocity;
    private float catchupDistance;
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Bouncy");
        playerPositions = new Transform[playerObjects.Length + ballObjects.Length];
        for(int i = 0; i < playerObjects.Length; i++)
        {
            playerPositions[i] = playerObjects[i].transform;
        }

        for (int i = 0; i < ballObjects.Length; i++)
        {
            playerPositions[i + playerObjects.GetLength(0)] = ballObjects[i].transform;
        }
    }

    void LateUpdate()
    {
        if (playerPositions.Length == 0)
        {
            Debug.Log("No players found!");
            return;
        }

        xMin = xMax = playerPositions[0].position.x;
        yMin = yMax = playerPositions[0].position.y;

        for (int i = 1; i < playerPositions.Length; i++)
        {
            if (playerPositions[i].position.x < xMin)
                xMin = playerPositions[i].position.x;

            if (playerPositions[i].position.x > xMax)
                xMax = playerPositions[i].position.x;

            if (playerPositions[i].position.y < yMin)
                yMin = playerPositions[i].position.y;

            if (playerPositions[i].position.y > yMax)
                yMax = playerPositions[i].position.y;

        }

        float xMiddle = (xMin + xMax) / 2;
        float yMiddle = (yMin + yMax) / 2;
        float distanceX = (xMax - xMin) * zoomFactorX;
        float distanceY = (yMax - yMin) * zoomFactorY;

        float finalDistance = Mathf.Max(distanceX, distanceY);

        if (finalDistance < minDistance)
            finalDistance = minDistance;
            
        Vector3 target = new Vector3(xMiddle, yMiddle, -finalDistance);
        //Target now decided - now the code below will gradually interpolate to that target position

        Vector3 posNoZ = transform.position;
        //posNoZ.z = target.z;

        Vector3 targetDirection = (target - posNoZ);

        interpVelocity = targetDirection.magnitude * 20f;

        targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.15f);

    }
}
