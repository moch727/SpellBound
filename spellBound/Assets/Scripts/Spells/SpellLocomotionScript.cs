using UnityEngine;
using UnityEngine.AI;

public class SpellLocomotionScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody rb;

    private Vector3 initialDestination; //for if object needs to move somewhere before tracking i.e. going up and then falling down
    private Vector3 targetLocation;

    [SerializeField] private float velocity;
    [SerializeField] private GameObject target;
    [SerializeField] private float maxRange;
    [SerializeField] private bool trackEnemy = false;

    //[SerializeField] private GameObject target;
    private GameObject owner;
    private bool startTracking = false;

    private bool enableC = false;
    private Vector3 centripetalForce;
    void Start()
    {
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
        //Vector3 enemyDirection = new Vector3(target.transform.position.x - this.transform.position.x, 0, target.transform.position.z - this.transform.position.z);
        //Debug.Log(transform.forward);
        //if (Mathf.Abs(Vector3.Cross(enemyDirection, transform.forward).magnitude) < 0.1f)
        //{
        //    Debug.Log("a");
        //}
        if (startTracking)
        {
            //rb.linearVelocity = Vector3.zero;
            followEnemy();

        }
        else
        {
            circularMotion();
        }
        //circularMotion();

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
        //target = owner;

        if (!enableC)
        {
            rb.AddForce(velocity * Vector3.forward, ForceMode.VelocityChange); //add an initial velocity
            enableC = true;

        } else { //cause circular motion, turns in direction its facing

            if(Vector3.Angle(target.transform.position - gameObject.transform.position, target.transform.position - owner.transform.position) < 5f)
            {
                startTracking = true;
            }
            else
            {
                Vector3 directionVector = new Vector3(owner.transform.position.x - this.transform.position.x, 0, owner.transform.position.z - this.transform.position.z);
                centripetalForce = (velocity * velocity / directionVector.magnitude) * directionVector.normalized;
                rb.AddForce(centripetalForce);
            }
        }
    }


    private void followEnemy()
    {
        if (trackEnemy)
        {

            if(maxRange > Vector3.Distance(transform.position,owner.transform.position))
            {
                transform.LookAt(target.transform.position);
                rb.AddRelativeForce(transform.forward * Mathf.Abs(velocity));
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "Destructable")
        {
            collision.gameObject.GetComponent<InteractableObject>().setOnFire();
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
