using UnityEngine;
using UnityEngine.UI;

namespace UI {
  public class UIManager : MonoBehaviour {
    static private UIManager _instance;
    static public UIManager UIM { get => _instance; }

    [SerializeField] 
    private Slider batteryBar = null;

    public void SetMaxBatteryLevel(int level) {
      batteryBar.maxValue = level;
    }
    public void SetBatteryLevel(int level) {
      batteryBar.value = level;
    }
    private void Awake() {
      if (!_instance)
        _instance = this;
    }
  }

}
