using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable

public class ResourceOrb : MonoBehaviour
{
    public GameObject? harvestTarget;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (harvestTarget is GameObject target)
        {
            rb.position = Vector3.MoveTowards(rb.position, target.transform.position, 0.005f);
            if (Vector3.Distance(rb.position, target.transform.position) < 0.005f)
            {
                harvestTarget = null;
                Destroy(gameObject);
            }
        }
    }
}
