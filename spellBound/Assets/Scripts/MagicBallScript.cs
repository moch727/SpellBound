using Unity.VisualScripting;
using UnityEngine;

public class MagicBallScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 ownerPosition;
    private Rigidbody rb;
    private float force = 150;
    private float maxRange = Mathf.Infinity;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ownerPosition = GameObject.Find("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, ownerPosition) > maxRange)
        {
            Destroy(gameObject);
        }

    }

    public void fire(float maxRange)
    {
        this.maxRange = maxRange;
        transform.parent = null;
        rb.AddForce(Vector3.forward * force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Destructable")
        {
            collision.gameObject.GetComponent<InteractableObject>().setOnFire();
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "Enemy")
        {

        }
    }
}
