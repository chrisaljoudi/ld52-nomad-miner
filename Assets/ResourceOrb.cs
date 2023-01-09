using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceOrb : MonoBehaviour
{
    public GameObject harvestTarget = null;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (harvestTarget != null)
        {
            var movement = (harvestTarget.transform.position - rb.position).normalized;
            rb.position += movement * 7 * Time.deltaTime;
            if (Vector3.Distance(rb.position, harvestTarget.transform.position) < 0.015f)
            {
                harvestTarget = null;
                Destroy(gameObject);
            }
        }
    }
}
