using UnityEngine.InputSystem;
using UnityEngine;
using Unity.Cinemachine;

public class PlayerScript : MonoBehaviour
{
    
    [HideInInspector] public PlayerCombatComponent combat;
    private PlayerLocomotionComponent locomotion;
    [HideInInspector] public PlayerAnimator animator;

   
    [HideInInspector] public GameObject cam;

    [SerializeField] GameObject firstPersonCam;
    [SerializeField] GameObject thirdPersonCam;


    public EnvironmentGroundLight environmentLight;
    //[HideInInspector]
    public bool inLight;
    [SerializeField] GameObject lightBuffEffect;
    private GameObject effect;

    public Action currentAction = Action.None;
    public enum Action
    {
        None, Attack, Dodge, Interact, CutScene, Dead,
    }

    [SerializeField] HoldLightFire holdLightFire;
    //[HideInInspector]
    public UIManager UIManager;
    public GameObject interactable;

    public Transform checkPoint;
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        combat = GetComponent<PlayerCombatComponent>();
        locomotion = GetComponent<PlayerLocomotionComponent>();
        animator = GetComponent<PlayerAnimator>();

        firstPersonCam.SetActive(true);
        thirdPersonCam.GetComponent<Camera>().enabled = false;
        cam = firstPersonCam;

        if(lightBuffEffect != null) effect = GameObject.Instantiate(lightBuffEffect);
    }
    void Update()
    {
        effect.transform.position = transform.position;

        if(environmentLight != null)
        {
            inLight = true;
        }
        else
        {
            bool entered = false;
            foreach (GroundLight l in combat.lights)
            {
                if (l != null && l.playerEntered)
                {
                    entered = true; break;
                }
            }
            inLight = entered;
        }

        effect.SetActive(inLight);

        //if (currentAction == Action.CutScene) animator.setToNeutral();
    }

    private void ForceThirdPersonCamera()
    {
        firstPersonCam.SetActive (false);
        thirdPersonCam.GetComponent<Camera>().enabled = true;
        thirdPersonCam.GetComponent<CinemachineBrain>().enabled = false; //To prevent control of the camera
    }

    public void setCutSceneState()
    {
        currentAction = Action.CutScene;
        animator.ResetAnimation();
        cam.SetActive(false);
        UIManager.GetComponent<Canvas>().enabled = false;
    }
    public void disableCutSceneState()
    {
        currentAction = PlayerScript.Action.None;
        cam.SetActive(true);
        UIManager.GetComponent<Canvas>().enabled = true;        
    }
    public void ResetCam()
    {
        firstPersonCam.SetActive(false);
        thirdPersonCam.GetComponent<CinemachineBrain>().enabled = true;

        cam.SetActive(true);
    }
    public void ResetCharacter() //For respawn after gameover
    {
        currentAction = Action.None;
        animator.ResetAnimation();
        combat.ResetStats();
        transform.position = checkPoint.transform.position;
    }
    public void OnInteract()
    {
        if (currentAction == Action.None && interactable != null)
        {
            interactable.GetComponent<IInteractable>().interact(this);
        }
    }
    public void OnAttack()
    {
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
        if (currentAction == Action.None && context.performed && GetComponent<Rigidbody>().linearVelocity.magnitude >= 0.5f) //If velocity is above negligible size
        {
            animator.animateDodge();
            locomotion.Dodge();
            currentAction = Action.Dodge;
        }
    }
    public void OnHoldLight(InputAction.CallbackContext context)
    {
        Debug.Log("HoldCalled");
        // locomotion.faceForward();
        // animator.animateHold(context.started || context.performed);
        // locomotion.camRotation = context.started || context.performed;
        // holdLightFire.setFireEffect(!context.canceled);
    }
    public void OnSwitchCam()
    {
        if (cam == thirdPersonCam)
        {
            firstPersonCam.SetActive(true);
            cam = firstPersonCam;
            //thirdPersonCam.SetActive(false);
            thirdPersonCam.GetComponent<Camera>().enabled = false;
        }
        else
        {
            //thirdPersonCam.SetActive(true);
            thirdPersonCam.GetComponent<Camera>().enabled = true;
            cam = thirdPersonCam;
            firstPersonCam.SetActive(false);
        }
    }
    public void addSpell(GameObject spell) //Delegate to an inventory component later
    {
        combat.addSpell(spell);
    }
    public void setMenuState()
    {
        animator.setToNeutral();
        // locomotion.camRotation = false;
        ForceThirdPersonCamera();
        currentAction = Action.Interact;
    }
    public void disableMenuState()
    {
        currentAction = Action.None;
        // locomotion.camRotation = true;
        ResetCam();
    }
    private void OnTriggerEnter(Collider other)
    {
        // if(currentAction != Action.Interact)
        // {
        //     if (other.CompareTag("Projectile") && other.GetComponent<SpellScript>().owner != gameObject)
        //     {
        //         combat.reduceHealth((int)other.GetComponent<SpellScript>().damage);
        //     }
        //     else if (other.CompareTag("DamageBox"))
        //     {
        //         combat.reduceHealth((int)other.GetComponent<MeleeHitbox>().getDamage());
        //     }
        // }

        if(other.CompareTag("LightSrc") && other.GetComponent<EnvironmentGroundLight>() != null) environmentLight = other.GetComponent<EnvironmentGroundLight>();
        if (other.CompareTag("Checkpoint"))
        {
            // Debug.Log("Checkpoint set!");
            checkPoint = other.transform;
        }

        // if (other.CompareTag("FallDamageBox"))
        // {
        //     currentAction = Action.Dead;
        //     combat.health = 0;
        //     animator.DeathFadeIn();
        // }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Interactable") && isFacingObject(other.gameObject))
        {
            interactable = other.gameObject;
        }
        else
        {
            interactable = null;
        }
    }

    private bool isFacingObject(GameObject o)
    {
        Quaternion r = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation((o.transform.position - transform.position).normalized), 360f);
        return Mathf.Abs(transform.rotation.eulerAngles.y - r.eulerAngles.y) < 90;
        //float dot = Vector3.Dot(transform.forward, (o.transform.position - transform.position).normalized);
        //return dot > 0.7f;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            interactable = null;
        }

        if (other.CompareTag("LightSrc") && other.GetComponent<EnvironmentGroundLight>() != null) environmentLight = null;
    }
}
