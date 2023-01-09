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
    public GameObject bloodPrefab;
    public GameObject trajectoryLine;
    public VolumetricLineBehavior trajectoryLineBehavior;

    public bool hasLaunched = false;

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

    public void LaunchReset()
    {
        rb.rotation = Quaternion.identity;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.Rotate(0, 0, 90);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        gameObject.GetComponent<Renderer>().enabled = true;
        hasLaunched = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasLaunched)
        {
            var mouseCoordinates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mouseUnit = (mouseCoordinates - rb.position).normalized;
            trajectoryLine.SetActive(true);
            trajectoryLineBehavior.StartPos = rb.position;
            trajectoryLineBehavior.EndPos = rb.position + mouseUnit * 3;
            if (Input.GetKeyDown("space"))
            {
                hasLaunched = true;
                trajectoryLine.SetActive(false);

                // Kick off player
                mouseUnit.z = 0;
                rb.AddForce(mouseUnit * 250);
            }
            return;
        }
        if (Input.GetKeyDown("space"))
        {
            DetachFromAsteroid();
        }
        trajectoryLineBehavior.StartPos = rb.position;
        trajectoryLineBehavior.EndPos = rb.position + rb.velocity.normalized * 3;
        
        if (Input.GetKeyDown("r")) { 
        	levelManager.restart(); 
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!hasLaunched)
        {
            return;
        }

        if (collision.gameObject.tag == "Asteroid")
        {
            AttachToAsteroid(collision.gameObject);
        } 
     
    }
    
    void OnTriggerEnter(Collider collision) {
    	if (!hasLaunched)
        {
            return;
        }
        
        if (collision.gameObject.tag == "Ship")
        {
            levelManager.ReturnedToShip(currentResources);
        }
    
       if (collision.gameObject.tag == "pebbleAsteroids") {
         	Debug.Log("Hit mini asteroids: speed reduced"); 
        	if (rb.velocity.magnitude < 1.0f) {
        		rb.velocity = new Vector3(0, 0, 0);
        	} else { 
			rb.velocity = new Vector3(rb.velocity.x*0.5f, rb.velocity.y*0.5f, 0);
           	}
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

        // Harvest the resources
        Asteroid asteroidObject = asteroid.GetComponent<Asteroid>();
        HarvestResources(asteroidObject.currentResources);
        asteroidObject.HarvestResourceObjects(gameObject);

        // Show trajectory line
        StartCoroutine(ShowTrajectoryLineDelayed());
    }

    IEnumerator ShowTrajectoryLineDelayed()
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

            StartCoroutine(ResetZ());
        }
    }

    public void Dead()
    {
        StartCoroutine(DieAfterBlood());
    }

    IEnumerator ResetZ()
    {
        yield return new WaitForSeconds(Time.deltaTime * 2);
        // Stop z movement & move back to z=0 where all asteroids are
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
        rb.position = new Vector3(rb.position.x, rb.position.y, 0);
    }
    IEnumerator DieAfterBlood()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        gameObject.GetComponent<Renderer>().enabled = false;
        var blood = Instantiate(bloodPrefab, transform);
        yield return new WaitForSeconds(0.6f);
        Destroy(blood);
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
