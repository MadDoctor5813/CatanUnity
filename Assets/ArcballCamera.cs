using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcballCamera : MonoBehaviour
{

    public Transform target;

    public float ScrollSensitivity;
    public float RotateSensitivity;

    public Vector3 DefaultPosition;

    private Camera arcballCamera;

    private float zoom;
    private float angleX;
    private float angleY;

	void Start ()
	{
        arcballCamera = GetComponent<Camera>();
        arcballCamera.transform.LookAt(target.transform.position);
	}
	
	void Update () 
	{
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        zoom += scroll * ScrollSensitivity * Time.deltaTime;
        if (Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            angleX += mouseX * RotateSensitivity * Time.deltaTime;
            angleY -= mouseY * RotateSensitivity * Time.deltaTime;
        }
        angleY = Mathf.Clamp(angleY, 5, 80);
        angleX = angleX % 360;
        TransformCamera();
	}

    private void TransformCamera()
    {
        transform.position = DefaultPosition;
        transform.rotation = Quaternion.identity;
        transform.RotateAround(target.position, Vector3.right, angleY);
        transform.RotateAround(target.position, Vector3.up, angleX);
        transform.position = Vector3.MoveTowards(transform.position, target.position, zoom);
    }

}
