using UnityEngine;

namespace General {
  public class InputManager : MonoBehaviour {
    static public float GetVertical() {
      return Input.GetAxis("Vertical");
    }

    static public float GetHorizontal() {
      return Input.GetAxis("Horizontal");
    }
  }
}