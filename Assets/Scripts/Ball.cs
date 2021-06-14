using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public bool isPushed = false;
    //public DistanceJoint2D rightDJ, leftDJ;
    [SerializeField] Transform rightHand, leftHand;
    [SerializeField] Rigidbody2D body2d;
    [SerializeField] LineRenderer line;
    [SerializeField] float staticForce = 10, tensionForce;
    Vector3[] armPoints;

    void Start()
    {
        armPoints = new Vector3[3];
    }

    void Update() => DrawArms();

    void OnMouseDrag() => Pull();

    void OnMouseUp() => Push();

    void DrawArms()
    {
        armPoints[0] = leftHand.position;
        armPoints[1] = transform.position;
        armPoints[2] = rightHand.position;
        line.SetPositions(armPoints);
    }

    void Push()
    {
        isPushed = true;
        rightHand.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        leftHand.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        if (IsPulledDown())
            body2d.AddForce(Vector3.Normalize(transform.position) * -tensionForce, ForceMode2D.Impulse);
        else
            body2d.AddForce(Vector3.Normalize(transform.position) * tensionForce, ForceMode2D.Impulse);
    }

    bool IsPulledDown()
    {
        return transform.position.y < rightHand.position.y && transform.position.y < leftHand.position.y;
    }

    void Pull()
    {
        var mousePosWorldSpace = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        body2d.MovePosition(new Vector3(mousePosWorldSpace.x, mousePosWorldSpace.y, -1));

        leftHand.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        rightHand.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        leftHand.GetComponent<Hand>().isClicked = false;
        rightHand.GetComponent<Hand>().isClicked = false;
        leftHand.GetComponent<Hand>().dJoint.distance = 3;
        rightHand.GetComponent<Hand>().dJoint.distance = 3;
        CalculateTensionForce();
    }

    void CalculateTensionForce()
    {
        var rightHandAngle_degree = Vector3.Angle((rightHand.position - transform.position), rightHand.up);
        var leftHandAngle_degree = Vector3.Angle(leftHand.up, (leftHand.position - transform.position));
        var rightHandAngle_radian = (Mathf.PI / 180) * rightHandAngle_degree;
        var leftHandAngle_radian = (Mathf.PI / 180) * leftHandAngle_degree;
        tensionForce = staticForce / (Mathf.Sin(rightHandAngle_radian) + Mathf.Sin(leftHandAngle_radian));
    }

    //reload button click 
    public void Reload() => SceneManager.LoadSceneAsync(0);
}
