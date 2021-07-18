using System.Collections.Generic;
using UnityEngine;

namespace General {
  public delegate void Callback();

  static class Vector3Ext {
    public static Vector3 xzOnly(this Vector3 v) {
      return new Vector3(v.x, 0, v.z);
    }

    public static Vector3 yOnly(this Vector3 v) {
      return new Vector3(0, v.y, 0);
    }

    public static Vector2 Rotate(this Vector2 p, float angle) {
      float cos = Mathf.Cos(angle);
      float sin = Mathf.Sin(angle);
      return new Vector2(p.x * cos - p.y * sin, p.x * sin + p.y * cos);
    }
  }

  public interface ILogEnabled {
    bool logOn {get;}
  }
  public class DebugExt : Debug {
    public static Dictionary<System.Type, bool> list;
    public static void Log(ILogEnabled caller, object message) {
      if(caller.logOn)
        Debug.Log($"[{caller.GetType()}]: " + message);
    }
  }

  // Compound direction
  public struct CompDir {
    public Vector3 fwd;
    public Vector3 side;
    public Vector3 vert;

    public CompDir(Vector3 fwd, Vector3 side, Vector3 vert) {
      this.fwd = fwd;
      this.side = side;
      this.vert = vert;
    }

    public CompDir(Vector3 fwd, Vector3 side) {
      this.fwd = fwd;
      this.side = side;
      this.vert = Vector3.zero;
    }

    public static CompDir zero {
      get {
        CompDir zero = new CompDir();
        zero.vert = zero.side = zero.fwd = Vector3.zero;
        return zero;
      }
    }

    public static CompDir operator -(CompDir a) {
      return new CompDir(-a.fwd, -a.side, -a.vert);
    }

    public static bool operator ==(CompDir lhs, CompDir rhs) {
      return lhs.fwd == rhs.fwd &&
             lhs.side == rhs.side &&
             lhs.vert == rhs.vert;
    }

    public static bool operator !=(CompDir lhs, CompDir rhs) {
      return !(lhs == rhs);
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        //
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //
        
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        
        // TODO: write your implementation of Equals() here
        // throw new System.NotImplementedException();
        return base.Equals (obj);
    }
    
    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        // throw new System.NotImplementedException();
        return base.GetHashCode();
    }
  }
}