using UnityEngine;
using General;
using static Game.CameraManager;

namespace MainCharacter {
  public class CharacterMovement : ILogEnabled {
    public bool logOn => false; // ILogEnabled
    private Rigidbody rigidbody;
    private Transform transform;
    private WallDetecter wallDetecter;

    private float acceleration;
    private float maxFlatSpeed;
    private float rotationSpeed;

    public float jumpForce;
    public float jumpDelay;
    public float jumpLastTimeExecuted;
    public bool jumpIsInProgress;

    public CharacterMovement(Rigidbody rb, WallDetecter wd, CharacterStats stats) {
      rigidbody = rb;
      transform = rb.transform;
      wallDetecter = wd;

      // Import stats
      acceleration = stats.acceleration;
      maxFlatSpeed = stats.maxFlatSpeed;
      rotationSpeed = stats.rotationSpeed;

      jumpForce = stats.jumpForce;
      jumpDelay = stats.jumpDelay;
    }

    public void Update() {
      if (Input.GetKeyDown("space") && IsGrounded())
        Jump();
    }

    public void FixedUpdate() {
      Vector3 fwdForce = CM.Forward2D * Input.GetAxis("Vertical");
      Vector3 sideForce = CM.Right2D * Input.GetAxis("Horizontal");
      Vector3 characterOrientation;


      if (wallDetecter.IsWallDetected())
        characterOrientation = wallDetecter.GetWallOrientation().fwd;
      else
        characterOrientation = (fwdForce + sideForce).normalized;

      Move(fwdForce, sideForce);
      Rotate(characterOrientation);
    }


    private void Move(Vector3 frontForce, Vector3 sideForce) {
      Vector3 flatVelocity = rigidbody.velocity.xzOnly();
      float frontIntensity = frontForce.magnitude;
      float sideIntensity = sideForce.magnitude;
      Vector3 movementDir = (frontForce + sideForce).xzOnly().normalized;

      /* Add force to required speed */
      if ((frontForce + sideForce).xzOnly().magnitude == 0)
        return;
      if (flatVelocity != movementDir * maxFlatSpeed)
        rigidbody.AddForce((movementDir * maxFlatSpeed - flatVelocity).normalized * acceleration);
    }
    private void Rotate(Vector3 dir) {
      if (dir.magnitude == 0)
        return;

      transform.rotation = Quaternion.Slerp(transform.rotation,
                                            Quaternion.LookRotation(dir.xzOnly()),
                                            rotationSpeed);
    }
    private bool IsGrounded() {
      RaycastHit hit;
      float distance = 1.5f;
      Vector3 dir = new Vector3(0, -1);
      bool isGrounded = Physics.Raycast(transform.position, dir, out hit, distance);

      Debug.DrawRay(transform.position, dir * distance, Color.red);

      return isGrounded;
    }
    private void Jump() {
      if (jumpIsInProgress && (Time.fixedTime - jumpLastTimeExecuted) > jumpDelay)
        jumpIsInProgress = false;

      if (!jumpIsInProgress) {
        jumpLastTimeExecuted = Time.fixedTime;
        jumpIsInProgress = true;
        rigidbody.AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
        return;
      }
    }
  }
}
