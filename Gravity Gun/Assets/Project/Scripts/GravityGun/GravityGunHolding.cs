using UnityEngine;

public class GravityGunHolding : StateMachineBehaviour
{
    private GravityGunInfo gravityGunInfo;

    [SerializeField] private float force = 50;
    [SerializeField] private AudioClip holdingClip, shootClip, dropClip;
    private Rigidbody objectRb;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gravityGunInfo = animator.GetComponent<GravityGunInfo>();
        animator.GetComponent<AudioSource>().loop = true;
        animator.GetComponent<AudioSource>().PlayOneShot(holdingClip);

        gravityGunInfo.holdParticles.Play();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (gravityGunInfo.currentHeld == null)
        {
            animator.SetBool("CanCatch", false);
        }
        try
        {
            if (gravityGunInfo.currentHeld.GetComponent<Rigidbody>() == null)
            {
                animator.SetBool("CanCatch", false);
            }
        }
        catch
        {
            animator.SetBool("CanCatch", false);
            return;
        }

        objectRb = gravityGunInfo.currentHeld.GetComponent<Rigidbody>();

        objectRb.useGravity = false;
        if (Vector3.Distance(gravityGunInfo.holdPos.position, gravityGunInfo.currentHeld.transform.position) > 5)
        {
            objectRb.AddForce(animator.transform.position - gravityGunInfo.currentHeld.transform.position);
        }
        else
        {
            objectRb.velocity = Vector3.zero;
            objectRb.angularDrag = Mathf.Infinity;
            Vector3 velocity = Vector3.Lerp(gravityGunInfo.currentHeld.transform.position, gravityGunInfo.holdPos.position, 10 * Time.deltaTime);
            //objectRb.MovePosition(velocity);
            objectRb.velocity = (gravityGunInfo.holdPos.position - velocity).normalized * 5 * Vector3.Distance(gravityGunInfo.currentHeld.transform.position, gravityGunInfo.holdPos.position);
            if (Input.GetButtonDown("Shoot"))
            {
                objectRb.useGravity = true;
                objectRb.AddForce(gravityGunInfo.cam.transform.forward * force, ForceMode.VelocityChange);
                animator.GetComponent<AudioSource>().Stop();
                animator.GetComponent<AudioSource>().PlayOneShot(shootClip);
                animator.SetTrigger("Shoot");
            }
        }


        if (Input.GetButtonDown("PickUp"))
        {
            animator.GetComponent<AudioSource>().Stop();
            animator.GetComponent<AudioSource>().PlayOneShot(dropClip);
            animator.SetBool("CanCatch", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        try
        {
            objectRb.useGravity = true;
            objectRb.angularDrag = 0.05f;
        }
        catch
        {
            return;
        }
        animator.GetComponent<AudioSource>().loop = false;
        gravityGunInfo.holdParticles.Stop();
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
