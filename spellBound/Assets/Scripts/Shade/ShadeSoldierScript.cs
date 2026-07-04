using System;
using UnityEngine;

using UnityEngine.AI;

public class ShadeSoldierScript : MonoBehaviour
{
    [SerializeField] DetectionScript detection;

    [SerializeField] float attackRange;
    [SerializeField] float detectRange;

    [SerializeField] Transform attachPoint;

    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;

    public int health;
    [SerializeField] float speed;

    public ShadeScript.AIState state;

    //[HideInInspector]
    public GameObject target;
    //public bool canRotate = true;
    //public bool canMove = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.stoppingDistance = attackRange;
        agent.speed = speed;

        state = ShadeScript.AIState.None;
    }

    private void FixedUpdate()
    {
        checkForDetection();
        if (target != null)
        {
            agent.SetDestination(target.transform.position);
            if(state == ShadeScript.AIState.Attack)
            {
                agent.speed = 0;
                agent.angularSpeed = 0;
            }
            else
            {
                agent.speed = speed;
                agent.angularSpeed = 120;
            }
            //Attack animation must be also complete to do this
            if (state == ShadeScript.AIState.None && Vector3.Distance(gameObject.transform.position, target.transform.position) <= attackRange && Physics.Raycast(attachPoint.position, transform.forward, out RaycastHit hit, attackRange) && hit.collider.CompareTag("Player"))
            {
                //Check if in line of sight
                animator.SetBool("Walk", false);
                animator.SetTrigger("Attack");
                SetAttackState();
            }
            else
            {
                animator.SetBool("Walk", true);
            }
        }
    }
    private void checkForDetection()
    {
        if (detection.detected)
        {
            if (target == null) //checkLineOfSight()
            {
                if (!animator.GetBool("Spawned")) animator.SetBool("Spawned", true);
                animator.SetBool("Walk", true);
                target = detection.target;
            }
        }
        else
        {
            target = null;
        }

    }
    private bool checkLineOfSight()
    {
        float angle = 40f;
        return (Physics.Raycast(attachPoint.position, Quaternion.AngleAxis(angle/2, Vector3.up) * transform.forward, out RaycastHit hit, attackRange) && hit.collider.CompareTag("Player")) ||
            (Physics.Raycast(attachPoint.position, Quaternion.AngleAxis(-angle / 2, Vector3.up) * transform.forward, out RaycastHit hit2, attackRange) && hit2.collider.CompareTag("Player"));
    }

    public void reduceHealth(int reduction)
    {
        health -= reduction;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    //Animation Events
    public void SetAttackState()
    {
        state = ShadeScript.AIState.Attack;
    }
    public void ResetState()
    {
        state = ShadeScript.AIState.None;
    }

    ////For ranged ai
    //public void cast()
    //{
    //    canRotate = false;

    //    if (fireball != null)
    //    {
    //        spellObject = GameObject.Instantiate(fireball, attachPoint.position, gameObject.transform.rotation);
    //        //spellObject = GameObject.Instantiate(fireball, attachPoint.transform);
    //        //spellObject.transform.rotation = gameObject.transform.rotation;
    //        spellObject.GetComponent<SpellScript>().owner = gameObject;
    //    }
    //}
    //public void Fire()
    //{
    //    if (spellObject != null) spellObject.GetComponent<SpellScript>().shoot();
    //}

    private void OnTriggerEnter(Collider other)
    {
        //other.
        if (other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner != gameObject)
        {
            reduceHealth((int)other.GetComponent<SpellScript>().damage);
        }
    }
    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.CompareTag("Player") && target == null && checkLineOfSight())
    //    {
    //        if (!animator.GetBool("Spawned")) animator.SetBool("Spawned", true);
    //        animator.SetBool("Walk", true);
    //        target = other.gameObject;
    //    }
    //}

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetBool("Walk", false);
            target = null;
        }
    }
}
