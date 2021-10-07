using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleGrappleGoop : Goop
{
    public float grappleStrength;
    public DoubleGrappleGoop linkedGoop;
    public Transform parentTransform;
    public bool shouldGrapple;

    public override void Activate()
    {
        shouldGrapple = true;
    }

    public override void HandleStick()
    {
        parentTransform = transform.parent.transform;
        if (GoopLauncher.Instance.activeGrappleGoopOne != null && GoopLauncher.Instance.activeGrappleGoopTwo != null)
        {
            GoopLauncher.Instance.HandleDoubleGoops();
        }
    }

    private void Update()
    {
        if (shouldGrapple && parentTransform.GetComponent<Rigidbody>() && linkedGoop != null)
        {
            print("move");
            parentTransform.position = Vector3.MoveTowards(parentTransform.position, linkedGoop.transform.position, grappleStrength * Time.deltaTime);
            if (Vector3.Distance(parentTransform.transform.position, linkedGoop.transform.position) < 1)
            {
                GoopLauncher.Instance.activeGrappleGoopOne = null;
                GoopLauncher.Instance.activeGrappleGoopTwo = null;
                Destroy(gameObject);
                Destroy(linkedGoop.gameObject);
            }
        }
    }
}
