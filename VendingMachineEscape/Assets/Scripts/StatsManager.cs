using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
  public class StatsManager : MonoBehaviour {
    static private StatsManager _instance;

    private void Awake() {
      if (!_instance)
        _instance = this;
    }
  }
}
