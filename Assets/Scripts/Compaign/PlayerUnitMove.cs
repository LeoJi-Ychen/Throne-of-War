using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CharacterController))]
public class PlayerUnitMove : MonoBehaviour
{
    public Animator anim;
    public float speed = 3;
    private CharacterController controller;
    public Vector3 targetLocation;
    public bool action;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (action)
        {
            MoveToTarget();
        }
    }
    void MoveToTarget()
    {
        controller.SimpleMove(dirToTarget(targetLocation)*speed);
        Vector3 aim = targetLocation;
        aim.y = transform.position.y;
        transform.LookAt(aim);
        if (distanceToTarget(targetLocation) < 1)
        {
            action = false;
            anim.Play("idle01");
        }
        else
        {
            anim.Play("run");
        }
    }
    float distanceToTarget(GameObject target)
    {
        Vector3 t = target.transform.position;
        t.y = transform.position.y;
        float res = (t - transform.position).magnitude;
        return res;
    }
    Vector3 dirToTarget(GameObject target)
    {
        Vector3 v = (target.transform.position - transform.position);
        v.y = 0;
        return v.normalized;
    }
    float distanceToTarget(Vector3 target)
    {
        Vector3 t = target;
        t.y = transform.position.y;
        float res = (t - transform.position).magnitude;
        return res;
    }
    Vector3 dirToTarget(Vector3 target)
    {
        Vector3 v = (target - transform.position);
        v.y = 0;
        return v.normalized;
    }
}
