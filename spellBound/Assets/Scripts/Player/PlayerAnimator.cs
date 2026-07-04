using Unity.Burst.Intrinsics;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private PlayerLocomotionComponent locomotionComponent;
    private PlayerCombatComponent combatComponent;
    private PlayerScript script;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        locomotionComponent = GetComponent<PlayerLocomotionComponent>();
        combatComponent = GetComponent<PlayerCombatComponent>();
        script = GetComponent<PlayerScript>();
    }
    void FixedUpdate()
    {
        if (script.currentAction == PlayerScript.Action.None) //!script.inAction
        {
            animator.SetFloat("MotionX", locomotionComponent.horizontalMovementInput.x);
            animator.SetFloat("MotionY", locomotionComponent.horizontalMovementInput.y);
        }
    }

    public void setToNeutral()
    {
        animator.SetFloat("MotionX", 0);
        animator.SetFloat("MotionY", 0);
    }
    public void animateAim(bool aim)
    {
        animator.SetBool("Load", aim);
    }
    public void animateAttack()
    {
        animator.SetTrigger("Attack");
        animator.SetInteger("SpellType", combatComponent.currentSpell().GetComponent<SpellScript>().animID);
        animator.SetBool("Load", false);
    }

    public void animateDodge()
    {
        animator.SetTrigger("Dodge");
    }
    public void Fire()
    {
        combatComponent.attack();
    }

    public void animateInteract(string action)
    {
        animator.SetTrigger(action);
    }

    public void TriggerInteract(string interaction)
    {
        script.enableInteractionForObject(interaction);
    }
    public void ResetState()
    {
        script.currentAction = PlayerScript.Action.None;
        script.locomotion.camRotation = true;
    }
}
