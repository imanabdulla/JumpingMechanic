using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Rigidbody2D body2d;
    [SerializeField] private float force;
    public DistanceJoint2D dJoint;
    public bool isClicked;

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && ball.isPushed)
        {
            body2d.AddForce(transform.right * force, ForceMode2D.Impulse);
            isClicked = true;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (isClicked)
        {
            body2d.bodyType = RigidbodyType2D.Static;
            dJoint.distance = 1.8f;
            isClicked = false;
        }
    }
}
