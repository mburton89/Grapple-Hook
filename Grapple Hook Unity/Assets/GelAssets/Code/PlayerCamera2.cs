using UnityEngine;

public class PlayerCamera2 : MonoBehaviour
{
    Vector3 originalPosition;

    //Shaking
    private float shakeAmount = 0.2f;
    private bool isShaking;
    private float shakeDuration = 0.05f;
    private float shakeTimer;

    //Bobbing
    private float bobAmount = 0.2f;
    private float bobDuration = 75f;
    private float bobSpeed = 1f;
    private bool bobCamera;
    private bool moveUp;
    private Vector3 goalBobPosition;

    private Camera cam;
    private Transform camTransform;
    private Transform feet;

    private readonly float camRotateX = 15.0f;
    private readonly float camRotateY = 15.0f;
    private Vector3 rotation;

    //Variable Init
    void Start()
    {
        feet = transform.Find("Feet");
        cam = GetComponentInChildren<Camera>();
        originalPosition = cam.transform.localPosition;
        goalBobPosition = originalPosition + new Vector3(0, bobAmount, 0);
        camTransform = cam.transform;
        rotation = cam.transform.eulerAngles;
        shakeTimer = shakeDuration;
        Cursor.lockState = CursorLockMode.Locked;
    }

    //No physics here.
    void Update()
    {
        HandleCamera();
        HandleShake();
        HandleBob();
    }

    private void FixedUpdate()
    {
        RotatePlayerToCamera();
    }

    public void SetBob(bool set)
    {
        if (bobCamera == set) return;
        bobCamera = set;
    }

    public void SetBobSpeed(float speed)
    {
        if (bobSpeed == speed) return;
        bobSpeed = speed;
    }

    public void SetCameraShake(bool set)
    {
        if (isShaking == set) return;
        isShaking = set;
    }

    private void HandleShake()
    {
        if (isShaking)
        {
            CameraShake();
            if (shakeTimer <= 0)
            {
                ResetCamera();
                isShaking = false;
                shakeTimer += shakeDuration;
            }
            else
            {
                shakeTimer -= Time.deltaTime;
            }
        }
    }

    private void HandleBob()
    {
        if (bobCamera)
        {
            BobCameraOnWalk();
        }
        else
        {
            ResetCamera();
        }
    }

    private void BobCameraOnWalk()
    {
        if (camTransform.localPosition == originalPosition)
        {
            moveUp = true;
            PlayFootStep();
        }
        else if (cam.transform.localPosition == goalBobPosition)
        {
            moveUp = false;
        }
        if (moveUp)
        {
            camTransform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, goalBobPosition, bobSpeed * bobAmount / bobDuration);
        }
        else
        {
            camTransform.localPosition = Vector3.MoveTowards(cam.transform.localPosition, originalPosition, bobSpeed * bobAmount / bobDuration);
        }
    }

    private void PlayFootStep()
    {

    }


    private void ResetCamera()
    {
        this.camTransform.localPosition = new Vector3(this.camTransform.localPosition.x, originalPosition.y, this.camTransform.localPosition.z);
    }

    private void CameraShake()
    {
        camTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeAmount;
    }

    //Grabs mouse Inputs, does mathemagic to rotate the camera to the desired angles.
    private void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * camRotateX;
        float mouseY = Input.GetAxis("Mouse Y") * camRotateY;
        rotation.y += mouseX;
        rotation.x -= mouseY;
        rotation.x = Mathf.Clamp(rotation.x, -90, 80);
        camTransform.eulerAngles = rotation;
    }

    //Rotates player character to cameras rotation.
    private void RotatePlayerToCamera()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, camTransform.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
