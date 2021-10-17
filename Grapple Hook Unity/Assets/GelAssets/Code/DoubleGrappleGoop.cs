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
        GetComponent<Collider>().enabled = false;
    }

    //public void Deactivate()
    //{
    //    GoopLauncher.Instance.activeGrappleGoopOne = null;
    //    GoopLauncher.Instance.activeGrappleGoopTwo = null;

    //    if (parentTransform.gameObject.GetComponent<Rigidbody>())
    //    {
    //        parentTransform.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //        parentTransform.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    //    }
    //    if (linkedGoop.parentTransform.GetComponent<Rigidbody>())
    //    {
    //        linkedGoop.parentTransform.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //        linkedGoop.parentTransform.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    //    }
    //    if (linkedGoop.gameObject.GetComponent<Rigidbody>())
    //    {
    //        Destroy(parentTransform.gameObject.GetComponent<Rigidbody>());
    //        parentTransform.SetParent(linkedGoop.transform);
    //        print("only once");
    //    }
    //    shouldGrapple = false;
    //}

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
            parentTransform.position = Vector3.MoveTowards(parentTransform.position, linkedGoop.transform.position, grappleStrength * Time.deltaTime);
            if (Vector3.Distance(parentTransform.transform.position, linkedGoop.transform.position) < 1)
            {
                if (parentTransform.gameObject.GetComponent<Rigidbody>())
                {
                    parentTransform.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    parentTransform.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                }
                GoopLauncher.Instance.activeGrappleGoopOne = null;
                GoopLauncher.Instance.activeGrappleGoopTwo = null;
                Destroy(gameObject);
                Destroy(linkedGoop.gameObject);
            }
        }
    }

    
}
