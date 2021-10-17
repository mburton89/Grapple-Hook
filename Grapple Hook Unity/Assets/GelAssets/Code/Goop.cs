using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Goop : MonoBehaviour
{
    [HideInInspector] public bool hasStuck;
    [SerializeField] private MeshRenderer inFlightRenderer;
    [SerializeField] private MeshRenderer impactRenderer;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasStuck)
        {
            transform.SetParent(collision.transform);
            Destroy(GetComponent<Rigidbody>());
            hasStuck = true;
            HandleStick();
            inFlightRenderer.enabled = false;
            impactRenderer.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasStuck)
        {
            transform.SetParent(other.transform);
            Destroy(GetComponent<Rigidbody>());
            hasStuck = true;
            HandleStick();
        }
    }

    public abstract void HandleStick();

    public abstract void Activate();
}
