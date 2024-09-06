using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform target;
    private readonly Vector3 _offset = new Vector3(0, 10, -6);

    // Update is called once per frame and changes the camera position to follow the player
    void Update()
    {
        transform.position = target.position + _offset;
    }
}
