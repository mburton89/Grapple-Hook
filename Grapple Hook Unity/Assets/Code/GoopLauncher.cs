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
    public GameObject explosiveGoopPrefab;
    public GameObject grappleOneGoopPrefab;
    public GameObject grappleTwoGoopPrefab;

    ExplosiveGoop activeExplosiveGoop;
    GrappleGoop activeGrappleGoop;
    public DoubleGrappleGoop activeGrappleGoopOne;
    public DoubleGrappleGoop activeGrappleGoopTwo;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (activeGrappleGoop == null)
            {
                LaunchGoop(GoopType.grappleOne);
            }
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
    }

    public void HandleDoubleGoops()
    {
        activeGrappleGoopOne.linkedGoop = activeGrappleGoopTwo;
        activeGrappleGoopTwo.linkedGoop = activeGrappleGoopOne;
        activeGrappleGoopOne.Activate();
        activeGrappleGoopTwo.Activate();
    }
}
