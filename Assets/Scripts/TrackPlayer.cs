using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class TrackPlayer : MonoBehaviour
{
    Camera currCamera;
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;
    public float wheelScroll;
    public float defFieldOfView = 60, minFieldOfView = 0, maxFieldOfView = 120;
    // Use this for initialization
    void Start()
    {
        targetPos = transform.position;
        currCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        
        wheelScroll = Input.GetAxis("Mouse ScrollWheel");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 30f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

            currCamera.fieldOfView -= wheelScroll;

            if (currCamera.fieldOfView < minFieldOfView)
                currCamera.fieldOfView = minFieldOfView;
            if (currCamera.fieldOfView > maxFieldOfView)
                currCamera.fieldOfView = maxFieldOfView;
        }
    }
}
