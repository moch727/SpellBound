using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{

    //}

    public float amplitude = 0.5f; //max height from current position
    private float frequency = Mathf.PI / 2; //how fast it cycles
    private float timeTracker = 0;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        float displacement = calculateYMovement() * Time.deltaTime;
        transform.Translate(0, displacement * amplitude, 0, Space.World);
    }

    float calculateYMovement()
    {

        timeTracker += frequency * Time.deltaTime;
        float yMovement = Mathf.Sin(timeTracker);

        return yMovement;
    }
}
