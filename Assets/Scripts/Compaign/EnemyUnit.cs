using UnityEngine;
using UnityEngine.SceneManagement;
public class EnemyUnit : MonoBehaviour
{
    public static bool War;
    public string sceneName = "Battlefield";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!War)
        {
            StartBattle();
        }
    }
    void StartBattle()
    {
        foreach(Unit g in Unit.AllUnits)
        {
            if(g != null)
            {
                if (distanceToTarget(g.gameObject) < 3)
                {
                    War = true;
                }
            }
        }
        if (War)
        {
            SceneManager.LoadSceneAsync(sceneName);
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
