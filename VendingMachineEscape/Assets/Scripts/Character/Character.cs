using System.Collections;
using UnityEngine;
using static Game.GameManager;
using static Game.CameraManager;
using InteractiveObjects;
using General;

namespace MainCharacter {
  public class Character : MonoBehaviour, ICharacter, ILogEnabled {
    public bool logOn => true; // ILogEnabled
    private bool isActive = true;

    [SerializeField]
    private CharacterStats characterStats = null;
    private CharacterMovement characterMovement;
    private CharacterBattery characterBattery;
    private IEnumerator batteryCoroutine;
    private Rigidbody rdb;
    private WallDetecter wallDetecter;

    private void Start() {
      DebugExt.Log(this, "Started");
      rdb = GetComponent<Rigidbody>();

      wallDetecter = gameObject.GetComponentInChildren<WallDetecter>();
      
      characterMovement = new CharacterMovement(rdb, wallDetecter, characterStats);
      characterBattery = new CharacterBattery(OnBatteryEmpty, characterStats);
      batteryCoroutine = characterBattery.Discharging();
      
      StartCoroutine(batteryCoroutine);

      GM.SetActiveCharacter(this);
    }

    private void OnBatteryEmpty() {
      Deactivate();
      DebugExt.Log(this, "Died: battery empty");
    }

    private void FixedUpdate() {
      if (!isActive)
        return;

      characterMovement?.FixedUpdate();
    }

    private void Update() {
      if (!isActive)
        return;
        
      characterMovement?.Update();
    }
    #region ICharacter

    public void Activate() {
      isActive = true;
    }

    public void Deactivate() {
      isActive = false;
      // rdb.velocity = Vector3.zero;
    }
    #endregion

    #region IBattery
    public void StartCharging() {
      StopCoroutine(batteryCoroutine);
      batteryCoroutine = characterBattery.Charging();
      StartCoroutine(batteryCoroutine);
    }

    public void StopCharging() {
      StopCoroutine(batteryCoroutine);
      batteryCoroutine = characterBattery.Discharging();
      StartCoroutine(batteryCoroutine);
    }
    #endregion
  }
}