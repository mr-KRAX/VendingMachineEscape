using UnityEngine;
using Cam;
using UI;


namespace Game {
  public class GameManager : MonoBehaviour {
    // static public readonly  character;
    static private GameManager _instance = null;
    [SerializeField] CameraManager CameraManager;
    [SerializeField] UIManager UIManager;
    public CameraManager cameraManager {get => CameraManager;}
    public UIManager ui {get => UIManager;}

    private void Awake() {
      if (!_instance)
        _instance = this;
      DontDestroyOnLoad(_instance);
      DontDestroyOnLoad(cameraManager);
    }

    public static GameManager GetInstance() {
      return _instance;
    }
  }
}