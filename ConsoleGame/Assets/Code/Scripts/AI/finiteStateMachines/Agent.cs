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

        //Wander
        timer = wanderTimer;
        startingPoint = transform.position;
        SetNewRandomDestination();
    }

    // SEEK PLAYER CODE
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
        // Draw a wireframe sphere in the scene view to visualize the wander radius
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
    }
#endif
}
