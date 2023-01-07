using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Rigidbody player;

    AsteroidConfig[] asteroids = new[]
    {
        new AsteroidConfig(0, 0, new[] { new Resource(Resource.Type.Gold) }),
        new AsteroidConfig(3, 3, new[] { new Resource(Resource.Type.Energy, 2) })
    };

    void Start()
    {
        // Configure asteroids
        foreach(AsteroidConfig config in asteroids)
        {
            GameObject asteroid = Instantiate(asteroidPrefab, new Vector3(config.x, config.y, 0), Quaternion.identity);
            asteroid.GetComponent<Asteroid>().SetResources(config.resources);
        }

        // Kick off player
        player.AddForce(0, 100, 0);
    }

    // Update is called once per frame
    void Update()
    {

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