using UnityEngine;
using General;
using static MainCharacter.CharacterStats;

namespace MainCharacter {
  public class CharacterMovement {
    private Rigidbody rdb;
    private Transform transform;

    public CharacterMovement(ICharacter character) {
      rdb = character.GetRigidbody();
      transform = character.GetTransform();
    }

    // TODO: Do not use camera
    public void Move(Vector3 front, Vector3 side) {
    // public void Move(CameraManager cam, float frontIntensity, float sideIntensity) {
      Vector3 flatVelocity = rdb.velocity.xzOnly();
      float frontIntensity = front.magnitude;
      float sideIntensity = side.magnitude;
      /* Slow character down */
      if (frontIntensity == 0 && sideIntensity == 0) {
          return;
        // if (Mathf.Approximately(flatVelocity.magnitude, 0))

        // if (flatVelocity.magnitude < 0.005f)
        //   // rdb.velocity = rdb.velocity.yOnly();
        //   ;
        // else
        //   rdb.AddForce((Vector3.zero - flatVelocity).normalized * STATS.acceleration * 3f);
        // return;
      }

      // Vector3 movementDir = (cam.forward.xzOnly() * frontIntensity + cam.right.xzOnly() * sideIntensity).normalized;
      Vector3 movementDir = (front + side).xzOnly().normalized;

      /* Rotate character */

      /* Add force to required speed */
      if (flatVelocity != movementDir * STATS.maxFlatSpeed)
        rdb.AddForce((movementDir * STATS.maxFlatSpeed - flatVelocity).normalized * STATS.acceleration);
    }

    public void Rotate(Vector3 dir) {
      if (dir.magnitude == 0)
        return;
      transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir.xzOnly()), STATS.rotationSpeed);
    }


    public void Jump() {
      if (STATS.jumpIsInProgress && Time.fixedTime - STATS.jumpLastTimeExecuted > STATS.jumpDelay)
        STATS.jumpIsInProgress = false;
      if (!STATS.jumpIsInProgress) {
        STATS.jumpLastTimeExecuted = Time.fixedTime;
        STATS.jumpIsInProgress = true;
        rdb.AddForce(transform.up * STATS.jumpForce, ForceMode.VelocityChange);
        Debug.Log("Jump");
        return;
      }
      Debug.Log("Delay");
    }
  }
}
