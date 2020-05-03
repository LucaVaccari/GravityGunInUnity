using UnityEngine;

public class GravityGunOpen : StateMachineBehaviour
{
    private GravityGunInfo gravityGunInfo;

    [SerializeField] private LayerMask pickable;
    [SerializeField] private float force = 25;
    [SerializeField] private AudioClip openClip, shootClip, pickUpClip;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gravityGunInfo = animator.GetComponent<GravityGunInfo>();
        //animator.GetComponent<AudioSource>().PlayOneShot(openClip, .5f);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RaycastHit hit;
        if (Input.GetButtonDown("PickUp"))
        {
            animator.GetComponent<AudioSource>().PlayOneShot(pickUpClip);
            animator.SetTrigger("Catch");
        }
        else if (Input.GetButtonDown("Shoot"))
        {
            //blast
            Collider[] objects = Physics.OverlapSphere(gravityGunInfo.holdPos.position, 5, pickable);

            foreach (Collider obj in objects)
            {
                if (obj.GetComponent<Rigidbody>() != null)
                {
                    obj.GetComponent<Rigidbody>().AddExplosionForce(force, gravityGunInfo.cam.transform.position, 5, 1, ForceMode.Impulse);
                }
            }

            gravityGunInfo.currentHeld = null;
            animator.GetComponent<AudioSource>().PlayOneShot(shootClip);
            animator.SetTrigger("Shoot");
        }
        else if (!Physics.Raycast(gravityGunInfo.cam.transform.position, gravityGunInfo.cam.transform.forward, out hit, 20, pickable))
        {
            gravityGunInfo.currentHeld = null;
            animator.SetBool("CanCatch", false);
        }
    }

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
