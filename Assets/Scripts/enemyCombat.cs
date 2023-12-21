using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCombat : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] GameObject target;

    void Update()
    {
        gun.transform.LookAt(target.transform.position);

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
