using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCombat : MonoBehaviour
{
    
    [SerializeField] Transform orientation;
    [SerializeField] Transform body;

    [SerializeField] GameObject gun;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] GameObject target;

    void Update()
    {
        // Quaternion playerRotation = Quaternion.Euler(0, playerCamera.transform.localRotation.eulerAngles.y, 0);
        orientation.LookAt(target.transform.position);

        Quaternion enemyRotation = Quaternion.Euler(0, orientation.transform.localRotation.eulerAngles.y, 0);
        Debug.Log(enemyRotation.ToString());
        body.rotation = enemyRotation;
        if (Input.GetKeyDown(KeyCode.J))
        {
            shoot();
        }
    }

    void shoot()
    {
        Instantiate(bulletPrefab, gun.transform.position, gun.transform.rotation);
    }
}
