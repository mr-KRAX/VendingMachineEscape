using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cam;
using Game;
using UI;
using General;

namespace MainCharacter {
  public class Character : MonoBehaviour, ICharacter {
    private GameManager gm;
    private CharacterStats stats;
    private Rigidbody rdb;
    private CharacterMovement movement;
    private CameraManager cam;
    private IEnumerator batteryCoroutine = null;


    private void Awake() {
      rdb = GetComponent<Rigidbody>();
      movement = new CharacterMovement(this);
      gm = GameManager.GetInstance();
      cam = gm.cameraManager;
      stats = CharacterStats.GetInstance();
    }

    private void Start() {
      stats.currBatteryLevel = stats.maxBatteryLevel;
      gm.ui.SetMaxBatteryLevel(stats.maxBatteryLevel);
      gm.ui.SetBatteryLevel(stats.currBatteryLevel);
      batteryCoroutine = DischargeBattery();
      StartCoroutine(batteryCoroutine);
    }

    private void FixedUpdate() {
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
      float distance = 1.5f;
      Vector3 dir = new Vector3(0, -1);
      bool isGrounded = Physics.Raycast(transform.position, dir, out hit, distance);
      Debug.DrawRay(transform.position, dir * distance, Color.red);
      // Debug.Log(hit.collider.gameObject.name);
      return isGrounded;
    }

    #region Coroutines
    private IEnumerator ChargeBattery() {
      Debug.Log("Charging");
      while (stats.currBatteryLevel < stats.maxBatteryLevel) {
        yield return new WaitForSeconds(1 / stats.chargingSpeed);
        gm.ui.SetBatteryLevel(++stats.currBatteryLevel);
      }
    }

    private IEnumerator DischargeBattery() {
      Debug.Log("Discharging");
      while (stats.currBatteryLevel > 0) {
        yield return new WaitForSeconds(1 / stats.dischargingSpeed);
        gm.ui.SetBatteryLevel(--stats.currBatteryLevel);
      }
    }
    #endregion

    #region ICharacter

    public Rigidbody GetRigidbody() {
      return rdb;
    }

    public Transform GetTransform() {
      return transform;
    }

    public void StartCharging() {
      StopCoroutine(batteryCoroutine);
      batteryCoroutine = ChargeBattery();
      StartCoroutine(batteryCoroutine);
    }

    public void StopCharging() {
      StopCoroutine(batteryCoroutine);
      batteryCoroutine = DischargeBattery();
      StartCoroutine(batteryCoroutine);
    }
    #endregion
  }
}