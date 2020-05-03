using UnityEngine;

public class GravityGunIdle : StateMachineBehaviour
{
    private GravityGunInfo gravityGunInfo;

    [SerializeField] private LayerMask pickable;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gravityGunInfo = animator.GetComponent<GravityGunInfo>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RaycastHit hit;
        if (Physics.Raycast(gravityGunInfo.cam.transform.position, gravityGunInfo.cam.transform.forward, out hit, 20, pickable))
        {
            if (hit.collider.GetComponent<Rigidbody>() == null)
            {
                Debug.LogWarning("You forgot to add Rigidbody to " + hit.collider.name);
                return;
            }

            gravityGunInfo.currentHeld = hit.collider.gameObject;
            animator.SetBool("CanCatch", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

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
