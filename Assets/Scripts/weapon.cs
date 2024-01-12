using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// https://www.youtube.com/watch?v=cI3E7_f74MA
// The above by LlamaAcademy was used to create a bullet trails

public class weapon : MonoBehaviour
{
    [Header("Game Stats")]
    public string weaponName; // weapon name

    public float rateOfFire; // weapons rate of fire
    public float damage; // weapons damage per shot

    public bool fullAuto; // if the weapon is full auto or tap fire

    private float bulletReady; // if the next bullet is ready to be shot

    private Vector3 Spawnpoint; // spawn point of the weapon
    private Quaternion Spawnrotation; // spawn rotation of the weapon

    [Header("Misc Stats")]
    public Vector3 positionOffset; // position we're the player holds the weapon
    public Transform muzzle; // position we're bullets spawn
    public TrailRenderer bulletTrail; // bullet trail

    [Header("References")]
    public Rigidbody rb; // rigidbody of the weapon
    public new Collider collider; // collider of the weapon
    private Transform current_user; // Transform of player thats holding the weapon

    // called on the first frame
    private void Start()
    {
        // initalise spawn point and rotation and bullet ready
        Spawnpoint = transform.position;
        Spawnrotation = transform.rotation;

        bulletReady = rateOfFire;
    }

    // called each frame
    void Update()
    {
        // If weapon is picked up
        if (current_user != null)
        {
            // if the gun is full auto
            if (fullAuto)
            {
                if (Input.GetKey(KeyCode.Mouse0) && bulletReady >= rateOfFire)
                {
                    Shoot();
                }
                else if (bulletReady < rateOfFire)
                {
                    bulletReady += Time.deltaTime;
                }
            }
            // if the gun is semi auto
            else
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && bulletReady >= rateOfFire)
                {
                    Shoot();
                }
                else if (bulletReady < rateOfFire)
                {
                    bulletReady += Time.deltaTime;
                }
            }
        }
        
    }

    // called when player picks up the weapon
    public void pickUpWeapon(Transform user)
    {
        // sets current user to user
        current_user = user;

        // removes gravity and force from the rigidbody
        rb.isKinematic = true;
        rb.useGravity = false;

        // disables the collider
        collider.enabled = false;

        // sets the weapon rotation
        transform.rotation = current_user.rotation;

        // Moves Gun To Hand
        transform.SetParent(user.transform);
        transform.localPosition = positionOffset;
    }

    // called when user drops the weapon
    public void DropWeapon()
    {
        // removes current user
        current_user = null;

        // renable the collider
        collider.enabled = true;
        // renable gravity and forces on the rigidbody
        rb.isKinematic = false;
        rb.useGravity = true;

        transform.parent = null;

        // adds a small upwards force to the weapon 
        rb.AddForce(0f, 0.25f, 0f, ForceMode.Impulse);
    }

    // Shoots a bullet out of the gun
    void Shoot()
    {
        bulletReady = 0;
        // creates a raycast
        RaycastHit hit;
        if (Physics.Raycast(current_user.position, current_user.forward, out hit))
        {
            //spawns a bullet trail
            TrailRenderer trail = Instantiate(bulletTrail, muzzle.position, Quaternion.identity);
            
            // moves bullet trail to the position the raycast hit
            StartCoroutine(SpawnTrail(trail, hit));
            // if the ray hit an enemy damage the enemy
            if (hit.collider.tag == "Enemy")
            {
                enemyHealth enemyHit = hit.collider.gameObject.GetComponentInParent<enemyHealth>();

                enemyHit.removeHealth(damage);
            }
        }
    }

    // Moves bullet trail to the position the raycast hit and then destory the trail
    // This trail was created with help from the video by Llama academy witch is linked in a comment at the top of this scirpt
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

    // Respawns the weapon when the a dealth zone is hit
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Death Zone")
        {
            // Respawns gun if dropped in a dealth zone
            transform.position = Spawnpoint;
            transform.rotation = Spawnrotation;

            // Remove previously applied velocity 
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
        }
    }
}
