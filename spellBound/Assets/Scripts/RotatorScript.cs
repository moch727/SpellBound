using UnityEngine;
using UnityEngine.UIElements;

public class RotatorScript : MonoBehaviour
{
    private float targetRotation;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation.y;
    }
    public void Turn()
    {
        targetRotation +=  30f;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, targetRotation, transform.rotation.z), 0.05f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Turn();
        }
    }
}
