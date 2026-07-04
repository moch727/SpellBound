using UnityEditor.Animations;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [HideInInspector]
    public PlayerCombatComponent combat;
    public PlayerLocomotionComponent locomotion;
    private PlayerAnimator animator;

    //UI component
    //[SerializeField] private Canvas playerUI;
    //[SerializeField] private GameObject noteManager;
    public GameObject cam;

    private GameObject noteUI;
    private NoteManagerScript noteUIManager;


    public GroundLight gl;
    //[HideInInspector]
    public bool inLight;

    public Action currentAction = Action.None;
    public enum Action
    {
        None, Attack, Dodge, Interact
    }

    //[HideInInspector]
    public GameObject interactable;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        combat = GetComponent<PlayerCombatComponent>();
        locomotion = GetComponent<PlayerLocomotionComponent>();
        animator = GetComponent<PlayerAnimator>();

        //playerUI.GetComponentInChildren<PointBar>().setMaxValue(combat.maxLP);
    }
    void Update()
    {
        bool entered = false;
        for (int i = 0; i < combat.lights.Length; i++)
        {
            if(combat.lights[i] != null && combat.lights[i].playerEntered)
            {
                entered = true; break;
            }
        }
        inLight = entered;
        //manageUI();
        //animate();
    }
    //void manageUI()
    //{
    //    playerUI.GetComponentInChildren<PointBar>().setValue(combat.lp);
    //    //noteManager.SetActive(attackingState);
    //}

    public void OnInteract()
    {
        if (currentAction == Action.None && interactable != null)
        {
            handleInteraction(interactable);
            //currentAction = Action.Interact;
        }
    }
    public void OnAttack()
    {
        //inAction = true;
        if (currentAction == Action.None)
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero; //freeze motion
            animator.setToNeutral();
            animator.animateAttack();
            combat.cast();
            currentAction = Action.Attack;
        }
    }
    public void OnDodge(InputAction.CallbackContext context)
    {
        //inAction = true;
        if (currentAction == Action.None && context.performed && GetComponent<Rigidbody>().linearVelocity.magnitude >= 0.5f) //If velocity is above negligible size
        {
            animator.animateDodge();
            locomotion.Dodge();
            currentAction = Action.Dodge;
        }
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        locomotion.faceForward();
        animator.animateAim(context.started || context.performed);
        locomotion.camRotation = (context.started || context.performed);
    }

    private void handleInteraction(GameObject o)
    {
        if(o.GetComponent<ItemScript>())
        {
            combat.addSpell(o.GetComponent<ItemScript>().collect());
            
        }
        else if (o.GetComponent<BoxScript>())
        {
            transform.position = o.GetComponent<BoxScript>().point.transform.position;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(o.GetComponent<BoxScript>().point.transform.right), 360f);
            locomotion.camRotation = false;

            animator.setToNeutral();
            animator.animateInteract("Open");
            currentAction = Action.Interact;
        }
    }

    public void enableInteractionForObject(string interaction)
    {
        if (interaction == "Box") interactable.GetComponent<BoxScript>().OpenBox();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(currentAction != Action.Interact)
        {
            if (other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner != gameObject)
            {
                combat.reduceLP((int)other.GetComponent<SpellScript>().damage);
            }
            else if (other.CompareTag("DamageBox"))
            {
                combat.reduceLP((int)other.GetComponent<MeleeHitbox>().owner.damage);
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        //inLight = other != null && other.gameObject.CompareTag("LightSrc");

        if (other.CompareTag("Interactable"))
        {
            interactable = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.CompareTag("LightSrc"))
        //{
        //    inLight = false;
        //}

        if (other.CompareTag("Interactable"))
        {
            interactable = null;
        }
    }
}
