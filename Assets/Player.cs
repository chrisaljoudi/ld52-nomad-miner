using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;

    public HingeJoint currentAsteroidJoint = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            DetachFromAsteroid();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Asteroid")
        {
            AttachToAsteroid(collision.gameObject);
        }
    }

    void AttachToAsteroid(GameObject asteroid)
    {
        if (currentAsteroidJoint != null)
        {
            Destroy(currentAsteroidJoint);
        }

        currentAsteroidJoint = gameObject.AddComponent<HingeJoint>();
        currentAsteroidJoint.connectedBody = asteroid.GetComponent<Rigidbody>();
        rb.AddForce(1000, 0, 0);
    }

    void DetachFromAsteroid()
    {
        if (currentAsteroidJoint != null)
        {
            Destroy(currentAsteroidJoint);
        }
    }
}