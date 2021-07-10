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
  }
}