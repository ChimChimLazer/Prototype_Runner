using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class enemyCombat : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float rateOfFire; // rate of fire for the enemys gun

    [Header("References")]
    public Transform orientation; // object that will always look at player 
    public Transform body; // enemy transform

    public GameObject gun; // enemys gun
    public Transform muzzle; // enemys bullet spawn location
    public GameObject bulletPrefab; // bullet prefab that the enemy will spawn

    public GameObject target; // enemys target (Will be set to the player that is in the scene)

    // enemys states
    private enum enemyState {
        idle,
        attcking,
        chasing,
    }
    private enemyState state; // enemys current state

    // enemys idle states
    public enum idle
    {
        idle,
        patrol,
        point,
    }

    [Header("Idle")]
    public idle idleType; // idle state the enemy is using

    public Transform[] patrolPoints; // patrol points to go to when using idle state
    private int patrolCount; // amount of patrol points used
    private int patrolNumber; // patrol number enemy is currently walking to

    private Vector3 point; // point we're the enemy spawned

    [Header("Player Detection")]
    public float detectionSpeed; // how fast the enemy detects the player when they can see them
    public float detectionCoolOffTime; // how long of not being able to see the player it takes before the enemy goes back to idling

    private float detectionTimer; // timer for detection
    private float detectionCoolOff; // timer for cooloff

    private bool canSeePlayer; // can the enemy see the player
    private bool playerDetected; // is the player detected

    private NavMeshAgent agent; // navmesh agent
    private float agentDisableTimer; // how long it takes for the enemy to stop moving once its found the player
    private float attackReady; // when the enemy is ready to shoot a bullet

    // called on first frame
    private void Start()
    {
        // get agent and point
        agent = GetComponent<NavMeshAgent>();
        point = transform.position;

        // initalise variables
        attackReady = rateOfFire;

        patrolNumber = 0;
        patrolCount = patrolPoints.Length-1;
        canSeePlayer = false;
    }

    // called every frame
    void Update()
    {
        // orientates the orientation object towards the player
        orientation.LookAt(target.transform.position);

        // handles player detectin
        playerDetection();

        // sets the enemys state
        setState();

        switch (state)
        {
            // idle state
            case enemyState.idle:
                switch (idleType)
                {
                    // enemy stops in place when idle
                    case idle.idle:
                        agent.enabled = false;
                        break;

                    // enemy will walk between patrol points while idle
                    case idle.patrol:

                        Vector3 currentPatrolPoint = patrolPoints[patrolNumber].position;

                        GoTo(currentPatrolPoint);

                        if (transform.position.x == currentPatrolPoint.x && transform.position.z == currentPatrolPoint.z)
                        {
                            if(patrolNumber == patrolCount)
                            {
                                patrolNumber = 0;
                            } else
                            {
                                patrolNumber += 1;
                            }
                        }
                        break;

                    // enemy will walk to spawn point when idle
                    case idle.point:

                        GoTo(point);

                        break;
                }
                break;

            // chasing state
            case enemyState.chasing:
                // enemy will run after player
                GoTo(target.transform.position);

                break;

            // attacking state
            case enemyState.attcking:

                // disable nav agent
                if (agent.enabled) {
                    if (agentDisableTimer <= 0)
                    {
                        agent.enabled = false;
                    } else
                    {
                        agentDisableTimer -= Time.deltaTime;
                    }
                   
                }
                
                // look at player
                setRotation();

                // Shoot at a set rate of fire
                if (attackReady >= rateOfFire)
                {
                    shoot();
                }
                else
                {
                    attackReady += Time.deltaTime;
                }
                break;
        }
    }

    // Sets the state of the enemy
    void setState()
    {
        // Idle - Player is not detected
        // Chasing - Player is detected but the enemy cannot see them
        // attacking - Player is detected and the enemy can see them
        if (playerDetected)
        {
            if (canSeePlayer)
            {
                state = enemyState.attcking;
            }
            else
            {
                state = enemyState.chasing;
            }

        }
        else
        {
            state = enemyState.idle;
        }
    }

    // spawn bullet and reset attack ready
    void shoot()
    {
        attackReady = 0;
        Instantiate(bulletPrefab, muzzle.transform.position, gun.transform.rotation);
    }

    // looks at player and aims gun at player
    void setRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        Quaternion enemyRotation = Quaternion.Euler(0, orientation.transform.localRotation.eulerAngles.y, 0);
        body.rotation = enemyRotation;

        gun.transform.LookAt(target.transform.position, Vector3.forward);
    }

    // moves to position
    void GoTo(Vector3 targetPosition)
    {
        agent.enabled = true;

        body.localRotation = Quaternion.Euler(0, 0, 0);

        agentDisableTimer = 0.3f;
        agent.SetDestination(targetPosition);
    }

    // Handles player detection
    void playerDetection()
    {
        // checks if teh enemy can see the player
        RaycastHit hit;
        if (Physics.Raycast(orientation.position, orientation.forward, out hit))
        {
            if (hit.collider.tag == "Player")
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }

        // detects player and undetects player
        if (!playerDetected)
        {
            if (canSeePlayer)
            {
                detectionTimer += Time.deltaTime;

                if (detectionTimer >= detectionSpeed)
                {
                    playerDetected = true;
                }

            } else
            {
                detectionTimer = 0;
            }
        } else
        {
            if (!canSeePlayer)
            {
                detectionCoolOff += Time.deltaTime;

                if(detectionCoolOff >= detectionCoolOffTime)
                {
                    playerDetected = false;
                }
            }
            else
            {
                detectionCoolOff = 0;
            }
        }

        // undetects the player if the player is dead
        if (!target.activeSelf)
        {
            playerDetected = false;
        }
    }
}
