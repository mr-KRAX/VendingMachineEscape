using UnityEngine;
using UnityEngine.UI;

namespace UI {
  public class UIManager : MonoBehaviour {
    [SerializeField] private Slider batteryBar;
    void Start() { }

    void Update() { }

    public void SetMaxBatteryLevel(int level) {
      batteryBar.maxValue = level;
    }
    public void SetBatteryLevel(int level) {
      batteryBar.value = level;
    }
  }

}
