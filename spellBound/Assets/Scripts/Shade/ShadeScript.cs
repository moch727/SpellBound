using System;
using UnityEngine;

using UnityEngine.AI;

public class ShadeScript : MonoBehaviour
{
    [SerializeField] DetectionScript detection;

    [SerializeField] float attackRange;

    [SerializeField] GameObject fireball;
    [SerializeField] Transform attachPoint;
    [SerializeField] Transform[] waypoints;
    private int index = 0;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;

    public int health;

    public enum AIState
    {
        Standby, None, Attack, Pursue
    }

    public AIState state = AIState.None;
    private bool targetFound;
    private bool death;

    [SerializeField] bool spawnAtStart;


    //[HideInInspector]
    public GameObject target;
    public GameObject spellObject;
    public bool canRotate = true;
    public bool canMove = false;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.stoppingDistance = attackRange;
        if (waypoints.Length == 0)
        {

            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        if (spawnAtStart)
        {
            animator.SetBool("Spawned", true);
            canMove = true;
        }
    }

    private void FixedUpdate()
    {
        checkForDetection();
        if (target == null)
        {
            patrol();
        }
        else if (canMove)
        {
            agent.stoppingDistance = attackRange;
            agent.SetDestination(target.transform.position);
            if (Vector3.Distance(gameObject.transform.position, target.transform.position) > attackRange)
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                if (detection.isVisible && state == AIState.None)
                {
                    animator.SetBool("Walk", false);
                    animator.SetTrigger("Attack");
                    state = AIState.Attack;
                }
            }
            //if (Vector3.Distance(gameObject.transform.position, target.transform.position) <= attackRange && detection.isVisible) // Physics.Raycast(attachPoint.position, transform.forward, out RaycastHit hit, attackRange) && hit.collider.CompareTag("Player")
            //{
            //    //Check if in line of sight
            //    animator.SetBool("Walk", false);
            //    animator.SetTrigger("Attack");
            //}
            //else
            //{
            //    animator.SetBool("Walk", true);
            //}
        }
    }
    private void patrol()
    {
        animator.SetBool("Walk", true);
        agent.stoppingDistance = 0;
        agent.SetDestination(waypoints.Length > 0 ? waypoints[index].position : startPosition);

        if (reachedDestination())
        {
            animator.SetBool("Walk", false);
            if (waypoints.Length > 0)
            {
                if (index + 1 >= waypoints.Length) index = 0;
                else index++;

                agent.SetDestination(waypoints[index].position);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, 2f * Time.deltaTime);
                animator.SetBool("Walk", !(Mathf.Abs(Quaternion.Angle(transform.rotation, startRotation)) <= 3f));
            }
        }
    }


    private bool reachedDestination()
    {
        if (Vector3.Distance(transform.position, agent.destination) < 0.1f)
        {
            return true;
        }
        return false;
        //if(!agent.pathPending && agent.remainingDistance <= 0.5f)
        //{
        //    if(!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        //    {
        //        return true;
        //    }
        //}
        //return false;
    }
    private void checkForDetection()
    {
        //Check if in front of ai and is visible i.e nothing blocking the way btwn player and ai
        if (detection.detected && detection.isVisible) //Physics.Raycast(transform.position + Vector3.up, (detection.target.transform.position - transform.position).normalized, out RaycastHit hit, aggroRange) && hit.collider.CompareTag("Player")
        {
            if (target == null) //checkLineOfSight()
            {
                if (!animator.GetBool("Spawned")) animator.SetBool("Spawned", true);
                animator.SetBool("Walk", true);
                target = detection.target;
                targetFound = true;
            }
        }
        else if (targetFound && detection.target == null)
        {
            animator.SetBool("Walk", false);
            target = null;
            targetFound = false;
            agent.ResetPath();
            if (detection.runbackDetector != null) detection.runbackDetector.enabled = true;
        }

    }
    public void reduceHealth(int reduction)
    {
        health -= reduction;
        if (health <= 0)
        {
            agent.speed = 0;
            animator.SetBool("Walk", false);
            animator.ResetTrigger("Attack");
            death = true;
            animator.SetTrigger("Death");
        }
    }

    //Animation Events
    public void ResetState()
    {
        canMove = true;
        canRotate = true;
        state = ShadeScript.AIState.None;
    }

    public void StartMoving()
    {
        canMove = true;
    }
    public void Terminate()
    {
        Destroy(gameObject);
    }

    //For ranged ai
    public void cast()
    {
        canRotate = false;

        if (fireball != null)
        {
            spellObject = GameObject.Instantiate(fireball, attachPoint.position, gameObject.transform.rotation);
            spellObject.GetComponent<SpellScript>().owner = gameObject;
        }
    }
    public void Fire()
    {
        if (spellObject != null) spellObject.GetComponent<SpellScript>().shoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        //other.
        if (other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner != gameObject)
        {
            reduceHealth((int)other.GetComponent<SpellScript>().damage);
        }
    }
}
