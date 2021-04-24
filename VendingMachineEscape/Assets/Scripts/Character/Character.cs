using UnityEngine;
using static Game.GameManager;
using static Game.CameraManager;
using static Game.InputManager;

namespace MainCharacter {
  public class Character : MonoBehaviour, ICharacter {
    private bool isActive = true;

    private Rigidbody rdb;
    private CharacterMovement movement;
    private Battery battery;

    private void Start() {
      rdb = GetComponent<Rigidbody>();
      movement = new CharacterMovement(this);
      battery = new Battery(OnBatteryEmpty);

      Launch();
    }

    private void Launch() {
      battery.StartDischarging();
      GM.SetActiveCharacter(this);
    }

    private void OnBatteryEmpty() {
      Deactivate();
      Debug.Log("DEAD");
    }

    private void FixedUpdate() {
      if (!isActive)
        return;
      AddGravity();
      ProcessMovement(IM.GetVertical(), IM.GetHorizontal());
    }

    private void ProcessMovement(float fwdInput, float sideInput) {
      Vector3 fwdDir = CM.forward * fwdInput;
      Vector3 sideDir = CM.right * sideInput;
      movement.Move(fwdDir, sideDir);
      if (fwdInput > 0 && sideInput != 0)
        CM.adjustCameraRotation(transform.rotation.eulerAngles.y);
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
      return isGrounded;
    }

    #region ICharacter
    public Rigidbody GetRigidbody() {
      return rdb;
    }

    public Transform GetTransform() {
      return transform;
    }

    public void ProcessJump() {
      if (isActive && IsGrounded())
        movement.Jump();
    }

    public void Activate() {
      isActive = true;
    }

    public void Deactivate() {
      isActive = false;
      rdb.velocity = Vector3.zero;
    }
    #endregion

    #region IBattery
    public void StartCharging() {
      battery.StartCharging();
    }

    public void StopCharging() {
      battery.StartDischarging();
    }
    #endregion
  }
}