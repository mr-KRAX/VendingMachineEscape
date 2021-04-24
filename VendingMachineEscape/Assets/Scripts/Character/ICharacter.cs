using UnityEngine;

namespace MainCharacter {
  public interface ICharacter {
    Rigidbody GetRigidbody();
    Transform GetTransform();
    void ProcessJump();
    void Activate();
    void Deactivate();
  }
}