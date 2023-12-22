using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class enemyCombat : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float rateOfFire;

    [SerializeField] Transform orientation;
    [SerializeField] Transform body;

    [SerializeField] GameObject gun;
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] GameObject target;

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
    public idle idleType;

    public Transform[] patrolPoints;
    private int patrolCount;
    private int patrolNumber;

    private Vector3 point;

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
        Debug.Log(patrolCount);
    }

    void Update()
    {
        orientation.LookAt(target.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(orientation.position, orientation.forward, out hit)){
            if(hit.collider.tag == "Player")
            {
                playerDetected = true;
                state = enemyState.attcking;
            }
            else
            {
                if (playerDetected)
                {
                    state = enemyState.chasing;
                } else
                {
                    state = enemyState.idle;
                }
                
            }
        } else
        {
            if (playerDetected)
            {
                state = enemyState.chasing;
            }
            else
            {
                state = enemyState.idle;
            }
        }

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

        agentDisableTimer = 0.25f;
        agent.SetDestination(targetPosition);
    }
}
