using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoopLauncher : MonoBehaviour
{
    public static GoopLauncher Instance;

    public enum GoopType
    {
        explosive,
        grappleOne,
        grappleTwo
    }

    public float launchForce = 15f;

    public Transform spawnPoint;
    public Transform lineSpawnPoint;
    public GameObject explosiveGoopPrefab;
    public GameObject grappleOneGoopPrefab;
    public GameObject grappleTwoGoopPrefab;

    Goop activeGoop;
    ExplosiveGoop activeExplosiveGoop;
    GrappleGoop activeGrappleGoop;
    [HideInInspector] public DoubleGrappleGoop activeGrappleGoopOne;
    [HideInInspector] public DoubleGrappleGoop activeGrappleGoopTwo;

    public LineRenderer lineRenderer;

    private void Awake()
    {
        Instance = this;
        lineRenderer.SetPosition(0, spawnPoint.position);
    }

    void Update()
    {

        if (activeGoop != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, lineSpawnPoint.position);
            lineRenderer.SetPosition(1, activeGoop.transform.position);
        }
        else
        {
            lineRenderer.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (activeGrappleGoop != null)
            {
                activeGrappleGoop.Deactivate();
                activeGrappleGoop = null;
            }
            LaunchGoop(GoopType.grappleOne);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (activeGrappleGoopOne != null && activeGrappleGoopTwo != null)
            {
                Destroy(activeGrappleGoopOne.gameObject);
                Destroy(activeGrappleGoopTwo.gameObject);
                activeGrappleGoopOne = null;
                activeGrappleGoopTwo = null;
            }
            else
            {
                LaunchGoop(GoopType.grappleTwo);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (activeExplosiveGoop != null)
            {
                activeExplosiveGoop.Activate();
                activeExplosiveGoop = null;
            }
            else
            {
                LaunchGoop(GoopType.explosive);
            }
        }
    }

    void LaunchGoop(GoopType goopType)
    {
        GameObject newGoop = null;
        if (goopType == GoopType.grappleOne)
        {
            newGoop = Instantiate(grappleOneGoopPrefab, spawnPoint.position, spawnPoint.rotation);
            activeGrappleGoop = newGoop.GetComponent<GrappleGoop>();
            activeGoop = newGoop.GetComponent<GrappleGoop>() as Goop;
        }
        else if (goopType == GoopType.grappleTwo)
        {
            newGoop = Instantiate(grappleTwoGoopPrefab, spawnPoint.position, spawnPoint.rotation);
            if (activeGrappleGoopOne == null)
            {
                activeGrappleGoopOne = newGoop.GetComponent<DoubleGrappleGoop>();
            }
            else if (activeGrappleGoopOne != null)
            {
                activeGrappleGoopTwo = newGoop.GetComponent<DoubleGrappleGoop>();
            }
        }
        else if (goopType == GoopType.explosive)
        {
            newGoop = Instantiate(explosiveGoopPrefab, spawnPoint.position, spawnPoint.rotation);
            activeExplosiveGoop = newGoop.GetComponent<ExplosiveGoop>();
        }
        newGoop.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * launchForce, ForceMode.Impulse);
        lineRenderer.SetPosition(0, newGoop.transform.position);
    }

    public void HandleDoubleGoops()
    {
        activeGrappleGoopOne.linkedGoop = activeGrappleGoopTwo;
        activeGrappleGoopTwo.linkedGoop = activeGrappleGoopOne;
        activeGrappleGoopOne.Activate();
        activeGrappleGoopTwo.Activate();
    }
}
