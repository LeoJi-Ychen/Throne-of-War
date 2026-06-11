using UnityEngine;

public class LookAtCenter : MonoBehaviour
{
    public Transform center;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 aim=center.position;
        aim.y = transform.position.y;
        transform.LookAt(aim);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Vector3 dirToTarget(Vector3 target)
    {
        Vector3 v = (target - transform.position);
        v.y = 0;
        return v.normalized;
    }
}
