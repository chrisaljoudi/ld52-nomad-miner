using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private AsteroidConfig[][] levels = new[]
    {
        new[] {
          new AsteroidConfig(0, 3, new[] { new Resource(Resource.Type.Gold) }),
          new AsteroidConfig(7, 7, new[] { new Resource(Resource.Type.Energy, 1) })
        }
    };

    public GameObject asteroidPrefab;

    public GameObject player;
    public GameObject ship;
    public GameObject notifyText;

    public List<GameObject> currentAsteroids;

    void Start()
    {
        notifyText.SetActive(false);
        currentAsteroids = new List<GameObject>();

        StartLevel(levels[0]);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartLevel(AsteroidConfig[] asteroids)
    {
        // Clear any existing asteroids
        if (currentAsteroids.Count != 0)
        {
            foreach(GameObject asteroid in currentAsteroids)
            {
                Destroy(asteroid);
            }
            currentAsteroids.Clear();
        }

        // Configure asteroids
        foreach (AsteroidConfig config in asteroids)
        {
            GameObject asteroid = Instantiate(asteroidPrefab, new Vector3(config.x, config.y, 0), Quaternion.identity);
            asteroid.GetComponent<Asteroid>().SetResources(config.resources);
            currentAsteroids.Add(asteroid);
        }

        // Initialize player and ship position
        player.transform.position = new Vector3(0, -3, 0);
        ship.transform.position = new Vector3(0, -4, 0);

        // Ready player
        player.GetComponent<Player>().LaunchReset();
    }

    public void ReturnedToShip(List<Resource> resources)
    {
        bool gotEnergy = false;
        foreach(Resource resource in resources)
        {
            if (resource.material == Resource.Type.Energy)
            {
                gotEnergy = true;
            }
        }

        if (!gotEnergy)
        {
            StartCoroutine(FailedLevel("Ship rejected you. Must harvest energy."));
        }
    }

    IEnumerator FailedLevel(string reason)
    {
        StartCoroutine(Notify(reason));
        player.GetComponent<Player>().Dead();
        yield return new WaitForSeconds(0.6f);
        StartLevel(new[] {
          new AsteroidConfig(0, 3, new[] { new Resource(Resource.Type.Gold) }),
          new AsteroidConfig(7, 7, new[] { new Resource(Resource.Type.Energy, 1) })
        });
    }

    public IEnumerator Notify(string notification)
    {
        notifyText.GetComponent<TextMeshPro>().text = notification;
        notifyText.SetActive(true);
        yield return new WaitForSeconds(5);
        notifyText.SetActive(false);
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