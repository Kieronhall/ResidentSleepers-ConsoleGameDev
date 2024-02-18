using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Agent : MonoBehaviour
{
    public StateManager sm;
    public WanderState wander;
    public IdleState idle;
    public SeekState seek;
    public PatrolState patrol;
    public LookAroundState lookaround;

    // NavMesh Variables
    public Transform player;
    private UnityEngine.AI.NavMeshAgent agent;

    //Wander Variables
    public float wanderRadius = 20f;
    public float wanderTimer = 5f;
    private Vector3 target;
    private float timer;
    private Vector3 startingPoint;

    //Patrol Variables

    public GameObject patrolFlag1;
    public GameObject patrolFlag2;
    public GameObject patrolFlag3;
    public GameObject patrolFlag4;
    private Transform currentGoal;


    public enum Type
    {
        Wandering,
        Patrol,
    }
    public Type agentType = Type.Wandering;

    sensors s;
    // Start is called before the first frame update
    void Start()
    {
        sm = new StateManager();
        wander = new WanderState(this, sm);
        idle = new IdleState(this, sm);
        seek = new SeekState(this, sm);
        patrol = new PatrolState(this, sm);
        lookaround = new LookAroundState(this, sm);
        sm.Init(idle);
        s = this.gameObject.GetComponent<sensors>();

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        
        switch (agentType)
        {
            case Type.Wandering:
                timer = wanderTimer;
                startingPoint = transform.position;
                SetNewRandomDestination();
                break;
            case Type.Patrol:
                break;
        }
    }

    // SEEK CODE
    public void agentGoToPlayer()
    {
        agent.destination = player.position;
    }

    // WANDER CODE
    public void SetNewRandomDestination()
    {
        // Get a random point within the specified wander radius based on the starting point
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;

        // Ensure the point is on the NavMesh
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(startingPoint + randomDirection, out hit, wanderRadius, 1);

        // Set the new destination for the agent
        agent.SetDestination(hit.position);
    }
    public void agentWander()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            SetNewRandomDestination();
            timer = wanderTimer;
        }
    }
    // PATROL CODE
    public void PatrolFunc()
    {
        if (agent == null)
        {
            Debug.LogError("Agent is null.");
            return;
        }

        // Check if the agent has reached the current goal
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            SetNextGoal();
        }
    }
    void SetNextGoal()
    {
        // Switch the goal to the next flag
        if (currentGoal == patrolFlag1.transform)
        {
            currentGoal = patrolFlag2.transform;
        }
        else if (currentGoal == patrolFlag2.transform)
        {
            currentGoal = patrolFlag3.transform;
        }
        else if (currentGoal == patrolFlag3.transform)
        {
            currentGoal = patrolFlag4.transform;
        }
        else if (currentGoal == patrolFlag4.transform)
        {
            currentGoal = patrolFlag1.transform;
        }
        else
        {
            currentGoal = patrolFlag1.transform; // Default to patrolFlag1 if currentGoal is null or unexpected
        }

        // Set the destination for the NavMeshAgent
        agent.SetDestination(currentGoal.position);
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();

        switch (agentType)
        {
            case Type.Wandering:
                if (s.Hit && (sm.getCurrState().GetType() != typeof(SeekState)))
                {
                    Debug.Log("Hit");
                    sm.pushState(seek);
                }
                if ((s.Hit == false) && (sm.getCurrState().GetType() != typeof(WanderState)))
                {
                    sm.pushState(wander);
                }
                break;
            case Type.Patrol:
                if (s.Hit && (sm.getCurrState().GetType() != typeof(SeekState)))
                {
                    Debug.Log("Hit ");
                    sm.pushState(seek);
                }
                if ((s.Hit == false) && (sm.getCurrState().GetType() != typeof(PatrolState)))
                {
                    sm.pushState(patrol);
                }
                //if (s.Hit && (sm.getCurrState().GetType() != typeof(SeekState)) &&
                //    (Time.time > lastSeekStateExitTime + seekStateCooldown))
                //{
                //    Debug.Log("Hit ");
                //    sm.pushState(seek);
                //}
                //if ((s.Hit == false) && (sm.getCurrState().GetType() != typeof(PatrolState)))
                //{
                //    sm.pushState(patrol);
                //}
                break;
        }

    }



#if UNITY_EDITOR
    
    void OnDrawGizmos()
    {
        // wander radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);

        if (sm != null)
        {
            string currentStateName = sm.GetCurrentStateName();
            if (!string.IsNullOrEmpty(currentStateName) && transform != null)
            {
                UnityEditor.Handles.Label(transform.position + Vector3.up * 2, currentStateName);
            }
            else
            {
                Debug.LogWarning("gizmo current state name is null or transform is null.");
            }
        }
        else
        {
            Debug.LogWarning("state machine is null.");
        }

        // agent path
        if (agent != null && agent.path != null)
        {
            // Draw the path in Gizmos
            Gizmos.color = Color.blue;
            for (int i = 0; i < agent.path.corners.Length - 1; i++)
            {
                Gizmos.DrawLine(agent.path.corners[i], agent.path.corners[i + 1]);
            }
        }
    }
#endif
}
