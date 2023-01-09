using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public HingeJoint currentAsteroidJoint = null;

    public LevelManager levelManager;
    public List<Resource> currentResources = new List<Resource>();

    public GameObject linePrefab;
    public GameObject trajectoryLine;
    public VolumetricLineBehavior trajectoryLineBehavior;

    // Start is called before the first frame update
    void Start()
    {
        trajectoryLine = Instantiate(linePrefab);
        trajectoryLine.SetActive(false);
        trajectoryLine.transform.position = Vector3.zero;
        trajectoryLineBehavior = trajectoryLine.GetComponent<VolumetricLineBehavior>();
        trajectoryLineBehavior.LineWidth = 0.2f;
        trajectoryLineBehavior.LineColor = new Color(1, 0, 0, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            DetachFromAsteroid();
        }
        trajectoryLineBehavior.StartPos = rb.position;
        trajectoryLineBehavior.EndPos = rb.position + rb.velocity.normalized * 3;
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
        asteroid.GetComponent<Rigidbody>().angularVelocity = new Vector3(1, -1, 3);
        //rb.AddForce(1000, 0, 0);

        // Harvest the resources
        Asteroid asteroidObject = asteroid.GetComponent<Asteroid>();
        HarvestResources(asteroidObject.currentResources);
        asteroidObject.HarvestResourceObjects(gameObject);

        // Show trajectory line
        StartCoroutine(ShowTrajectoryLine());
    }
    IEnumerator ShowTrajectoryLine()
    {
        // Wait a frame before showing line, otherwise we get a flash of a bad trajectory.
        yield return new WaitForSeconds(Time.deltaTime);
        trajectoryLine.SetActive(true);
    }
    void DetachFromAsteroid()
    {
        if (currentAsteroidJoint != null)
        {
            // Hide trajectory line
            trajectoryLine.SetActive(false);

            Destroy(currentAsteroidJoint);

            // Stop z movement & move back to z=0 where all asteroids are
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            //rb.position = new Vector3(rb.position.x, rb.position.y, 0);
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