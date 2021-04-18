using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

  public class BatteryBar : MonoBehaviour {
      private Slider slider;

      private void Start() {
          slider = GetComponent<Slider>();
      }

  }

}
