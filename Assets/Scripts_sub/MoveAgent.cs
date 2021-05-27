using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    public List<Transform> wayPoints;
    public int nextIdx;
    //public float remainingDistance;
    //public bool isPathStale;

    private Transform Target;
    private Transform platerTarget;

    private readonly float patrolSpeed = 5.5f;
    private readonly float traceSpeed = 5.0f;

    private float damping = 1.0f;

    private NavMeshAgent agent;

    private Transform enemyTr;

    private bool patrolling;
    public bool Patrolling
    {
        get { return patrolling;  }
        set
        {
            patrolling = value;
            if (patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f;
                MoveWayPoint();
            }
        }
    }

    private Vector3 traceTarget;
    public Vector3 TraceTarget
    {
        get { return traceTarget; }
        set
        {
            traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            SetTraceTarget(traceTarget);
        }
    }

    public float Speed
    {
        get { return agent.velocity.magnitude;  }
    }
    // Start is called before the first frame update
    void Start()
    {
        enemyTr = GetComponent<Transform>();

        agent = GetComponent<NavMeshAgent>();

        agent.autoBraking = false;
        agent.updateRotation = false;
        agent.speed = patrolSpeed;
        patrolling = true;

        var group = GameObject.Find("WayPointGroup");
        if (group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);

            nextIdx = ++nextIdx % wayPoints.Count;
        }
        MoveWayPoint();
    }

    void MoveWayPoint()
    {
        //Debug.Log(string.Format("MoveWayPoint idx: {0}", nextIdx));
        //isPathStale = agent.isPathStale;
        if (agent.isPathStale) return;

        agent.destination = wayPoints[nextIdx].position;
        agent.isStopped = false;
    }

    void SetTraceTarget(Vector3 pos)
    {
        if (agent.isPathStale) return;

        agent.destination = pos;
        agent.isStopped = false;
    }

    public void Stop()
    {
        agent.isStopped = true;

        agent.velocity = Vector3.zero;
        patrolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(string.Format("agent.isStopped {0} patrolling {1} agent.Destination {2} ", agent.isStopped, patrolling, agent.destination));
        if (agent.isStopped == false)
        {
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);

            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }

        if (!patrolling) return;

        //remainingDistance = agent.remainingDistance;
        if (agent.remainingDistance <= 0.5f)
        {
            nextIdx = ++nextIdx % wayPoints.Count;
            //nextIdx = Random.Range(0, wayPoints.Count);

            MoveWayPoint();
        }
    }
}
