using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        rotateCamera();
    }

    private void rotateCamera()
    {
        transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"));
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"), Space.World); //space world prevents rotation in z axis
    }
}
