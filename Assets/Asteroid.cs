using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject resourcePrefab;
    public List<Resource> currentResources;
    public List<GameObject> currentResourceObjects;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.rotation = Random.rotation;
        var fraction = Mathf.Min(Random.value + 0.2f, 1.0f);
        gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(fraction, -fraction, fraction * 2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetResources(Resource[] resources)
    {
        currentResources = new List<Resource>(resources);
        currentResourceObjects = new List<GameObject>();
        int resourceIndex = 0;
        foreach(Resource resource in currentResources)
        {
            for(int i = 1; i <= resource.count; ++i)
            {
                GameObject resourceOrb = Instantiate(resourcePrefab);
                currentResourceObjects.Add(resourceOrb);
                resourceOrb.transform.parent = gameObject.transform;

                float angle = (30f + resourceIndex * 30f) % 360;
                resourceOrb.transform.localPosition = new Vector3(0, 12, -0.5f);
                resourceOrb.transform.RotateAround(gameObject.transform.position, Vector3.back, angle);
                resourceOrb.GetComponent<Renderer>().material.SetColor("_Color", resource.GetColor());

                resourceIndex += 1;
            }
        }
    }

    public void HarvestResourceObjects(GameObject target)
    {
        foreach(GameObject resourceOrb in currentResourceObjects)
        {
            resourceOrb.GetComponent<ResourceOrb>().harvestTarget = target;
        }
        currentResourceObjects.Clear();
    }
}

public class Resource
{
    public enum Type
    {
        Silver,
        Gold,
        Energy
    }

    public Type material;
    public int count;

    public Resource(Type type, int count = 1)
    {
        this.material = type;
        this.count = count;
    }

    public Color GetColor()
    {
        switch(material)
        {
            case Type.Silver:
                return Color.blue;
            case Type.Gold:
                return Color.yellow;
            case Type.Energy:
                return Color.red;
        }
        return Color.black;
    }
}