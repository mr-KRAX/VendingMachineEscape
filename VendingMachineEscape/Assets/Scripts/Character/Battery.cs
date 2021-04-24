using System.Collections;
using UnityEngine;
using General;
using static Game.GameManager;
using static MainCharacter.CharacterStats;
using static UI.UIManager;


namespace MainCharacter {
  public class Battery {
    public int Level { get; private set; }
    private IEnumerator batteryCoroutine = null;
    private Callback onBatteryEmpty;

    public Battery(Callback cb) {
      onBatteryEmpty = cb;
      Level = STATS.maxBatteryLevel;
      UIM.SetMaxBatteryLevel(STATS.maxBatteryLevel);
      UIM.SetBatteryLevel(Level);
      batteryCoroutine = DischargeBattery();
    }

    private IEnumerator ChargeBattery() {
      Debug.Log("Charging");
      while (Level < STATS.maxBatteryLevel) {
        yield return new WaitForSeconds(1 / STATS.chargingSpeed);
        UIM.SetBatteryLevel(++Level);
      }
    }

    private IEnumerator DischargeBattery() {
      Debug.Log("Discharging");
      while (Level > 0) {
        yield return new WaitForSeconds(1 / STATS.dischargingSpeed);
        UIM.SetBatteryLevel(--Level);
      }
      Debug.Log("Empty");
      onBatteryEmpty();
    }

    public void StartDischarging() {
      if (batteryCoroutine != null)
        GM.StopCoroutine(batteryCoroutine);
      batteryCoroutine = DischargeBattery();
      GM.StartCoroutine(batteryCoroutine);
    }

    public void StartCharging() {
      GM.StopCoroutine(batteryCoroutine);
      batteryCoroutine = ChargeBattery();
      GM.StartCoroutine(batteryCoroutine);
    }
  }
}