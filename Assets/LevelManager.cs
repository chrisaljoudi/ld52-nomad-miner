using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject asteroidPrefab;

    public GameObject player;
    public Rigidbody playerRb;

    public GameObject ship;

    void Start()
    {
        StartLevel(new[] {
          new AsteroidConfig(0, 0, new[] { new Resource(Resource.Type.Gold) }),
          new AsteroidConfig(3, 3, new[] { new Resource(Resource.Type.Energy, 2) })
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartLevel(AsteroidConfig[] asteroids)
    {
        // Configure asteroids
        foreach (AsteroidConfig config in asteroids)
        {
            GameObject asteroid = Instantiate(asteroidPrefab, new Vector3(config.x, config.y, 0), Quaternion.identity);
            asteroid.GetComponent<Asteroid>().SetResources(config.resources);
        }

        // Initialize player and ship position
        player.transform.position = new Vector3(0, -3, 0);
        ship.transform.position = new Vector3(0, -4, 0);

        // Kick off player
        playerRb.AddForce(0, 100, 0);
    }

    public void ReturnedToShip(List<Resource> resources)
    {
        foreach(Resource resource in resources)
        {
            Debug.LogFormat("Resource type {0}, count {1}", resource.material, resource.count);
        }
    }

}

class AsteroidConfig
{
    public int x;
    public int y;
    public Resource[] resources;

    public AsteroidConfig(int x, int y, Resource[] resources)
    {
        this.x = x;
        this.y = y;
        this.resources = resources;
    }
}