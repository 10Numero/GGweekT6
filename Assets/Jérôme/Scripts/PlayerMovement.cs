using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public LayerMask Obstacle;
    public GameObject CameraPivot;
    public Vector3 Offset;
    public Transform Character;
    float Xrotation, Yrotation;

    Vector3 MovingDirection;

    [Range(0.2f, 20)]
    public float Xsensitivity, Ysensitivity;

    enum Action { jump };

    Dictionary<KeyCode, Vector3> directionKeys;
    Dictionary<KeyCode, Action> actionKeys;

    Rigidbody playerRigidbody;
    CapsuleCollider playerCollider;

    Collision currentCollision;

    Animator CharacterAnimator;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InitializeCustomParameters();
    }

    public void Update()
    {

        CharacterAnimator.SetBool("isMoving", playerRigidbody.velocity.magnitude > 5);

        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse X") != 0)
            UpdateCameraRotation();

        CameraPivot.transform.position = Vector3.Lerp(CameraPivot.transform.position, transform.position, Time.deltaTime * 30f);
        CheckCollision();

        foreach (var key in directionKeys)
        {
            if (Input.GetKey(key.Key))
                Move(key.Value);
        }

        foreach (var key in actionKeys)
        {
            if (Input.GetKey(key.Key))
                PerformAction(key.Value);
        }

        UpdatePlayerPhysics();
    }

    void InitializeCustomParameters()
    {
        CharacterAnimator = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();

        directionKeys = new Dictionary<KeyCode, Vector3>();

        directionKeys.Add(KeyCode.Z, Vector2.up);
        directionKeys.Add(KeyCode.S, Vector2.down);
        directionKeys.Add(KeyCode.Q, Vector2.left);
        directionKeys.Add(KeyCode.D, Vector2.right);

        actionKeys = new Dictionary<KeyCode, Action>();

        actionKeys.Add(KeyCode.Space, Action.jump);
    }

    void UpdateCameraRotation()
    {
        Xrotation = CameraPivot.transform.eulerAngles.y + Input.GetAxis("Mouse X") * Xsensitivity;
        Yrotation = CameraPivot.transform.eulerAngles.x - Input.GetAxis("Mouse Y") * Ysensitivity;

        if (CameraPivot.transform.localEulerAngles.x >= 270 && CameraPivot.transform.localEulerAngles.x <= 360) Yrotation = Mathf.Clamp(Yrotation, 270, 361);
        else Yrotation = Mathf.Clamp(Yrotation, -1, 90);

        CameraPivot.transform.eulerAngles = new Vector3(Yrotation, Xrotation, 0);
    }

    void CheckCollision()
    {
        RaycastHit hit;

        if (Physics.Raycast(CameraPivot.transform.position, CameraPivot.transform.GetChild(0).position - CameraPivot.transform.position, out hit, Vector3.Distance(Offset, Vector3.zero), Obstacle))
            CameraPivot.transform.GetChild(0).localPosition = Vector3.Lerp(Vector3.zero, Offset, hit.distance / Vector3.Distance(Offset, Vector3.zero) - 0.05f);
        else
            CameraPivot.transform.GetChild(0).localPosition = Offset;
    }

    void Move(Vector2 inputDirection)
    {
        Vector3 ForwardLookingDirection = new Vector3(Mathf.Sin(CameraPivot.transform.eulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(CameraPivot.transform.eulerAngles.y * Mathf.Deg2Rad));
        Vector3 LeftLookingDirection = new Vector3(Mathf.Cos(CameraPivot.transform.eulerAngles.y * Mathf.Deg2Rad), 0, -Mathf.Sin(CameraPivot.transform.eulerAngles.y * Mathf.Deg2Rad));

        MovingDirection = Vector3.Normalize(LeftLookingDirection * inputDirection.x + ForwardLookingDirection * inputDirection.y);

        playerRigidbody.AddForce(MovingDirection * 20 / (6 - playerRigidbody.drag));

        Character.rotation = Quaternion.Slerp(Character.rotation, Quaternion.LookRotation(MovingDirection, Vector3.up), Time.deltaTime * 10f);
    }

    void Jump()
    {
        if (IsLanded())
            playerRigidbody.AddForce(0, 100, 0);
    }

    bool IsLanded()
    {
        RaycastHit groundHit;
        if (Physics.SphereCast(transform.position, playerCollider.radius * (0.99f), Vector3.down, out groundHit,
        ((playerCollider.height / 2f) - playerCollider.radius) + 0.01f, Physics.AllLayers, QueryTriggerInteraction.Ignore))
        {
            return true;
        }

        else
            return false;
    }

    void PerformAction(Action action)
    {
        switch (action)
        {
            case Action.jump:
                Jump();
                break;
        }
    }

    void UpdatePlayerPhysics()
    {
        playerRigidbody.velocity = new Vector3(Mathf.Clamp(playerRigidbody.velocity.x, -3, 3), playerRigidbody.velocity.y,
        Mathf.Clamp(playerRigidbody.velocity.z, -3, 3));

        if (IsLanded())
            playerRigidbody.drag = 10f;

        else
            playerRigidbody.drag = 0f;
    }
}