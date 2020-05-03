using UnityEngine;
using UnityEngine.VFX;

public class GravityGunInfo : MonoBehaviour
{
    public Camera cam;
    [HideInInspector] public GameObject currentHeld;
    public Transform holdPos;
    public VisualEffect gravityGunParticle, holdParticles;
}
