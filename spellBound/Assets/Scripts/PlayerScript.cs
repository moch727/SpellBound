using UnityEditor.Animations;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

public class PlayerScript : MonoBehaviour
{
    //Animator component
    private Animator animator;

    //UI component
    [SerializeField] private GameObject noteManager;
    private NoteManagerScript managerScript;

    private Spell spell;

    private BarScript barScript;
    void Start()
    {
        animator = GetComponent<Animator>();
        managerScript = noteManager.GetComponent<NoteManagerScript>();
        spell = GetComponent<Spell>();
    }

    //Combat component
    public GameObject magicBall;
    private GameObject newBall;
    private bool attackingState = false;

    //Locomotion component
    private float speed = 2f;
    private float xVelocity = 0;
    private float zVelocity = 0;
    private float sprintValue;
    void Update()
    {
        manageUI();
        actions();
        animate();


    }
    void manageUI()
    {
    }
    void actions()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Pass a pattern of notes?
            spell.generateSpell("FireBall");
            managerScript.createImages(spell.getPatternValues());
            attackingState = true;
        }

        if (attackingState && Input.GetKeyDown(managerScript.getKey()))
        {
            if (managerScript.noteKeyPressed())
            {
                animator.SetTrigger("Attack");
                attackingState = false;
            }
        }


    }

    void animate()
    {
        animator.SetFloat("ZVelocity", Input.GetAxis("Vertical"));
        animator.SetFloat("XVelocity", Input.GetAxis("Horizontal"));
        animator.SetFloat("YRotation", Input.GetAxis("Mouse X"));
        animator.SetFloat("SprintValue", Input.GetAxis("Sprint"));
    }

    public void SpawnFireball()
    {
        Transform pivot = GameObject.Find("MagicPivot").transform;
        
        newBall = MagicBallScript.Instantiate(magicBall, pivot.transform.position, Quaternion.identity, pivot);
    }

    public void Fire()
    {
        newBall.GetComponent<MagicBallScript>().fire(10);
    }
}
