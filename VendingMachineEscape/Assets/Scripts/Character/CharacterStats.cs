using UnityEngine;

namespace MainCharacter {
  public class CharacterStats : MonoBehaviour {
    static private CharacterStats _instance = null;

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
    public float maxFlatSpeed {get => MaxFlatSpeed;} 
    public float acceleration {get => Acceleration;} 
    public float rotationSpeed {get => RotationSpeed;} 
    #endregion

    #region Jumping
    public float jumpForce {get => JumpForce;} 
    public float jumpDelay {get => JumpDelay;} 
    public float jumpLastTimeExecuted;
    public bool jumpIsInProgress = false; 

    #endregion

    #region Battery
    public int maxBatteryLevel {get => MaxBatteryLevel;}
    public float chargingSpeed {get => ChargingSpeed;}
    public float dischargingSpeed {get => DischargingSpeed;}
    public int currBatteryLevel;

    #endregion
    
    
    private CharacterStats() { }

    private void Awake() {
      if (!_instance)
        _instance = this;
      DontDestroyOnLoad(_instance);
    }

    static public CharacterStats GetInstance() {
      return _instance;
    }


  }
}