using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveGoop : Goop
{
    public float explosionForce = 10f;
    public float radius = 10f;
    public GameObject explosionEffect;

    public override void HandleStick()
    {
        //Dont Need
    }

    public override void Activate()
    {
        Explode();
    }

    public void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody colliderRB = collider.GetComponent<Rigidbody>();

            if (colliderRB != null)
            {
                colliderRB.AddExplosionForce(explosionForce, transform.position, radius, 1f, ForceMode.Impulse);
            }
        }
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
    }
}
