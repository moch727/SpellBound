using UnityEditor.Animations;
using UnityEngine.InputSystem;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerScript : MonoBehaviour
{
    [HideInInspector] public PlayerCombatComponent combat;
    [HideInInspector] public PlayerLocomotionComponent locomotion;
    [HideInInspector] public PlayerAnimator animator;

    //UI component
    //[SerializeField] private Canvas playerUI;
    //[SerializeField] private GameObject noteManager;
    [HideInInspector] public GameObject cam;

    [SerializeField] GameObject firstPersonCam;
    [SerializeField] GameObject thirdPersonCam;

    private GameObject noteUI;
    private NoteManagerScript noteUIManager;


    public EnvironmentGroundLight environmentLight;
    //[HideInInspector]
    public bool inLight;

    public Action currentAction = Action.None;
    public enum Action
    {
        None, Attack, Dodge, Interact, CutScene
    }

    //[HideInInspector]
    public UIManager UIManager;
    public GameObject interactable;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        combat = GetComponent<PlayerCombatComponent>();
        locomotion = GetComponent<PlayerLocomotionComponent>();
        animator = GetComponent<PlayerAnimator>();

        firstPersonCam.SetActive(false);
        thirdPersonCam.SetActive(true);
        cam = thirdPersonCam;
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

        //if (currentAction == Action.CutScene) animator.setToNeutral();
        //manageUI();
        //animate();
    }
    //void manageUI()
    //{
    //    playerUI.GetComponentInChildren<PointBar>().setValue(combat.lp);
    //    //noteManager.SetActive(attackingState);
    //}

    private void ForceThirdPersonCamera()
    {
        firstPersonCam.SetActive (false);
        thirdPersonCam.SetActive (true);
        thirdPersonCam.GetComponent<CinemachineBrain>().enabled = false;
    }

    public void ResetAnimation()
    {
        GetComponent<Animator>().Rebind();
        GetComponent<Animator>().Update(0f);
        animator.setToNeutral();
    }
    public void ResetCam()
    {
        firstPersonCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        thirdPersonCam.GetComponent<CinemachineBrain>().enabled = true;

        cam.SetActive(true);
    }

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
    public void OnSwitchCam()
    {
        if (cam == thirdPersonCam)
        {
            firstPersonCam.SetActive(true);
            cam = firstPersonCam;
            thirdPersonCam.SetActive(false);
        }
        else
        {
            thirdPersonCam.SetActive(true);
            cam = thirdPersonCam;
            firstPersonCam.SetActive(false);
        }
    }
    private void handleInteraction(GameObject o)
    {
        if(o.GetComponent<ItemScript>() != null)
        {
            combat.addSpell(o.GetComponent<ItemScript>().collect());
            
        }
        else
        {
            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            ForceThirdPersonCamera();

            if (o.GetComponent<BoxScript>() != null)
            {
                transform.position = o.GetComponent<BoxScript>().point.transform.position;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(o.GetComponent<BoxScript>().point.transform.right), 360f);
                locomotion.camRotation = false;

                animator.setToNeutral();
                animator.animateInteract("Open");
                currentAction = Action.Interact;
            }
            else if (o.GetComponent<FurnaceScript>() != null)
            {
                animator.setToNeutral();
                animator.animateInteract("Furnace");

                currentAction = Action.Interact;
            }
        }

    }

    public void enableInteractionForObject(string interaction)
    {
        if (interaction == "Box") interactable.GetComponent<BoxScript>().OpenBox();
        else if (interaction == "Furnace")
        {
            interactable.GetComponent<FurnaceScript>().lit = true;

            UIManager.ShowFurnaceBox();
            //Then level up UI should showup, after make sure to trigger "Resume" for player to reset
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(currentAction != Action.Interact)
        {
            if (other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner != gameObject)
            {
                combat.reduceHealth((int)other.GetComponent<SpellScript>().damage);
            }
            else if (other.CompareTag("DamageBox"))
            {
                combat.reduceHealth((int)other.GetComponent<MeleeHitbox>().getDamage());
            }
        }

        if(other.CompareTag("LightSrc") && other.GetComponent<EnvironmentGroundLight>() != null) environmentLight = other.GetComponent<EnvironmentGroundLight>();

    }
    private void OnTriggerStay(Collider other)
    {
        //inLight = other != null && other.gameObject.CompareTag("LightSrc");

        if (other.CompareTag("Interactable") && IsInteractable(other.gameObject))
        {
            interactable = other.gameObject;
        }
    }

    private bool IsInteractable(GameObject o) //For new interact objects add its script here
    {
        return o.GetComponent<BoxScript>() != null || o.GetComponent<FurnaceScript>() != null || o.GetComponent<ItemScript>() != null;
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

        if (other.CompareTag("LightSrc") && other.GetComponent<EnvironmentGroundLight>() != null) environmentLight = null;
    }
}
