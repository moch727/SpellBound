using System;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using NUnit.Framework.Internal;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;

using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class AIScript : MonoBehaviour
{
    public string enemyName;

    [SerializeField] DetectionScript detection;

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

    [System.Serializable]
    public class AttackData
    {
        public string attackName;
        public int animID;
        public int attackRange;
        public int attackDamage;

        public MeleeHitbox meleeDamageBox;
    }

    [SerializeField] AttackData[] attackList;
    //[HideInInspector] 
    private AttackData currentAttack = null;
    private AttackData previousAttack = null;

    //For melee enemies
    [SerializeField] MeleeHitbox damageBox;
    //For ranged enemies
    [HideInInspector] public GameObject spellObject;
    private float maxAttackRange;

    public int health;
    public float defense; //10% defense means 90% of the damage goes thru

    public float baseXP;

    [HideInInspector] public int maxHealth;

    [SerializeField] Transform attachPoint;
    [SerializeField] Transform[] waypoints;
    private int index = 0;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;

    public float patrolWalkSpeed;
    public float combatWalkSpeed;

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
        maxHealth = health;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        maxAttackRange = attackList[attackList.Length - 1].attackRange;
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
                agent.speed = combatWalkSpeed;
                float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.transform.position);
                //Debug.Log(distanceToTarget);
                //animator.SetInteger("WalkNum", 1);

                if (distanceToTarget > maxAttackRange + 0.1f) //If distance is too far for ai to attack
                {
                    currentAttack = null;
                }
                else if (currentAttack == null)
                {
                    selectAttack(distanceToTarget);
                    Debug.Log(currentAttack.attackName);
                }

                if (distanceToTarget > agent.stoppingDistance)
                {
                    //agent.angularSpeed = 120f;
                    animator.SetBool("Walk", true);
                }
                else
                {
                    //Quaternion r = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((target.transform.position - transform.position).normalized), 360f);
                    //Mathf.Abs(transform.rotation.eulerAngles.y - r.eulerAngles.y) < 90
                    //agent.angularSpeed = 360f;

                    if (state == AIState.None) //verify if center of ai is blocked by obstacle? also check if ai is facing player
                    {
                        Debug.Log("triggerAttack");
                        animator.SetBool("Walk", false);
                        animator.SetInteger("AttackNum", currentAttack.animID);
                        animator.SetTrigger("Attack");

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
                    //Debug.Log(target.tag);
                    if (mud.transform.localScale.x <= 0.1f)
                    {
                        target.GetComponent<PlayerCombatComponent>().XP += baseXP; //Multiply by level to scale
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

    private void selectAttack(float distanceToTarget)
    {
        int validAttack = 0;
        int shortestAttack = 0;

        if(attackList.Length == 1)
        {
            validAttack = 1;
        }
        else
        {
            while (validAttack < attackList.Length)
            {
                if (attackList[validAttack].attackRange > distanceToTarget) break;
                validAttack++;
            }


            if (type == AIBehaviour.Ranged && distanceToTarget > 3f)
            {
                while (attackList[shortestAttack].attackRange < 3f)
                {
                    shortestAttack++;
                }
            }
        }


        //Debug.Log(validAttack);
        currentAttack = attackList[UnityEngine.Random.Range(shortestAttack, validAttack)];
        agent.stoppingDistance = currentAttack.attackRange;

    }
    public void reduceHealth(int reduction)
    {
        health -= (int) (reduction * (1-defense));
        if (health <= 0) //Reset everything for death
        {
            agent.speed = 0;
            GetComponent<Collider>().enabled = false;

            animator.SetBool("Walk", false);
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Death");

            if (!mud.activeSelf)
            {
                mud.transform.localScale = Vector3.zero;
                mud.SetActive(true);
            }
            death = true;
        }
    }

    public float getDamage()
    {
        return currentAttack.attackDamage;
    }
    //Pathfinding
    private void patrol()
    {
        animator.SetBool("Walk", true);
        //animator.SetInteger("WalkNum", 0);
        agent.stoppingDistance = 0;
        agent.speed = patrolWalkSpeed;
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
            else //If no waypoints listed, stay in position with starting rotation (i.e guard)
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

                animator.SetInteger("WalkNum", 1);
            }
        }
        else if (targetFound && detection.target == null)
        {
            animator.SetBool("Walk", false);
            target = null;
            targetFound = false;
            agent.ResetPath();

            animator.SetInteger("WalkNum", 0);
            //if (detection.runbackDetector != null) detection.runbackDetector.enabled = true;
        }

    }

    //Animation Events
    public void ResetState()
    {
        canMove = true;
        canRotate = true;
        state = AIState.None;
        if(attachPoint != null) attachPoint.rotation = Quaternion.identity;
        previousAttack = currentAttack;
        currentAttack = null;
        agent.enabled = true;
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
            spellObject = GameObject.Instantiate(o, attachPoint.position, Quaternion.identity, attachPoint);
            spellObject.GetComponent<SpellScript>().owner = gameObject;

            if (o.GetComponent<SpellLocomotionScript>() != null)
            {
                attachPoint.rotation = Quaternion.LookRotation((target.transform.position - transform.position).normalized);
                spellObject.GetComponent<SpellScript>().owner = attachPoint.gameObject;
            }
        }
    }
    public void Fire()
    {
        if (spellObject != null) spellObject.GetComponent<SpellScript>().shoot();
    }
    public void EnableDamagebox()
    {
        //damageBox.setDamageBox(true);
        currentAttack.meleeDamageBox.setDamageBox(true);
    }
    public void DisableDamagebox()
    {
        //damageBox.setDamageBox(false);
        currentAttack.meleeDamageBox.setDamageBox(false);
    }
    public void Terminate()
    {
        startShrink = true;
        if(mud == null)
        {
            target.GetComponent<PlayerCombatComponent>().XP += baseXP; //Multiply by level to scale
            Destroy(gameObject);
        }
        //if (mud != null) mud.SetActive(true);
        //Destroy(gameObject);
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
                //target = other.GetComponent<SpellScript>().owner;
            }
            reduceHealth((int)other.GetComponent<SpellScript>().damage);
        }
    }
}
