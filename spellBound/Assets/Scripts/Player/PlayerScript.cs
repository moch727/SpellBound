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
    public GameObject cam;

    private GameObject noteUI;
    private NoteManagerScript noteUIManager;


    private GroundLight gl;
    private float timeElapsed;
    void Awake()
    {
        combat = GetComponent<PlayerCombatComponent>();
        locomotionComponent = GetComponent<PlayerLocomotionComponent>();

        playerUI.GetComponentInChildren<PointBar>().setMaxValue(combat.maxLP);
        //animator = GetComponent<Animator>();
    }
    void Update()
    {
        manageUI();
        actions();
        //animate();
    }
    void manageUI()
    {
        playerUI.GetComponentInChildren<PointBar>().setValue(combat.lp);
        //noteManager.SetActive(attackingState);
    }
    void actions()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0) && noteUI == null && !combat.getSpellActive()) //allow only one spell casted at a time, fix later
        //{
        //    //noteUI = Instantiate(noteManager, playerUI.transform);
        //    //noteUIManager = noteUI.GetComponent<NoteManagerScript>();

        //    //combat.setIsAttacking(true);
        //    //combat.createSpell(spellName);
        //    //noteUIManager.createImages(combat.getCurrentSpell().getPattern());
        //    combat.attack();
        //    combat.setIsAttacking(false);

        //}

        //if (combat.getIsAttacking() && Input.GetKeyDown(noteUIManager.getKey()))
        //{
        //    noteUIManager.noteKeyPressed();
        //    if (noteUIManager.getCompleted())
        //    {
        //        //animator.SetTrigger("Attack");
        //        combat.attack();
        //        Destroy(noteUI);
        //        combat.setIsAttacking(false);

        //    }
        //}
        //Disable note system
        if (gl != null && getLightlvl() == 2)
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= 1 && combat.lp < combat.maxLP)
            {
                combat.lp += 1;
                timeElapsed = 0f;
            }
        }
    }

    public int getLightlvl()
    {
        if (gl != null) return gl.getlightLevel(Vector3.Distance(gameObject.transform.position, gl.gameObject.transform.position));
        return 0;
    }
    public void OnAttack()
    {

        if (!combat.getSpellActive())
        {
            combat.attack();
            //combat.setIsAttacking(false);
        }

    }
    public void animate()
    {
        animator.SetFloat("ZVelocity", Input.GetAxis("Vertical"));
        animator.SetFloat("XVelocity", Input.GetAxis("Horizontal"));
        animator.SetFloat("YRotation", Input.GetAxis("Mouse X"));
        animator.SetFloat("SprintValue", Input.GetAxis("Sprint"));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("LightSrc"))
        {
            gl = other.gameObject.GetComponent<GroundLight>();
            timeElapsed = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("LightSrc"))
        {
            gl = null;
        }
    }
}
