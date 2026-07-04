using UnityEngine;

public class AttackRotator : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.gameObject.transform.LookAt(animator.gameObject.GetComponent<ShadeScript>().target.transform);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetComponent<AIScript>().target != null && animator.GetComponent<AIScript>().canRotate && animator.GetComponent<AIScript>().canMove) // && animator.GetComponent<MeleeAIScript>().canRotate && animator.GetComponent<MeleeAIScript>().canMove
        {
            Vector3 direction = (animator.GetComponent<AIScript>().target.transform.position - animator.transform.position).normalized;
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
