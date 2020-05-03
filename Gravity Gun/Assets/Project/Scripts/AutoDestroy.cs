using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float destroyTime = 2;
    void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}
