using UnityEditor.Animations;
using UnityEngine.InputSystem;
using UnityEngine;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Collections;

public class PlayerScript : MonoBehaviour
{

    private PlayerCombatComponent combat;
    private PlayerLocomotionComponent locomotionComponent;

    //Animator component
    private Animator animator;

    //UI component
    [SerializeField] private Canvas playerUI;
    [SerializeField] private GameObject noteManager;

    private GameObject noteUI;
    private NoteManagerScript noteUIManager;

    void Start()
    {
        combat = GetComponent<PlayerCombatComponent>();
        locomotionComponent = GetComponent<PlayerLocomotionComponent>();

        //animator = GetComponent<Animator>();
    }
    private String spellName = "FireBall";

    //Combat component
    public GameObject magicBall;
    private GameObject newBall;
    void Update()
    {
        manageUI();
        actions();
        //animate();


    }
    void manageUI()
    {
        //noteManager.SetActive(attackingState);
    }
    void actions()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && noteUI == null && !combat.getSpellActive()) //allow only one spell casted at a time, fix later
        {
            noteUI = Instantiate(noteManager, playerUI.transform);
            noteUIManager = noteUI.GetComponent<NoteManagerScript>();

            combat.setIsAttacking(true);
            combat.createSpell(spellName);
            noteUIManager.createImages(combat.getCurrentSpell().getPattern());

        }

        if (combat.getIsAttacking() && Input.GetKeyDown(noteUIManager.getKey()))
        {
            noteUIManager.noteKeyPressed();
            if (noteUIManager.getCompleted())
            {
                //animator.SetTrigger("Attack");
                combat.attack();
                Destroy(noteUI);
                combat.setIsAttacking(false);

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

    //public void SpawnFireball()
    //{
    //    Transform pivot = GameObject.Find("MagicPivot").transform;
        
    //    newBall = Instantiate(magicBall, pivot.transform.position, Quaternion.identity, pivot);
    //}

    //public void Fire()
    //{
    //    newBall.GetComponent<SpellScript>().castSpell(gameObject, 10);
    //}
}
