using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRoulette : MonoBehaviour
{
    public GameObject center;
    public Rigidbody2D body;

    public static BallRoulette instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }
    void OnEnable()
    {
        //Vector3 vector = (transform.position - center.transform.position).normalized;
        //body.AddForce((new Vector2(vector.y, (-1 * vector.x))) * -800);
        body.AddTorque(170);
    }

    public void UpdateBallSecond()
    {
        body.position = new Vector2(2.6f, 1.1f);
        body.rotation = 0;
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        body.inertia = 0;
        body.centerOfMass = Vector2.zero;
        Vector3 vector = (transform.position - center.transform.position).normalized;
        body.AddForce((new Vector2(vector.y, (-1 * vector.x))) * -1000);

    }

    /* void OnDrawGizmos()
     {
         Gizmos.color = Color.red;
         Gizmos.DrawSphere(new Vector2(xValue, yValue), 0.1f);
     }*/

    // Update is called once per frame
    void FixedUpdate()
    {
        if (RouletteManager.Instance.isGameRouletteStart)
        {
            Vector3 vector = (transform.position - center.transform.position).normalized;
            body.AddForce(new Vector2(body.velocity.x * -0.1f, body.velocity.y * -0.2f));
        }
    }

    private void OnDisable()
    {
        // Stop all forces and rotation when disabled
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        body.Sleep();
    }
}




