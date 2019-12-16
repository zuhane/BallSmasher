using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SmashBrosCamera : MonoBehaviour
{
    private List<Transform> targets;

    public Vector3 offset;

    public float smoothTime = .5f;
    private Vector3 velocity;

    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float zoomLimiter = 50f;

    public float orthoFactor = 0.5f;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();

        SetTargets();
    }

    public void SetTargets()
    {
        targets = new List<Transform>();

        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        GameObject[] ballObjects = GameObject.FindGameObjectsWithTag("Bouncy");

        if (playerObjects.GetLength(0) > 0)
        for (int i = 0; i < playerObjects.Length; i++)
        {
            targets.Add(playerObjects[i].transform);
        }

        if (ballObjects.GetLength(0) > 0)
            for (int i = 0; i < ballObjects.Length; i++)
        {
            targets.Add(ballObjects[i].transform);
        }
    }

    private void LateUpdate()
    {
        if (targets.Count == 0) return;

        Move();
        Zoom();
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    private float GetGreatestDistance()
    {
        Bounds bounds = new Bounds();
        if (targets[0] != null)
        {
            bounds = new Bounds(targets[0].position, Vector3.zero);
        }
            

        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
                bounds.Encapsulate(targets[i].position);
        }

        return Mathf.Max(bounds.size.x, bounds.size.y);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        Bounds bounds = new Bounds();

        if (targets[0] != null)
        {
            bounds = new Bounds(targets[0].position, Vector3.zero);
        }


        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i] != null)
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

}
