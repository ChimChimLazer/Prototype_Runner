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

    [Header("References")]
    public Rigidbody rb;
    public new Collider collider;
    private Transform current_user;

    void Update()
    {
        if (current_user != null)
        {
            transform.rotation = current_user.rotation;
        }
    }

    public void pickUpWeapon(Transform user)
    {
        //https://forum.unity.com/threads/deactivate-rigidbody.889837/
        current_user = user;
        Rigidbody.Destroy(rb);
        Collider.Destroy(collider);

        // Moves Gun To Hand
        transform.SetParent(user.transform);
        transform.localPosition = positionOffset;
    }

    public void DropWeapon()
    {
        current_user = null;
        Rigidbody.Instantiate(rb);
        Collider.Instantiate(collider);
    }
}
