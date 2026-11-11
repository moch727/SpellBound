using System;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using NUnit.Framework.Internal;
using Unity.Cinemachine;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class AIScript : MonoBehaviour
{
    public string enemyName;

    [SerializeField] DetectionScript detection;
    private EntityHealthScript healthManager;
    private EnemyAnimator enemyAnimator;
    private EnemyCombat enemyCombat;

    //Enum for type of ai? (i.e ranged, melee, balanced, etc.)
    public enum AIBehaviour
    {
        Balanced,
        Melee,
        Ranged
    }

    //if melee type, it should do a ranged attack then approach?
    //If ranged, it should try to stay away, unless player in melee range

    [SerializeField] AIBehaviour type;
    [SerializeField] Transform[] waypoints;
    private int index = 0;
    private Vector3 startPosition;
    private Quaternion startRotation;
    public NavMeshAgent agent;

    public float patrolWalkSpeed;
    public enum AIState
    {
        Standby, None, Attack, Pursue
    }
    public AIState state = AIState.None;

    private bool targetFound;

    private bool death;
    [SerializeField] GameObject mud;
    private float targetSize;
    private bool startShrink;

    [SerializeField] EnemyHealth healthUI;

    [HideInInspector]
    public GameObject target;
    public bool canRotate = true;
    public bool canMove = true; //Enable this initially if spawning animation doesnt exist

    void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimator>();
        enemyCombat = GetComponent<EnemyCombat>();
        healthManager = GetComponent<EntityHealthScript>();
        healthManager.initialize();

        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length == 0)
        {
            
            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        healthUI.bar.gameObject.SetActive(false);

        targetSize = mud.transform.localScale.x;
    }

    private void Update()
    {
        if (target != null)
        {
            healthUI.gameObject.SetActive(true);
            healthUI.bar.gameObject.SetActive(true);
            if (healthUI.meleeAIScript == null) healthUI.meleeAIScript = this;
            healthUI.SetMax();
            healthUI.cam = target.GetComponent<PlayerScript>().cam.GetComponent<Camera>();
        }
        else
        {
            healthUI.gameObject.SetActive(false);
            healthUI.bar.gameObject.SetActive(false);
            healthUI.meleeAIScript = null;
        }

        if (healthManager.getHealth() <= 0) //Reset everything for death
        {
            prepareForDeath();
        }

        if (!death)
        {
            checkForDetection();
            if (this.target == null)
            {
                //agent.angularSpeed = 120f;
                patrol();
            }
            else if (canMove) //Target found and can move
            {
                agent.SetDestination(target.transform.position);
                agent.speed = enemyCombat.getCombatWalkSpeed();
                float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.transform.position);

                if (distanceToTarget > enemyCombat.getMaxAttackRange() + 0.1f) //If distance is too far for ai to attack
                {
                    enemyCombat.setCurrentAttack(null);
                }
                else if (enemyCombat.getCurrentAttack() == null)
                {
                    agent.stoppingDistance = enemyCombat.selectAttack(distanceToTarget, type).attackRange;
                }

                if (distanceToTarget > agent.stoppingDistance)
                {
                    //agent.angularSpeed = 120f;
                    enemyAnimator.animateWalk(true);
                }
                else
                {
                    //Quaternion r = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((target.transform.position - transform.position).normalized), 360f);
                    //Mathf.Abs(transform.rotation.eulerAngles.y - r.eulerAngles.y) < 90
                    //agent.angularSpeed = 360f;

                    if (state == AIState.None) //verify if center of ai is blocked by obstacle? also check if ai is facing player
                    {
                        enemyAnimator.animateAttack();
                        state = AIState.Attack;
                    }
                }
            }
        }
        else
        {
            if(mud != null)
            {
                if (startShrink)
                {
                    mud.transform.localScale = Vector3.Slerp(mud.transform.localScale, Vector3.zero, Time.deltaTime);
                    if (mud.transform.localScale.x <= 0.1f)
                    {
                        enemyCombat.dropXP();
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Vector3 targetScale = Vector3.one * targetSize;
                    targetScale.y = 0;
                    mud.transform.localScale = Vector3.MoveTowards(mud.transform.localScale, targetScale, Time.deltaTime * targetSize * 2f);
                }

            }
        }

    }

    public EntityHealthScript getHealthManager()
    {
        return healthManager;
    }

    private void prepareForDeath()
    {
        agent.speed = 0;
        GetComponent<Collider>().enabled = false;

        enemyAnimator.animateDeath();

        if (!mud.activeSelf)
        {
            mud.transform.localScale = Vector3.zero;
            mud.SetActive(true);
        }
        death = true;
    }

    public float getDamage()
    {
        return enemyCombat.getCurrentAttack().attackDamage;
    }
    //Pathfinding
    private void patrol()
    {
        enemyAnimator.animateWalk(true);

        agent.stoppingDistance = 0;
        agent.speed = patrolWalkSpeed;
        agent.SetDestination(waypoints.Length > 0 ? waypoints[index].position : startPosition);

        if (reachedDestination())
        {
            enemyAnimator.animateWalk(false);
            if (waypoints.Length > 0)
            {
                if (index + 1 >= waypoints.Length) index = 0;
                else index++;

                agent.SetDestination(waypoints[index].position);
            }
            else //If no waypoints listed, stay in position with starting rotation (i.e guard)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, 2f * Time.deltaTime);
                enemyAnimator.animateWalk(!(Mathf.Abs(Quaternion.Angle(transform.rotation, startRotation)) <= 3f));

            }
        }
    }
    private bool reachedDestination()
    {
        return Vector3.Distance(transform.position, agent.destination) < 0.1f;
    }
    private void checkForDetection()
    {
        //Check if in front of ai and is visible i.e nothing blocking the way btwn player and ai
        if (detection.detected && detection.isVisible)
        {
            if (target == null) //checkLineOfSight()
            {
                enemyAnimator.animateSpawn();
                enemyAnimator.setWalkType(1);

                enemyAnimator.animateWalk(true);
                target = detection.target;
                targetFound = true;


            }
        }
        else if (targetFound && detection.target == null)
        {
            enemyAnimator.animateWalk(false);
            enemyAnimator.setWalkType(0);

            target = null;
            targetFound = false;
            agent.ResetPath();
            //if (detection.runbackDetector != null) detection.runbackDetector.enabled = true;
        }

    }

    public void animateMud()
    {
        startShrink = true;
    }
    public bool hasMud()
    {
        return mud != null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile") && !other.GetComponent<SpellScript>().owner.CompareTag("Enemy"))
        {
            if(target == null)
            {
                detection.detected = true;
                detection.target = other.GetComponent<SpellScript>().owner;
                detection.isVisible = true;

                checkForDetection();
            }
        }
    }
}
