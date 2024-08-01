using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterRoulette : MonoBehaviour
{
    public float gravity = 100f;
    
    public Rigidbody2D targetRigidbody;

    // Update is called once per frame
    /*void FixedUpdate()
    {
        if (RouletteManager.Instance.isGameRouletteStart)
        {
            foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>())
            {
                if (o.GetComponent<Rigidbody2D>())
                {
                    Vector3 vector = (transform.position - o.transform.position).normalized;
                    o.GetComponent<Rigidbody2D>().AddForce(vector * gravity);
                }
            }
        }

    }*/
    
    void FixedUpdate()
    {
        if (RouletteManager.Instance.isGameRouletteStart && targetRigidbody != null)
        {
            Vector3 vector = (transform.position - targetRigidbody.transform.position).normalized;
            targetRigidbody.AddForce(vector * gravity);
        }
    }
}
