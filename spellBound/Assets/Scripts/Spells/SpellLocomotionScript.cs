using UnityEngine;
using UnityEngine.AI;

public class SpellLocomotionScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody rb;

    private Vector3 initialDestination; //for if object needs to move somewhere before tracking i.e. going up and then falling down
    private Vector3 targetLocation;

    private float speed;
    [SerializeField] private bool trackEnemy = false;
    //[SerializeField] private GameObject target;
    private GameObject owner;

    private float maxRange;
    private float trackingRange;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (owner != null && Vector3.Distance(transform.position, owner.transform.position) > maxRange)
        {
            Destroy(gameObject);
        }
    }

    public void cast(GameObject owner, float maxRange)
    {
        this.maxRange = maxRange;
        transform.parent = null;
        rb.AddForce(Vector3.forward * 150);
        this.owner = owner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Destructable")
        {
            collision.gameObject.GetComponent<InteractableObject>().setOnFire();
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")
        {

        }
    }
}
