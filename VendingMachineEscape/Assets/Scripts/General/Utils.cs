using UnityEngine;

namespace General {

  static class Vector3Ext {
    public static Vector3 xzOnly(this Vector3 v) {
      return new Vector3(v.x, 0, v.z);
    }

    public static Vector3 yOnly(this Vector3 v){
      return new Vector3(0, v.y, 0);
    }
  }
}