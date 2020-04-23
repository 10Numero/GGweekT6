using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactible : MonoBehaviour
{

    MeshCollider _collider;
    Rigidbody _rigidbody;

    public enum ObjectType
    {
        Cake,
        Bowl,
        Apple,
        Sugar
    };

    public ObjectType Type;
    public bool IsGrabbed;

    void Start()
    {
        _collider = GetComponent<MeshCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _collider.isTrigger = IsGrabbed;
        _rigidbody.isKinematic = IsGrabbed;
        _rigidbody.useGravity = !IsGrabbed;

        if (IsGrabbed)
        {
            _rigidbody.velocity = Vector3.zero;
            transform.localPosition = Vector3.zero;
            gameObject.layer = LayerMask.NameToLayer("Player");
        }

        else
            gameObject.layer = LayerMask.NameToLayer("Default");
    }
}