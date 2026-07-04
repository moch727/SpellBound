using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    [SerializeField] Transform player;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;
        //transform.position = new Vector3(player.position.x, player.position.y+1, player.position.z);
        rotateCamera();
    }

    private void rotateCamera()
    {
        transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"));
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"), Space.World); //space world prevents rotation in z axis
    }
}
