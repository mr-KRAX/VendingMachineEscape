using UnityEngine;

namespace MainCharacter {
  public interface ICharacter {
    Rigidbody GetRigidbody();
    Transform GetTransform();
  }
}