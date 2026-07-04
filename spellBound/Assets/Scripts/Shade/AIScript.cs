using System;
using System.Security.Cryptography;
using NUnit.Framework.Internal;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class AIScript : MonoBehaviour
{

    [SerializeField] DetectionScript detection;

    //For melee enemies
    [SerializeField] MeleeHitbox damageBox;

    //For ranged enemies
    private GameObject spellObject;


    //[SerializeField] int[] rangeOfAttacks; //Corresponding index of action contains its effective range
    [SerializeField] int numOfAttacks;

    [SerializeField] float attackRange;

    public float damage;
    public int health;

    [SerializeField] Transform attachPoint;
    [SerializeField] Transform[] waypoints;
    private int index = 0;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;
    private ShadeScript.AIState state = ShadeScript.AIState.None;
    private bool targetFound;

    private bool death;
    [SerializeField] GameObject mud;
    private bool startShrink;

    [SerializeField] EnemyHealth healthUI;

    //[HideInInspector]
    public GameObject target;
    public bool canRotate = true;
    public bool canMove = true; //Enable this initially if spawning animation doesnt exist

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        //detection.radius = detectRange;
        agent.stoppingDistance = attackRange;
        if (waypoints.Length == 0)
        {
            
            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        healthUI.bar.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            healthUI.bar.gameObject.SetActive(true);
            healthUI.cam = target.GetComponent<PlayerScript>().cam.GetComponent<Camera>();
        }
        else healthUI.bar.gameObject.SetActive(false);

        if (!death)
        {
            checkForDetection();
            if (target == null)
            {
                //agent.angularSpeed = 120f;
                patrol();
            }
            else if (canMove) //Target found and can move
            {
                agent.stoppingDistance = attackRange;
                agent.SetDestination(target.transform.position);
                if (Vector3.Distance(gameObject.transform.position, target.transform.position) > attackRange)
                {
                    //agent.angularSpeed = 120f;
                    animator.SetBool("Walk", true);
                }
                else
                {
                    //Quaternion r = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((target.transform.position - transform.position).normalized), 360f);
                    //Mathf.Abs(transform.rotation.eulerAngles.y - r.eulerAngles.y) < 90
                    //agent.angularSpeed = 360f;
                    if (state == ShadeScript.AIState.None) //verify if center of ai is blocked by obstacle? also check if ai is facing player
                    {
                        animator.SetBool("Walk", false);
                        animator.SetTrigger("Attack");
                        selectAttack();
                        state = ShadeScript.AIState.Attack;
                    }
                }
            }
        }
        else
        {
            if(mud != null && startShrink)
            {
                mud.transform.localScale = Vector3.Slerp(mud.transform.localScale, Vector3.zero, Time.deltaTime);
                if (mud.transform.localScale.x <= 0.1f) Destroy(gameObject);
            }
        }

    }

    private void selectAttack()
    {
        //Completely random or select from an array that contains the effective range of each attack and choose one with similar distance to current distance to target
        if(numOfAttacks == 3) animator.SetInteger("AttackNum", 2);
        else animator.SetInteger("AttackNum", UnityEngine.Random.Range(0, numOfAttacks));
    }
    public void reduceHealth(int reduction)
    {
        health -= reduction;
        if (health <= 0) //Reset everything for death
        {
            agent.speed = 0;
            GetComponent<Collider>().enabled = false;
            animator.SetBool("Walk", false);
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Death");
            death = true;
        }
    }

    //Pathfinding
    private void patrol()
    {
        animator.SetBool("Walk", true);
        agent.stoppingDistance = 0;
        agent.SetDestination(waypoints.Length > 0 ? waypoints[index].position : startPosition);

        if (reachedDestination())
        {
            animator.SetBool("Walk", false);
            if(waypoints.Length > 0)
            {
                if (index + 1 >= waypoints.Length) index = 0;
                else index++;

                agent.SetDestination(waypoints[index].position);
            }
            else //If no waypoints listed, stay in position with starting rotation (i.e watchmen)
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
        if (detection.detected && detection.isVisible)
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
            //if (detection.runbackDetector != null) detection.runbackDetector.enabled = true;
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
    public void PrepareAttack(GameObject o)
    {
        canMove = false;
        canRotate = false;
        agent.ResetPath();
        if(o != null)
        {
            spellObject = GameObject.Instantiate(o, attachPoint.position, gameObject.transform.rotation, attachPoint);
            spellObject.GetComponent<SpellScript>().owner = gameObject;
        }
    }
    public void Fire()
    {
        if (spellObject != null) spellObject.GetComponent<SpellScript>().shoot();
    }
    public void EnableDamagebox()
    {
        damageBox.setDamageBox(true);
    }
    public void DisableDamagebox()
    {
        damageBox.setDamageBox(false);
    }
    public void Terminate()
    {
        startShrink = true;
        //if (mud != null) mud.SetActive(true);
        //Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner != gameObject)
        {
            if(target == null)
            {
                detection.detected = true;
                detection.target = other.GetComponent<SpellScript>().owner;
                detection.isVisible = true;

                checkForDetection();
                //target = other.GetComponent<SpellScript>().owner;
            }
            reduceHealth((int)other.GetComponent<SpellScript>().damage);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Debug.Log("a");
    //        animator.SetBool("Walk", false);
    //        target = null;
    //        agent.ResetPath();
    //    }
    //}
}
