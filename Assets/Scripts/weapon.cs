using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class weapon : MonoBehaviour
{
    [Header("Game Stats")]
    public string weaponName;

    public float rateOfFire;
    public float damage;

    public bool fullAuto;

    private float bulletReady;

    [Header("Misc Stats")]
    public Vector3 positionOffset;
    public Transform muzzle;
    public TrailRenderer bulletTrail;

    [Header("References")]
    public Rigidbody rb;
    public new Collider collider;
    private Transform current_user;

    private void Start()
    {
        bulletReady = rateOfFire;
    }

    void Update()
    {
        // If weapon is picked up
        if (current_user != null)
        {
            transform.rotation = current_user.rotation;

            if (Input.GetKey(KeyCode.Mouse0) && bulletReady >= rateOfFire)
            {
                Shoot();

            } else if (bulletReady < rateOfFire)
            {
                bulletReady += Time.deltaTime;
            }
        }
    }

    public void pickUpWeapon(Transform user)
    {
        //https://forum.unity.com/threads/deactivate-rigidbody.889837/
        current_user = user;
        rb.isKinematic = true;
        rb.useGravity = false;
        collider.enabled = false;

        // Moves Gun To Hand
        transform.SetParent(user.transform);
        transform.localPosition = positionOffset;
    }

    public void DropWeapon()
    {
        current_user = null;

        collider.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        transform.parent = null;

        rb.AddForce(0f, 0.25f, 0f, ForceMode.Impulse);
    }

    void Shoot()
    {
        bulletReady = 0;
        RaycastHit hit;
        if (Physics.Raycast(current_user.position, current_user.forward, out hit))
        {
            TrailRenderer trail = Instantiate(bulletTrail, muzzle.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));
            if (hit.collider.tag == "Enemy")
            {
                enemyCombat enemyHit = hit.collider.gameObject.GetComponent<enemyCombat>();

                enemyHit.removeHealth(damage);
            }
        }
    }

    // https://www.youtube.com/watch?v=cI3E7_f74MA
    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        Destroy(trail.gameObject, trail.time);
    }
}
