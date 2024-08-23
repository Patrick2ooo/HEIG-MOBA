using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    private const float CAM_SPEED = 0.1f;
    private static Vector3 Offset = new Vector3(0, 15, -8);

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + Offset, CAM_SPEED);
    }
}
