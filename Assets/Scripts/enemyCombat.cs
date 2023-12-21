using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCombat : MonoBehaviour
{
    
    [SerializeField] Transform orientation;
    [SerializeField] Transform body;
    
    [SerializeField] GameObject gun;
    [SerializeField] Transform muzzle;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] GameObject target;

    void Update()
    {
        orientation.LookAt(target.transform.position);

        Quaternion enemyRotation = Quaternion.Euler(0, orientation.transform.localRotation.eulerAngles.y, 0);
        body.rotation = enemyRotation;

        gun.transform.LookAt(target.transform.position, Vector3.forward);

        if (Input.GetKeyDown(KeyCode.J))
        {
            shoot();
        }
    }

    void shoot()
    {
        Instantiate(bulletPrefab, muzzle.transform.position, gun.transform.rotation);
    }
}
