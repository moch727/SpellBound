using UnityEngine;

public class AIWalkRotator : StateMachineBehaviour
{
    [SerializeField] float speed;
    private AIScript aiScript;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiScript = animator.GetComponent<AIScript>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (aiScript.target != null)
        {
            Vector3 direction = (aiScript.target.transform.position - animator.transform.position).normalized;
            animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
            //aiScript.GetComponent<Rigidbody>().linearVelocity = shade.transform.forward * speed;
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

}
