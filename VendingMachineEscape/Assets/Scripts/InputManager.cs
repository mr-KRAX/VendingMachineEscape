using UnityEngine;
using static Game.GameManager;
// using static Game.CameraManager;

namespace Game {
  public class InputManager : MonoBehaviour {
    private void Update() {
      if (Input.GetKeyUp("q"))
        CM.RotateCameraClockwise();
      if (Input.GetKeyUp("e"))
        CM.RotateCameraCounterclockwise();
    }

    public float GetVertical() {
      return Input.GetAxis("Vertical");
    }
    public float GetHorizontal() {
      return Input.GetAxis("Horizontal");
    }
  }
}