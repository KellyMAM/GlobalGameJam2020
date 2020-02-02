using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomToVelocity : MonoBehaviour
{
    public Rigidbody rb;
    public CinemachineVirtualCamera vcam;
    public float staticZoom;
    public float maxZoom;
    public float maxZoomSpeed;

    private void Update()
    {
        float zoom = Mathf.SmoothStep(staticZoom, maxZoom, rb.velocity.magnitude / maxZoomSpeed);
        vcam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, zoom, 0);
    }
}
