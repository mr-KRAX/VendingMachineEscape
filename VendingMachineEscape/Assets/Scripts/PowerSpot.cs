using UnityEngine;
using MainCharacter;

namespace InteractiveObjects {
  public class PowerSpot : MonoBehaviour {
    public GameObject gfx;
    private void OnTriggerEnter(Collider other) {
      other.gameObject.GetComponent<Character>()?.StartCharging();
      gfx.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(61f/255, 255f/255, 70f/255, 0.5f));
    }

    private void OnTriggerExit(Collider other) {
      Debug.Log("Exited");
      other.gameObject.GetComponent<Character>()?.StopCharging();
      gfx.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(61f/255, 106f/255, 255f/255, 0.5f));
    }
  }
}
