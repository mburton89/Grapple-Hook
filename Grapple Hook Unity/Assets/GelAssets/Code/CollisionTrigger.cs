using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrigger : MonoBehaviour
{
    public Material green;

    private void OnCollisionEnter(Collision collision)
    { 
        GetComponent<MeshRenderer>().material = green;
    }
}
