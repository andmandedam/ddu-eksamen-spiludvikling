using System;
using UnityEngine;

public class GenericCamera : MonoBehaviour
{
    [Header("Camera Movement Behavior")]
    public Transform[] targets;
    public Vector3 cameraOffset;

    [SerializeField] private float cameraSpeed;
    [SerializeField] private float resetMargin;


    private void Update()
    {
        Follow();            
    }

    private void CameraMove(Vector3 target)
    {
        if (Vector3.Distance(target, transform.position) < resetMargin)
        {
            transform.position = target;
            return;
        }
        transform.position = Vector3.Lerp(transform.position, target, Time.smoothDeltaTime * cameraSpeed);
    }

    private void Follow()
    {
        Vector3 target = cameraOffset;

        for (var i = 0; i < targets.Length; i++) 
        { 
            target += targets[i].position; 
        }
        target.z = 0;
        target /= targets.Length;
        target.z = transform.position.z;

        CameraMove(target);
    }


}