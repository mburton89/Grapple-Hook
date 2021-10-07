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
        objectToMove = FindObjectOfType<PlayerMovement>().transform;
        objectToMove.GetComponent<PlayerMovement>().enabled = false;
        objectToMove.GetComponent<Rigidbody>().useGravity = false;
        shouldGrapple = true;
    }

    private void Update()
    {
        if (shouldGrapple)
        {
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, transform.position, grappleStrength * Time.deltaTime);
            if (Vector3.Distance(objectToMove.position, transform.position) < 1)
            {
                objectToMove.GetComponent<PlayerMovement>().enabled = true;
                objectToMove.GetComponent<Rigidbody>().useGravity = true;
                shouldGrapple = false;
                Destroy(gameObject);
            }
        }
    }
}
