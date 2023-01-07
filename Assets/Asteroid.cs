using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject resourcePrefab;
    public Resource[] currentResources;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetResources(Resource[] resources)
    {
        currentResources = resources;
        foreach(Resource resource in currentResources)
        {
            for(int i = 1; i <= resource.count; ++i)
            {
                GameObject resourceOrb = Instantiate(resourcePrefab);
                resourceOrb.transform.parent = gameObject.transform;

                // Position it so multiples of the same resource 'stack'
                // TODO need to add offset here (or adjust the prefab with an anchor) since the position is the center of the cylinder
                resourceOrb.transform.localPosition = new Vector3(0, i * 0.55f, 0);
                resourceOrb.GetComponent<Renderer>().material.SetColor("_Color", resource.GetColor());
            }
        }
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
                return Color.gray;
            case Type.Gold:
                return Color.yellow;
            case Type.Energy:
                return Color.red;
        }
        return Color.black;
    }
}