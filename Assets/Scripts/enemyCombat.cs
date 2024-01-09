using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class enemyCombat : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float rateOfFire;

    [Header("References")]
    public Transform orientation;
    public Transform body;

    public GameObject gun;
    public Transform muzzle;
    public GameObject bulletPrefab;

    public GameObject target;

    private enum enemyState {
        idle,
        attcking,
        chasing,
    }
    private enemyState state;

    public enum idle
    {
        idle,
        patrol,
        point,
    }

    [Header("Idle")]
    public idle idleType;

    public Transform[] patrolPoints;
    private int patrolCount;
    private int patrolNumber;

    private Vector3 point;

    [Header("Player Detection")]
    public float detectionSpeed;
    public float detectionCoolOffTime;

    private float detectionTimer;
    private float detectionCoolOff;

    private bool canSeePlayer;
    private bool playerDetected;

    private NavMeshAgent agent;
    private float agentDisableTimer;
    private float attackReady;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        point = transform.position;

        attackReady = rateOfFire;

        patrolNumber = 0;
        patrolCount = patrolPoints.Length-1;
        canSeePlayer = false;
    }

    void Update()
    {
        orientation.LookAt(target.transform.position);

        playerDetection();

        setState();

        switch (state)
        {
            case enemyState.idle:
                switch (idleType)
                {
                    case idle.idle:
                        agent.enabled = false;
                        break;

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

                    case idle.point:

                        GoTo(point);

                        break;
                }
                break;

            case enemyState.chasing:

                GoTo(target.transform.position);

                break;

            case enemyState.attcking:

                if (agent.enabled) {
                    if (agentDisableTimer <= 0)
                    {
                        agent.enabled = false;
                    } else
                    {
                        agentDisableTimer -= Time.deltaTime;
                    }
                   
                }
                
                setRotation();

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

    void setState()
    {
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

    void shoot()
    {
        attackReady = 0;
        Instantiate(bulletPrefab, muzzle.transform.position, gun.transform.rotation);
    }

    void setRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);

        Quaternion enemyRotation = Quaternion.Euler(0, orientation.transform.localRotation.eulerAngles.y, 0);
        body.rotation = enemyRotation;

        gun.transform.LookAt(target.transform.position, Vector3.forward);
    }

    void GoTo(Vector3 targetPosition)
    {
        agent.enabled = true;

        body.localRotation = Quaternion.Euler(0, 0, 0);

        agentDisableTimer = 0.3f;
        agent.SetDestination(targetPosition);
    }

    void playerDetection()
    {
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

        if (!target.activeSelf)
        {
            playerDetected = false;
        }
    }
}
