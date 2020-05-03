using UnityEngine;

public class Sniper : MonoBehaviour
{
    [SerializeField] Transform player;

    LineRenderer line;

    private void Start()
    {
        line = GetComponent<LineRenderer>();
        line.SetPosition(0, transform.position);
    }

    private void Update()
    {
        RaycastHit hit;
        if(Physics.Linecast(transform.position, player.position, out hit))
        {
            line.SetPosition(1, Vector3.Lerp(line.GetPosition(1), hit.point, Time.deltaTime));
        }
    }
}
