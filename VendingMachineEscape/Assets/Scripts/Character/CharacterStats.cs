using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainCharacter {
  [CreateAssetMenu(fileName = "New CharacterStats")]
  public class CharacterStats : ScriptableObject {
    [Header("Flat movement")]
    [SerializeField] float MaxFlatSpeed = 4f;
    [SerializeField] float Acceleration = 20f;
    [SerializeField] float RotationSpeed = 0.1f;

    [Header("Jumping")]
    [SerializeField] float JumpForce = 5f;
    [SerializeField] float JumpDelay = 0.5f;

    [Header("Battery")]
    [SerializeField] int MaxBatteryLevel = 10;
    [SerializeField] float ChargingSpeed = 2f;
    [SerializeField] float DischargingSpeed = 0.5f;



    #region FlatMovement
    public float maxFlatSpeed { get => MaxFlatSpeed; }
    public float acceleration { get => Acceleration; }
    public float rotationSpeed { get => RotationSpeed; }
    #endregion

    #region Jumping
    public float jumpForce { get => JumpForce; }
    public float jumpDelay { get => JumpDelay; }
    #endregion

    #region Battery
    public int maxBatteryLevel { get => MaxBatteryLevel; }
    public float chargingSpeed { get => ChargingSpeed; }
    public float dischargingSpeed { get => DischargingSpeed; }
    #endregion
  }
}
