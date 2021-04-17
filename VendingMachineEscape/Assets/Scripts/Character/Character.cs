using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cam;
using General;

namespace Character {
  public class Character : MonoBehaviour, ICharacter {
    private Rigidbody rdb;
    private CharacterMovement movement;
    private CameraManager cam;

    void Start() {
      rdb = GetComponent<Rigidbody>();
      cam = GameObject.Find("CameraManager").GetComponent<CameraManager>();
      movement = new CharacterMovement(this);
    }

    void Update() {

    }
    void FixedUpdate() {
      AddGravity();
      movement.Move(cam, Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
      if (Input.GetKeyDown("space") && IsGrounded())
        movement.Jump();
    }

    private void AddGravity() {
      rdb.AddForce(Physics.gravity * 2f, ForceMode.Acceleration);
    }

    private bool IsGrounded() {
      RaycastHit hit;
      float distance = 1f;
      Vector3 dir = new Vector3(0, -1);
      return Physics.Raycast(transform.position, dir, out hit, distance);
    }

    public Rigidbody GetRigidbody() {
      return rdb;
    }

    public Transform GetTransform() {
      return transform;
    }
  }
}