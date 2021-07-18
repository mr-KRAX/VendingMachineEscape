using UnityEngine;
using MainCharacter;
using General;

namespace InteractiveObjects {
  public class PowerSpot : MonoBehaviour, ILogEnabled {
    public bool logOn => true;
    public Transform plague;
    public GameObject gfx;
    private void OnTriggerEnter(Collider other) {
      if (other.tag == "Character") {
        gfx.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(61f / 255, 255f / 255, 70f / 255, 0.5f));
        plague.GetComponent<Rigidbody>().isKinematic = true;
        other.gameObject.GetComponent<Character>()?.StartCharging();
      }

    }

    private void OnTriggerStay(Collider other) {
      if (other.name == "Player") {
        plague.position = Vector3.Slerp(plague.position, transform.position, 0.1f);
        plague.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-plague.forward), 0.1f);

      }
    }

    private void OnTriggerExit(Collider other) {
      DebugExt.Log(this, "Character left charging zone");
      if (other.name == "Player") {
        gfx.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(61f / 255, 106f / 255, 255f / 255, 0.5f));
        // gfx.GetComponent<MeshRenderer>().material.SetColor("_Color", new Color(61f/255, 255f/255, 70f/255, 0.5f));
        plague.GetComponent<Rigidbody>().isKinematic = false;
        // plague.position = Vector3.Lerp(plague.position, plague.position, 1);
        other.gameObject.GetComponent<Character>()?.StopCharging();
      }
    }
  }
}
