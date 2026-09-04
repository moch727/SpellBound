using UnityEngine;

public class SwitchScript : MonoBehaviour
{
    [SerializeField] GameObject button;
    [SerializeField] GameObject back;

    [SerializeField] Material initialMaterial;
    [SerializeField] Material pressMaterial;

    private Vector3 initialPosition;
    private Vector3 pressPosition;
    private bool resetPosition;

    private DetectingLamp detectingLamp;
    private MovingFloorSwitch floorSwitch;
    void Start()
    {
        initialMaterial = button.transform.GetChild(0).GetComponent<MeshRenderer>().material;
        initialPosition = button.transform.position;
        pressPosition = back.transform.position + transform.up *  0.02f;
        detectingLamp = GetComponent<DetectingLamp>();
        floorSwitch = GetComponent<MovingFloorSwitch>();
    }

    // Update is called once per frame
    void Update()
    {
        if(detectingLamp != null && detectingLamp.triggered)
        {
            foreach (Transform child in button.transform)
            {
                child.GetComponent<MeshRenderer>().material = pressMaterial;
            }
            button.transform.position = Vector3.MoveTowards(button.transform.position, pressPosition, 0.1f * Time.deltaTime);
        }
        else if (floorSwitch != null)
        {
            if (floorSwitch.startMoving)
            {
                foreach (Transform child in button.transform)
                {
                    child.GetComponent<MeshRenderer>().material = pressMaterial;
                }
                button.transform.position = Vector3.MoveTowards(button.transform.position, pressPosition, 0.1f * Time.deltaTime);
            }
            else
            {
                foreach (Transform child in button.transform)
                {
                    child.GetComponent<MeshRenderer>().material = initialMaterial;
                }
                button.transform.position = Vector3.MoveTowards(button.transform.position, initialPosition, 0.1f * Time.deltaTime);
            }


        }
    }
}
