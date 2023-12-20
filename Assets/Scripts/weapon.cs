using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    [Header("Game Stats")]
    public string weaponName;
    public float rateOfFire;
    public float damage;

    [Header("Misc Stats")]
    public Vector3 positionOffset;
    public GameObject bullet;

    [Header("References")]
    public Rigidbody rb;
    public new Collider collider;
    private Transform current_user;

    void Update()
    {
        // If weapon is picked up
        if (current_user != null)
        {
            transform.rotation = current_user.rotation;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Shoot();
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
        RaycastHit hit;
        if (Physics.Raycast(current_user.position, current_user.forward, out hit))
        {
            GameObject current_bullet = Instantiate(bullet, transform.position, transform.rotation);
            // https://discussions.unity.com/t/how-do-i-make-an-object-always-face-the-player/5486
            // https://docs.unity3d.com/ScriptReference/Transform.LookAt.html
            current_bullet.transform.LookAt(hit.point); // Bullet Faces Point
        }
    }
}
