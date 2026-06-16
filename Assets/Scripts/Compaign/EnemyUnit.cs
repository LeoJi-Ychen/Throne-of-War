using System.Collections.Generic;
using UnityEngine;
public class EnemyUnit : MonoBehaviour
{
    public GameObject sign;
    public float gravity = -9.8f;
    private Vector3 velocity;
    public static int eid;
    public int ID;
    public int troopState;
    public bool preWar;
    public int orderId;
    public AreaGraph graph;
    public Animator anim;
    float speed = 0.5f;
    public float stopDistance = 0.2f;
    private CharacterController controller;
    public Vector3 targetLocation_last;
    public Vector3 targetLocation;
    public bool action;
    private List<Vector3> movePath;
    private int currentIndex;
    public static List<GameObject> AllEnemyUnit = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        graph = GameObject.FindWithTag("Map").GetComponent<AreaGraph>();
        ID = eid;
        eid++;
        if (WarManager.HasData)
        {
            WorldData data = new WorldData();
            data = WorldData.LoadStructFromJson();
            bool contain = false;
            int index = -1;
            for(int i = 0;i< data.emytroops.Count; i++)
            {
                if (data.emytroops[i].id == ID)
                {
                    contain = true;
                    index = i;
                    break;
                }
            }
            if (contain)
            {
                controller.enabled = false;
                transform.position = data.emytroops[index].pos;
                troopState = data.emytroops[index].state;
                controller.enabled = true;                
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {
        preWar = false;
        if (!AllEnemyUnit.Contains(this.gameObject))
        {
            AllEnemyUnit.Add(this.gameObject);
        }       
    }
    private void OnDisable()
    {
        AllEnemyUnit.Remove(this.gameObject);
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
            anim.Play("idle");
            return;
        }
        StartBattle();
        Move();
        if (WarManager.MainGameTimer > 30)
        {
            troopState = 1;
            action = true;
        }
        if (troopState == 1)
        {
            foreach(GameObject c in Castle.AllCastle)
            {
                if (c.GetComponent<Castle>().camp == 1)
                {
                    targetLocation = c.transform.position;
                    break;
                }
            }
        }
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
                    if (areaPath != null)
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
            MoveAlongPath();
            HandleGravity();
        }
        else
        {
            this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }
    void StartBattle()
    {
        foreach (var p in PlayerUnit.AllPlayerUnit)
        {
            if (p != null)
            {
                if (distanceToTarget(p) < 3)
                {
                    WarManager.NewWar(this.gameObject,p);
                }
            }
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
            anim.Play("idle");
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
}
