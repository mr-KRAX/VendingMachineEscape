using UnityEngine;
using static Game.GameManager;
using static Game.CameraManager;

namespace Game {
  public class InputManager : MonoBehaviour {
    static private InputManager _instance;
    static public InputManager IM { get => _instance; }

    private void Awake() {
      if (!_instance)
        _instance = this;
    }
    private void Update() {
      if (Input.GetKeyUp("q"))
        CM.ChangeLookSide();
      if (Input.GetKeyDown("space"))
        GM.ActiveCharacter.ProcessJump();
    }

    public float GetVertical() {
      return Input.GetAxis("Vertical");
    }
    public float GetHorizontal() {
      return Input.GetAxis("Horizontal");
    }
  }
}