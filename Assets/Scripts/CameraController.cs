using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float cameraSpeed = 5.0f;
    public GameObject player;
    //public Vector3 offset;
    private float dist = 2;
    private float height = 0.2f;

    private void FixedUpdate()
    {
        //Vector3 cameraPos = player.transform.position + offset;
        //Vector3 lerpPos = Vector3.Lerp(transform.position, cameraPos, cameraSpeed);

        Vector3 cameraPos = player.transform.position - (player.transform.forward * dist) +
            (Vector3.up * height);
        Vector3 lerpPos = Vector3.Lerp(transform.position, cameraPos, cameraSpeed * Time.deltaTime);
        transform.position = lerpPos;
        transform.LookAt(player.transform);
        
    }
}
