using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using Cam;

namespace Character {
  public class CharacterMovement {
    private readonly float mov_maxFlatSpeed = 6f;
    private readonly float mov_acceleration = 20f;
    private readonly float rotation_speed = 0.1f;

    private readonly float jump_force = 5f;
    private readonly float jump_delay = 0.5f;
    private float jump_lastTimeExecuted;
    private bool jump_isBeingExecuted = false;

    private readonly Rigidbody rdb;
    private readonly Transform transform;

    public CharacterMovement(ICharacter character) {
      rdb = character.GetRigidbody();
      transform = character.GetTransform();
    }

    public void Move(CameraManager cam, float frontIntensity, float sideIntensity) {
      Vector3 flatVelocity = rdb.velocity.xzOnly();
      // Slow character down
      if (frontIntensity == 0 && sideIntensity == 0) {
        if (Mathf.Approximately(flatVelocity.magnitude, 0))
          return;

        if (flatVelocity.magnitude < 0.5f)
          rdb.velocity = rdb.velocity.yOnly();
        else
          rdb.AddForce((Vector3.zero - flatVelocity).normalized * mov_acceleration * 3f);
        return;
      }

      Vector3 movementDir = (cam.forward.xzOnly() * frontIntensity + cam.right.xzOnly() * sideIntensity).normalized;
      // Rotate character
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDir), rotation_speed);
      // Add force to required speed
      if (flatVelocity != movementDir * mov_maxFlatSpeed) {
        rdb.AddForce((movementDir * mov_maxFlatSpeed - flatVelocity).normalized * mov_acceleration);
        // Debug.Log($"AddForce: acc:{acc}, vel:{new Vector3(rdb.velocity.x, 0, rdb.velocity.z)}");
      }

      if (frontIntensity > 0 && sideIntensity != 0)
        cam.adjustCameraRotation(transform.rotation.eulerAngles.y);
    }

    public void Jump() {
      if (jump_isBeingExecuted && Time.fixedTime - jump_lastTimeExecuted > jump_delay)
        jump_isBeingExecuted = false;
      if (!jump_isBeingExecuted) {
        jump_lastTimeExecuted = Time.fixedTime;
        jump_isBeingExecuted = true;
        rdb.AddForce(transform.up * jump_force, ForceMode.VelocityChange);
        Debug.Log("Jump");
        return;
      }
      Debug.Log("Delay");
    }
  }
}
