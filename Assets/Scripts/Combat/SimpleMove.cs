using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public GameObject aim;
    public Vector3 speed;
    public Vector3 originSpeed;
    float m;
    CharacterController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        speed = (aim.transform.position - transform.position);
        speed.y = 0;
        originSpeed = speed.normalized;
        Vector3 aimPos = aim.transform.position;
        aimPos.y = transform.position.y;
        transform.LookAt(aimPos);
        m = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(controller != null)
        {
            if (m < 5)
            {
                m += Time.deltaTime;
            }
            speed = originSpeed * m;
            controller.SimpleMove(speed);
        }
    }
}
