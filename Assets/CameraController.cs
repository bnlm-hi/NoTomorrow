using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 myPos;
    public Transform myPlay;

    public void Update()
    {
        transform.position = myPlay.position + myPos;

        if (myPlay != null)
        {
            transform.LookAt(myPlay);
        }
    }
}
