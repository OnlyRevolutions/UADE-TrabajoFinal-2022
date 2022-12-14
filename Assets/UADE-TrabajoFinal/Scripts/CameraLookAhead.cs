using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraLookAhead : MonoBehaviour
{
    [SerializeField] float lookRange = 15f;
    Camera cam;
    CinemachineVirtualCamera _virtualCamera;
    void Start()
    {
        cam = Camera.main;
        _virtualCamera = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // Limitamos la distancia hasta la que se puede alejar la camara
            if (Vector3.Distance(transform.position, _virtualCamera.Follow.position) > lookRange) return;

            // Limitamos la magnitud de la direccion hacia la que se mueve la camara
            Vector3 dir = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            if(dir.magnitude > 1) dir.Normalize();

            _virtualCamera.Follow.position += dir;
        }
    }
}
