using UnityEngine;
using MainCharacter;

namespace Game {
  public class GameManager : MonoBehaviour {
    // static public readonly  character;
    static private GameManager _instance;
    static public GameManager GM { get => _instance; }
    static public InputManager IM { get => _instance.inputManager; }
    static public CameraManager CM { get => _instance.cameraManager; }

    [SerializeField]
    private InputManager inputManager = null;
    [SerializeField]
    private CameraManager cameraManager = null;


    private ICharacter activeCharacter = null;
    public ICharacter ActiveCharacter { get => activeCharacter; }

    private void Awake() {
      if (!_instance)
        _instance = this;
    }

    public void SetActiveCharacter(ICharacter ch) {
      if (activeCharacter != null)
        activeCharacter.Deactivate();
      activeCharacter = ch;
      activeCharacter.Activate();
    }
  }
}