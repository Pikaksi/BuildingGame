using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
// max speed while sprinting, walking and stationary

    [SerializeField] float strafeMultiplier;
    [SerializeField] float movementMultiplier;
    [SerializeField] float sprintMultiplier;
    [SerializeField] Vector3 startPos;
    [SerializeField] Vector3 gravity;
    [SerializeField] float jumpPower;
    [SerializeField] float maxSpeedWalking;  // also applied when standing still
    [SerializeField] float maxSpeedSprinting;
    [SerializeField] float jumpCooldown;
    
    GameObject cameraGameObject;
    CameraFollow playerFollowScript;
    GroundColliderScript groundColliderScript;
    Rigidbody rb;
    Vector3 fullMovement;
    float jumpCooldownRemaining = 0f;

    void Awake()
    {
        Physics.gravity = gravity;
        cameraGameObject = GameObject.FindWithTag("MainCamera");
        playerFollowScript = cameraGameObject.GetComponent<CameraFollow>();
        rb = gameObject.GetComponent<Rigidbody>();
        groundColliderScript = gameObject.GetComponentInChildren<GroundColliderScript>();
    }

    void Start()
    {
        transform.position = startPos;
    }

    void Update()
    {
        HorizontalMovement();
        Jumping();
        jumpCooldownRemaining = Mathf.Clamp(jumpCooldownRemaining - Time.deltaTime, 0f, 999f);
    }

    private void Jumping()
    {
        if (Input.GetKey(KeyCode.Space) && groundColliderScript.OnGround && jumpCooldownRemaining <= 0) {
            jumpCooldownRemaining = jumpCooldown;
            rb.AddForce(new Vector3(0f, jumpPower, 0f));
        }
    }

    private void HorizontalMovement()
    {
        // camera doesn't bug when hitting a wall
        rb.angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.Euler(0f, playerFollowScript.GetCameraRotation().x, 0f);

        float sideMovement = (Input.GetKey(KeyCode.D) ? 1 : 0) + (Input.GetKey(KeyCode.A) ? -1 : 0);
        float fBMovement = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);

        float forwardMovement = fBMovement * movementMultiplier * Time.deltaTime;
        float sideWaysMovement = sideMovement * strafeMultiplier * movementMultiplier * Time.deltaTime;
    
        bool isSprinting = false;
        if (forwardMovement > 0 && fBMovement == 1 && Input.GetKey(KeyCode.LeftControl)) {
            isSprinting = true;
            forwardMovement *= sprintMultiplier;
        }

        rb.AddForce(transform.forward * forwardMovement);
        rb.AddForce(transform.right * sideWaysMovement);

        if (isSprinting) {
            rb.velocity = ScaleSpeed(maxSpeedSprinting, rb.velocity);
        }
        else {
            rb.velocity = ScaleSpeed(maxSpeedWalking, rb.velocity);
        }
    }

    private Vector3 ScaleSpeed(float maxSpeed, Vector3 currentSpeed)
    {
        if (Mathf.Sqrt(Mathf.Pow(currentSpeed.x, 2) + Mathf.Pow(currentSpeed.z, 2)) > maxSpeed) {
            float verticalSpeed = currentSpeed.y;
            currentSpeed = new Vector3(currentSpeed.x, 0f, currentSpeed.z).normalized;
            currentSpeed *= maxSpeed;
            currentSpeed = new Vector3(currentSpeed.x, verticalSpeed, currentSpeed.z);
        }
        return currentSpeed;
    }
}