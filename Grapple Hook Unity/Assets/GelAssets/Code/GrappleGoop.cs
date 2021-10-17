using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleGoop : Goop
{
    public float grappleStrength;
    Transform objectToMove;
    bool shouldGrapple;

    public override void HandleStick()
    {
        Activate();
    }

    public override void Activate()
    {
        objectToMove = FindObjectOfType<PlayerMovement2>().transform;
        objectToMove.GetComponent<PlayerMovement2>().enabled = false;
        objectToMove.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToMove.GetComponent<Rigidbody>().useGravity = false;
        shouldGrapple = true;
    }

    public void Deactivate()
    {
        objectToMove = FindObjectOfType<PlayerMovement2>().transform;
        objectToMove.GetComponent<PlayerMovement2>().enabled = true;
        objectToMove.GetComponent<Rigidbody>().useGravity = true;
        shouldGrapple = false;
        Destroy(this);
    }

    private void Update()
    {
        if (shouldGrapple)
        {
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, transform.position, grappleStrength * Time.deltaTime);
            if (Vector3.Distance(objectToMove.position, transform.position) < 1)
            {
                Deactivate();
            }
        }
    }
}
