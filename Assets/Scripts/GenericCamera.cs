using UnityEngine;

public class GenericCamera : MonoBehaviour
{
    [Header("Camera Movement Behavior")]
    public Transform[] targets;
    public Vector3 cameraOffset;

    [SerializeField] private float cameraSpeed;
    [SerializeField] private float resetMargin;

    private Vector3 _target;

    private void Update()
    {
        Follow();            
    }

    private void CameraMove()
    {
        if (Vector3.Distance(_target, transform.position) < resetMargin)
        {
            transform.position = _target;
            return;
        }
            transform.position = Vector3.Lerp(transform.position, _target, Time.smoothDeltaTime * cameraSpeed);
    }

    private void Follow()
    {
        _target = cameraOffset;

        for (var i = 0; i < targets.Length; i++) 
        { 
            _target += targets[i].position; 
        }

        _target /= targets.Length;

        CameraMove();
    }


}