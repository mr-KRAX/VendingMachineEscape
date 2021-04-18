using UnityEngine;
using Game;

namespace General {
  public class InputManager : MonoBehaviour {
    private GameManager gm;

    private void Start() {
      gm = GameManager.GetInstance();
    }
    static public float GetVertical() {
      return Input.GetAxis("Vertical");
    }
    static public float GetHorizontal() {
      return Input.GetAxis("Horizontal");
    }
    private void Update() {
      if (Input.GetKeyUp("q"))
        gm.cameraManager.ChangeLookSide();
    }
  }
}