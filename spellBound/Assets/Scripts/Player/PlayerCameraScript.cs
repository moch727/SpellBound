using UnityEngine;

public class PlayerCameraScript : MonoBehaviour
{
    [SerializeField] Transform player;

    private float xRotation;
    private float yRotation;
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
        //transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"), Space.World); //space world prevents rotation in z axis
        //transform.Rotate(Vector3.right, -Input.GetAxis("Mouse Y"));
        //For third person, smoother

        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * 200;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * 200;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
