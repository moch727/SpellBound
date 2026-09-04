using UnityEngine;
using UnityEngine.AI;

public class SpellLocomotionScript : MonoBehaviour
{
    private Rigidbody rb;
    private SpellScript spellScript;

    private Vector3 initialDestination; //for if object needs to move somewhere before tracking i.e. going up and then falling down


    [SerializeField] GameObject target;

    public float velocity;
    public float maxRange;
    [SerializeField] bool trackEnemy = false;
    [SerializeField] bool useGravity = false;
    [SerializeField] float steer;
    private ParticleSystem particle;
    private ParticleSystem.MainModule main;
    public string pivot;

    [HideInInspector]
    public GameObject owner = null;
    public bool complete;

    void Awake()
    { 
        rb = GetComponent<Rigidbody>();
        spellScript = GetComponent<SpellScript>();
        rb.useGravity = useGravity;
        particle = GetComponent<ParticleSystem>();
        if(particle != null)
        {
            main = particle.main;

            particle.Stop();
            main.duration = maxRange / velocity;
            main.startLifetime = maxRange / velocity;
            if(particle.GetComponentInChildren<ParticleSystem>() != null)
            {
                ParticleSystem child = particle.GetComponentInChildren<ParticleSystem>();
                ParticleSystem.MainModule childMain = child.main;

                childMain.duration = maxRange / velocity;
                childMain.startLifetime = maxRange / velocity;
            }
            particle.Play();
        }

        //main.emitterVelocity = Vector3.zero;
        //main.startSpeed = 0f;
        //1.53, 0, 0.4 position
    }

    private void FixedUpdate() //useful for physics calculations
    {
        if (trackEnemy && target != null)
        {

            Quaternion direction = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, steer);
            rb.AddForce(transform.forward * Time.deltaTime * velocity / Mathf.Sqrt(2), ForceMode.Impulse);

        }

        if (owner != null && maxRange < Vector3.Distance(transform.position, owner.transform.position))
        {
            complete = true;
            switch (spellScript.spellType)
            {

                case SpellScript.SpellType.Construct:
                    rb.linearVelocity = Vector3.zero;
                    if(GetComponent<DiscoConstruct>() != null) GetComponent<DiscoConstruct>().enabled = true;
                    break;

                default:
                    Destroy(gameObject);
                    break;
            }
        }

    }

    public void startMotion(GameObject owner)
    {
        this.owner = owner;
        if (owner.CompareTag("Player")) this.owner = owner.GetComponent<PlayerScript>().cam;

        //main.startSpeed = velocity;

        switch (spellScript.spellType) {

            case SpellScript.SpellType.Construct:
                Vector3 direction = Quaternion.Euler(-10, 0, 0) * Vector3.forward;
                rb.AddForce(direction * velocity, ForceMode.Impulse);
                break;

            default:
                rb.AddForce(this.owner.transform.forward * velocity, ForceMode.Impulse); //Apply initial speed
                break;
        }

        transform.parent = null;
        this.owner = owner;
        //main.emitterVelocity = this.owner.transform.forward * velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (trackEnemy)
            {
                rb.linearVelocity = Vector3.zero;
                target = other.gameObject;
            }
        }
        else if (other.gameObject.CompareTag("Enhancer"))
        {
            maxRange = 50;
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            main.duration = maxRange / velocity;
            main.startLifetime = maxRange / velocity;
            particle.Play();
        }
    }
}
