using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private float attackReady;
    private bool attcking;

    private void Start()
    {
        attcking = false;
        attackReady = rateOfFire;
    }

    void Update()
    {
        setRotation();

        RaycastHit hit;
        if (Physics.Raycast(orientation.position, orientation.forward, out hit)){
            if(hit.collider.tag == "Player")
            {
                attcking = true;
            }
            else
            {
                attcking = false;
            }
        } else
        {
            attcking = false;
        }

        if (attackReady >= rateOfFire)
        {
            if (attcking == true)
            {
                shoot();
            }
        }
        else
        {
            attackReady += Time.deltaTime;
        }
    }

    void shoot()
    {
        attackReady = 0;
        Instantiate(bulletPrefab, muzzle.transform.position, gun.transform.rotation);
    }

    void setRotation()
    {
        orientation.LookAt(target.transform.position);

        Quaternion enemyRotation = Quaternion.Euler(0, orientation.transform.localRotation.eulerAngles.y, 0);
        body.rotation = enemyRotation;

        gun.transform.LookAt(target.transform.position, Vector3.forward);
    }
}
