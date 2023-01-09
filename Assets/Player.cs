using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public HingeJoint currentAsteroidJoint = null;

    public LevelManager levelManager;
    public List<Resource> currentResources = new List<Resource>();

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
        else if (collision.gameObject.tag == "Ship")
        {
            levelManager.ReturnedToShip(currentResources);
        }
    }

    void AttachToAsteroid(GameObject asteroid)
    {
        // Detach from previous asteroid if any
        if (currentAsteroidJoint != null)
        {
            Destroy(currentAsteroidJoint);
        }

        // Attach to asteroid and begin rotating
        currentAsteroidJoint = gameObject.AddComponent<HingeJoint>();
        currentAsteroidJoint.connectedBody = asteroid.GetComponent<Rigidbody>();
        asteroid.GetComponent<Rigidbody>().angularVelocity = new Vector3(2, 0, 2);
        //rb.AddForce(1000, 0, 0);

        // Harvest the resources
        Asteroid asteroidObject = asteroid.GetComponent<Asteroid>();
        HarvestResources(asteroidObject.currentResources);
        asteroidObject.HarvestResourceObjects(gameObject);
    }

    void DetachFromAsteroid()
    {
        if (currentAsteroidJoint != null)
        {
            Destroy(currentAsteroidJoint);
        }
    }

    void HarvestResources(List<Resource> resources)
    {
        foreach(Resource resource in resources)
        {
            Resource existing = currentResources.Find(r => r.material == resource.material);
            if (existing != null)
            {
                existing.count += resource.count;
            }
            else
            {
                currentResources.Add(resource);
            }
        }
    }
}