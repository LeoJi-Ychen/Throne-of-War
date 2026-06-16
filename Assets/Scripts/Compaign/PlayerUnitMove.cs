using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CharacterController))]
public class PlayerUnitMove : MonoBehaviour
{
    public GameObject sign;
    public float gravity = -9.8f;
    private Vector3 velocity;
    public bool preWar;
    public AreaGraph graph;
    public Animator anim;
    public float speed = 3;
    public float stopDistance = 0.2f;
    private CharacterController controller;
    public Vector3 targetLocation_last;
    public Vector3 targetLocation;
    public bool action;
    private List<Vector3> movePath;
    private int currentIndex;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        graph = GameObject.FindWithTag("Map").GetComponent<AreaGraph>();
    }
    public void SetPath(List<Vector3> path)
    {
        movePath = path;
        currentIndex = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (preWar)
        {
            sign.SetActive(true);
            this.gameObject.layer = LayerMask.NameToLayer("Unit");
            controller.enabled = false;
            anim.Play("idle01");
            return;
        }
        Move();
    }
    void Move()
    {
        if (action)
        {
            this.gameObject.layer = LayerMask.NameToLayer("Unit");
            if (targetLocation != targetLocation_last)
            {
                AreaNode endNode = null;
                endNode = graph.FindArea(targetLocation);
                if (endNode != null)
                {
                    AreaNode startNode = graph.FindArea(transform.position);
                    List<AreaNode> areaPath = new List<AreaNode>();
                    List<Vector3> path = new List<Vector3>();
                    areaPath = graph.FindPathAStar(startNode, endNode);
                    if(areaPath != null)
                    {
                        if (areaPath.Count > 0)
                        {
                            path = new List<Vector3>(graph.ConvertAreaPathToWorldPath(areaPath, targetLocation));
                            SetPath(path);
                            targetLocation_last = targetLocation;
                        }
                        else
                        {
                            if (startNode == endNode)
                            {
                                path = new List<Vector3>(graph.ConvertAreaPathToWorldPath(areaPath, targetLocation));
                                SetPath(path);
                                targetLocation_last = targetLocation;
                            }
                        }
                    }                   
                }
            }       
            //MoveToTarget();
            MoveAlongPath();
            HandleGravity();
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
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
    private void MoveAlongPath()
    {
        if (movePath == null || currentIndex >= movePath.Count)
            return;

        Vector3 target = movePath[currentIndex];

        Vector3 dir = target - transform.position;
        dir.y = 0;

        if (dir.magnitude <= stopDistance)
        {
            currentIndex++;
            return;
        }

        Vector3 move = dir.normalized * speed * Time.deltaTime;

        controller.Move(move);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            Quaternion.LookRotation(dir),
            360f * Time.deltaTime);
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
    void HandleGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
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
