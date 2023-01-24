using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float zoomSpeed = 10f;
    Camera _camera;
    Vector3 originalLocation;

    void Start()
    {
        _camera = Camera.main;
        originalLocation = _camera.transform.position;
    }

    void Update()
    {
        Zoom();
    }

    void Zoom()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, 1000);
        Vector3 zoomTowards = ray.GetPoint(5);

        float step = zoomSpeed * Time.deltaTime;

        //Change camera size
        if (transform.position != originalLocation || transform.position == originalLocation && Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            _camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * step;
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(zoomTowards.x, zoomTowards.y, originalLocation.z), Input.GetAxis("Mouse ScrollWheel") * step);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, originalLocation, Input.GetAxis("Mouse ScrollWheel") * step);
            }
        }
    }
}
