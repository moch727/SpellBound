using UnityEngine;
using UnityEngine.AI;

public class SpellLocomotionScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody rb;

    private Vector3 initialDestination; //for if object needs to move somewhere before tracking i.e. going up and then falling down
    private Vector3 targetLocation;

    private float velocity = 5f;
    [SerializeField] private bool trackEnemy = false;
    //[SerializeField] private GameObject target;
    private GameObject owner;
    private GameObject target;

    private float maxRange;
    private float trackingRange;
    private bool enableC = false;
    private Vector3 centripetalForce;
    void Start()
    {
        transform.Rotate(new Vector3(0, 180, 0));
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (owner != null && Vector3.Distance(transform.position, owner.transform.position) > maxRange)
        //{
        //    owner.GetComponent<PlayerCombatComponent>().setSpellActive(false);
        //    Destroy(gameObject);
        //}
    }

    private void FixedUpdate() //useful for physics calculations
    {

    }

    public void cast(GameObject owner, float maxRange)
    {
        this.maxRange = maxRange;
        transform.parent = null;
        //rb.AddForce(Vector3.forward * 150);
        this.owner = owner;

        circularMotion();

    }

    private void circularMotion()
    {
        //initially fly towards throw direction
        //then track towards the enemy
        target = owner;

        if (!enableC)
        {
            rb.AddRelativeForce(velocity * Vector3.forward, ForceMode.VelocityChange); //add an initial velocity
            enableC = true;

        } else { //cause circular motion, turns in direction its facing

            Vector3 directionVector = new Vector3(target.transform.position.x - this.transform.position.x, 0, target.transform.position.z - this.transform.position.z);
            centripetalForce = (velocity * velocity / directionVector.magnitude) * directionVector.normalized;
            rb.AddForce(centripetalForce);
        }


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
