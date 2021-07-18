using System.Collections;
using UnityEngine;
using General;
using static Game.GameManager;
using static UI.UIManager;


namespace MainCharacter {
  public class CharacterBattery : ILogEnabled {
    public bool logOn => true; // ILogEnabled
    private Callback onBatteryEmpty;
    public int Level { get => level; }
    private int level;
    private int maxBatteryLevel;
    private float dischargingSpeed;
    private float chargingSpeed;

    public CharacterBattery(Callback cb, CharacterStats stats) {
      onBatteryEmpty = cb;

      level = stats.maxBatteryLevel;
      maxBatteryLevel = stats.maxBatteryLevel;
      dischargingSpeed = stats.dischargingSpeed;
      chargingSpeed = stats.chargingSpeed;
      

      UIM.SetMaxBatteryLevel(maxBatteryLevel);
      UIM.SetBatteryLevel(level);
    }

    public IEnumerator Charging() {
      DebugExt.Log(this, "Charging started");
      while (Level < maxBatteryLevel) {
        yield return new WaitForSeconds(1 / chargingSpeed);
        UIM.SetBatteryLevel(++level);
      }
    }

    public IEnumerator Discharging() {
      DebugExt.Log(this, "Discharging started");
      while (Level > 0) {
        yield return new WaitForSeconds(1 / dischargingSpeed);
        UIM.SetBatteryLevel(--level);
      }

      DebugExt.Log(this, "Battery discharged");
      onBatteryEmpty();
    }
  }
}