using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using Cam;

namespace MainCharacter {
  public class CharacterMovement {
    private Rigidbody rdb;
    private Transform transform;
    private CharacterStats stats;

    public CharacterMovement(ICharacter character) {
      rdb = character.GetRigidbody();
      transform = character.GetTransform();
      stats = CharacterStats.GetInstance();
    }

    public void Move(CameraManager cam, float frontIntensity, float sideIntensity) {
      Vector3 flatVelocity = rdb.velocity.xzOnly();

      /* Slow character down */
      if (frontIntensity == 0 && sideIntensity == 0) {
        if (Mathf.Approximately(flatVelocity.magnitude, 0))
          return;

        if (flatVelocity.magnitude < 0.5f)
          rdb.velocity = rdb.velocity.yOnly();
        else
          rdb.AddForce((Vector3.zero - flatVelocity).normalized * stats.acceleration * 3f);
        return;
      }

      Vector3 movementDir = (cam.forward.xzOnly() * frontIntensity + cam.right.xzOnly() * sideIntensity).normalized;

      /* Rotate character */
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDir), stats.rotationSpeed);

      /* Add force to required speed */
      if (flatVelocity != movementDir * stats.maxFlatSpeed)
        rdb.AddForce((movementDir * stats.maxFlatSpeed - flatVelocity).normalized * stats.acceleration);

      if (frontIntensity > 0 && sideIntensity != 0)
        cam.adjustCameraRotation(transform.rotation.eulerAngles.y);
    }

    public void Jump() {
      if (stats.jumpIsInProgress && Time.fixedTime - stats.jumpLastTimeExecuted > stats.jumpDelay)
        stats.jumpIsInProgress = false;
      if (!stats.jumpIsInProgress) {
        stats.jumpLastTimeExecuted = Time.fixedTime;
        stats.jumpIsInProgress = true;
        rdb.AddForce(transform.up * stats.jumpForce, ForceMode.VelocityChange);
        Debug.Log("Jump");
        return;
      }
      Debug.Log("Delay");
    }
  }
}
