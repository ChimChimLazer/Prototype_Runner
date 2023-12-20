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
            Vector3 weaponOffset = (positionOffset.x * current_user.right) + (positionOffset.y * current_user.up) + (positionOffset.z * current_user.forward);

            LeanTween.move(gameObject, current_user.position + weaponOffset, 0.05f);
            //transform.position = current_user.position + weaponOffset;
            transform.rotation = current_user.rotation;
            
        }
    }

    public void pickUpWeapon(Transform user)
    {
        //https://forum.unity.com/threads/deactivate-rigidbody.889837/
        current_user = user;
        Rigidbody.Destroy(rb);
        Collider.Destroy(collider);
    }

    public void DropWeapon()
    {
        current_user = null;
        //Rigidbody.Instantiate(rb);
        //Collider.Instantiate(collider);
    }
}
