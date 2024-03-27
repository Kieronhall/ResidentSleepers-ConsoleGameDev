using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem;
using ThirdPerson;

public class Agent : MonoBehaviour
{
    public StateManager sm;
    public WanderState wander;
    public IdleState idle;
    public SeekState seek;
    public PatrolState patrol;
    public LookAroundState lookaround;
    public ShootingState shooting;

    public Type agentType = Type.Wander;

    // NavMesh Variables
    public Transform player;
    private UnityEngine.AI.NavMeshAgent agent;

    //Wander Variables
    [Header("Wander Settings")]
    public float wanderTimer = 5f;

    //Patrol Variables
    [Header("Waypoints")]
    public GameObject patrolFlag1;
    public GameObject patrolFlag2;
    public GameObject patrolFlag3;
    public GameObject patrolFlag4;
    private Transform currentGoal;
    private Transform lastGoal;

    //Shooting Variables
    public bool ShootingActiveBool = false;
    public float rotationSpeed = 2f;
    public float lerpDuration = 1f;
    public GameObject gunOnAgent;
    public GameObject muzzleFlash;
    public HealthBar healthBar;
    float damageCooldown = 1.0f; 
    float lastDamageTime; 


    public enum Type
    {
        Wander,
        Patrol,
    }
    
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
        shooting = new ShootingState(this, sm);
        sm.Init(idle);
        s = this.gameObject.GetComponent<sensors>();
        healthBar.GetComponent<HealthBar>();
        lastDamageTime = -damageCooldown; 

        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        
        switch (agentType)
        {
            case Type.Wander:
                //SetNextGoal();
                //InvokeRepeating("SetNextGoal", 0f, wanderTimer);
                break;
            case Type.Patrol: 
                break;
        }
    }

    // SEEK CODE
    public void agentGoToPlayer()
    {
        // Store the current goal as the last goal before seeking the player
        lastGoal = currentGoal;
        
        agent.destination = player.position;
    }

    // WANDER CODE
    public void WanderFunc()
    {
        if (agent == null)
        {
            Debug.LogError("Agent is null.");
            return;
        }

        // Check if the agent has reached the current goal
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            WanderSetNextGoal();
        }
    }
    public void WanderSetNextGoal()
    {
        // choosing a random patrol flag as the next goal
        GameObject[] patrolFlags = { patrolFlag1, patrolFlag2, patrolFlag3, patrolFlag4 };
        GameObject randomFlag = patrolFlags[Random.Range(0, patrolFlags.Length)];

        // Ensure the GameObject has a Transform component
        if (randomFlag != null)
        {
            currentGoal = randomFlag.transform;

            agent.SetDestination(currentGoal.position);
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
            PatrolSetNextGoal();
        }
    }
    void PatrolSetNextGoal()
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
    public void ResumeLastGoal()
    {
        // Check if there is a last goal stored
        if (lastGoal != null)
        {
            currentGoal = lastGoal;
            agent.SetDestination(currentGoal.position);
        }
    }
    // SHOOTING CODE
    public float DistanceToPlayer()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is not assigned in the Agent.");
            return float.MaxValue; // Return a large distance if playerTransform is not set.
        }

        // Calculate the distance between the agent's position and the player's position.
        float distance = Vector3.Distance(transform.position, player.position);
        return distance;
    }
    public void StandStill()
    {
        agent.SetDestination(transform.position);

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f;

        if (directionToPlayer != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSpeed);
        }

    }
    public void GunReposition(Vector3 targetPosition, Quaternion targetRotation, string gun)
    {
        StartCoroutine(LerpCoroutine(targetPosition, targetRotation, gun));
    }
    private IEnumerator LerpCoroutine(Vector3 targetPosition, Quaternion targetRotation, string gun)
    {
        float lerpTimer = 0f; // Timer for tracking lerp progress
        Transform gunOBJ = gunOnAgent.transform;

        Vector3 startPosition = gunOBJ.localPosition;
        Quaternion startRotation = gunOBJ.localRotation;


        while (lerpTimer < lerpDuration)
        {
            // Update position
            gunOBJ.localPosition = Vector3.Lerp(startPosition, targetPosition, lerpTimer / lerpDuration);

            // Update rotation
            gunOBJ.localRotation = Quaternion.Lerp(startRotation, targetRotation, lerpTimer / lerpDuration);

            // Increment lerp timer
            lerpTimer += Time.deltaTime;
            yield return null;
        }
        // Ensure final position and rotation
        gunOBJ.localPosition = targetPosition;
        gunOBJ.localRotation = targetRotation;


    }
    public void ShootingActive()
    {
        ShootingActiveBool = true;
    }
    public void ShootingNotActive()
    {
        ShootingActiveBool = false;
    }
    //public void muzzleFlashVisible()
    //{
    //    muzzleFlash.SetActive(true);
    //}
    public IEnumerator muzzleFlashVisible()
    {
        yield return new WaitForSeconds(1f);

        muzzleFlash.SetActive(true);
        
    }
    public void muzzleFlashInvisible()
    {
        muzzleFlash.SetActive(false);
    }
    public void PopShooting()
    {
        if (!s.Hit)
        {
            sm.popState();
        }
    }
    public void agentTakeDamage()
    {
        if (Time.time - lastDamageTime >= damageCooldown)
        {
            healthBar.TakeDamage(10);
            lastDamageTime = Time.time; 
        }
    }

    // DEATH ANIMATION CODE
    public void deathMovement()
    {
        agent.isStopped = true;
        muzzleFlash.SetActive(false);
    }


    //ANIMATIONS
    public void agentAnimIdle()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isWalking", false);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isRunning", false);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isShooting", false);
    }
    public void agentAnimWalk()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isRunning", false);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isWalking", true);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isShooting", false);
    }
    public void agentAnimRun()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isWalking", false);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isRunning", true);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isShooting", false);
    }
    public void agentAnimShoot()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isShooting", true);
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isRunning", false);
    }

    public void animRunningOn()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isRunning", true);
    }
    public void animRunningOff()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isRunning", false);
    }
    public void animWalkingOn()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isWalking", true);
    }
    public void animWalkingOff()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isWalking", false);
    }
    public void animShootingOn()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isShooting", true);
    }
    public void animShootingOff()
    {
        GetComponentInChildren<fsmAgentAnimationState>().animator.SetBool("isShooting", false);
    }

    // Update is called once per frame
    void Update()
    {
        sm.Update();

        switch (agentType)
        {
            case Type.Wander:
                if (s.Hit && (sm.getCurrState().GetType() != typeof(SeekState)) && !ShootingActiveBool)
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
                if (s.Hit && (sm.getCurrState().GetType() != typeof(SeekState)) && !ShootingActiveBool)
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
