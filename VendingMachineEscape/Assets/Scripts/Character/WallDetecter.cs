using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using InteractiveObjects;

namespace MainCharacter {
  public class WallDetecter : MonoBehaviour, ILogEnabled {
    public bool logOn => true; // ILogEnabled
    public Wall activeWall = null;
    private uint wallsCount = 0;

    private LinkedList<Wall> activeWallsQueue = new LinkedList<Wall>(); 
    

    private void OnTriggerEnter(Collider other) {
      if (other.tag != "Wall")
        return;
      Wall triggeredWall = other.GetComponent<Wall>();

      activeWallsQueue.AddLast(triggeredWall);
      wallsCount++;
      // if (activeWall == null) 
        activeWall = triggeredWall;
      DebugExt.Log(this, $"{triggeredWall.name} detected");
    }

    private void OnTriggerStay(Collider other) {
      if (other.tag != "Wall")
        return;
      Debug.DrawLine(transform.position, activeWall.location, Color.blue);
    }

    private void OnTriggerExit(Collider other) {
      if (other.tag != "Wall")
        return;
      Wall triggeredWall = other.GetComponent<Wall>();
      if (activeWallsQueue.Remove(triggeredWall))
        if (activeWallsQueue.Count != 0) {
          activeWall = activeWallsQueue.First.Value;
          return;
          }
        activeWall = null;
    }

    public bool IsWallDetected() {
      return activeWall != null;
    }

    public CompDir GetWallOrientation(){
      if (activeWall == null)
        return CompDir.zero;


      CompDir orientation = new CompDir(activeWall.normal, activeWall.tangent);
      Vector3 targetDir = transform.position - activeWall.location;

      DebugExt.Log(this, $"{activeWall.name}: angle={Vector3.Angle(activeWall.normal, targetDir)}");
      if (Vector3.Angle(activeWall.normal, targetDir) > 90f)
        return -orientation;
      return orientation;
    }
  }
}
