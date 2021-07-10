using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InteractiveObjects {
  public class Wall : MonoBehaviour {
    [SerializeField]
    private Vector3 Tangent, Vertical;
    
    public Vector3 normal {get; private set;}
    public Vector3 tangent {get; private set;}
    public Vector3 location {get => transform.position;}

    private void Start() {
      if (Tangent == Vector3.zero) 
        normal = transform.forward;
      if (Vertical == Vector3.zero) 
        tangent = transform.right;
    }

    private void OnDrawGizmos() {
      Start();
      Vector3 centre = transform.position;// + new Vector3(0, 0.5f, 0.5f);
      Debug.DrawRay(centre, normal, Color.magenta);
      Debug.DrawRay(centre, tangent, Color.cyan);
    }

  }
}
