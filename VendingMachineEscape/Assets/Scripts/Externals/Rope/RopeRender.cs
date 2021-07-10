// using Multiplayer;
using General;
using System;
using UnityEngine;

namespace HumanAPI {
  public class RopeRender : MonoBehaviour {
    public int meshSegments = 20;

    public int segmentsAround = 6;

    public float radius = 0.1f;

    private Vector2[] rotatedRadius;

    private Mesh mesh;

    private MeshFilter mf;

    private Vector3[] vertices;

    private bool forceUpdate;

    public bool visible;

    public bool isDirty;

    public virtual void OnEnable() {
      int rings = meshSegments + 1;
      vertices = new Vector3[rings * segmentsAround + 2 * segmentsAround];
      int[] triangles = new int[meshSegments * segmentsAround * 6 + 6 * (segmentsAround - 2)];
      int vNum = 0;
      for (int i = 0; i < rings - 1; i++) {
        for (int j = 0; j < segmentsAround; j++) {
          int v_currSeg = i * segmentsAround;
          int v_nextSeg = v_currSeg + segmentsAround;
          int v_nextPiece = (j + 1) % segmentsAround;
          int v0 = v_currSeg + j;
          int v1 = v_currSeg + v_nextPiece;
          int v2 = v_nextSeg + j;
          int v3 = v_nextSeg + v_nextPiece;
          triangles[vNum++] = v0;
          triangles[vNum++] = v1;
          triangles[vNum++] = v2;
          triangles[vNum++] = v2;
          triangles[vNum++] = v1;
          triangles[vNum++] = v3;
        }
      }

			// back and front caps, to close mesh 
      int num10 = rings * segmentsAround;
      for (int k = 0; k < segmentsAround - 2; k++) {
        triangles[vNum++] = num10;
        triangles[vNum++] = num10 + k + 2;
        triangles[vNum++] = num10 + k + 1;
      }
      int num11 = (rings + 1) * segmentsAround;
      for (int l = 0; l < segmentsAround - 2; l++) {
        triangles[vNum++] = num11;
        triangles[vNum++] = num11 + l + 1;
        triangles[vNum++] = num11 + l + 2;
      }
      
			mesh = new Mesh();
      mesh.name = "rope " + base.name;
      mesh.vertices = vertices;
      mesh.triangles = triangles;
      mf = GetComponent<MeshFilter>();
      rotatedRadius = new Vector2[segmentsAround];
      for (int m = 0; m < segmentsAround; m++) {
        rotatedRadius[m] = new Vector2(radius, 0f).Rotate((float)Math.PI * 2f * (float)m / (float)segmentsAround);
      }
			mf.mesh = mesh;
    }

    protected void ForceUpdate() {
      forceUpdate = true;
    }

    public virtual void LateUpdate() {
			// return ;
      if (!isDirty) {
        CheckDirty();
      }
      if (!forceUpdate && (!visible || !isDirty)) {
        return;
      }
      forceUpdate = false;
      ReadData();
      float lod = 1f;
      int idx = 0;
      int num = (int)((float)meshSegments / lod);
      int num2 = num + 1;
      for (int i = 0; i < num2; i++) {
        UpdateRing(1f * (float)i / (float)num, ref idx);
      }
      for (int j = 0; j < meshSegments - num; j++) {
        for (int k = 0; k < segmentsAround; k++) {
          vertices[idx++] = vertices[idx - segmentsAround];
        }
      }
      UpdateRing(0f, ref idx);
      UpdateRing(1f, ref idx);
      mesh.vertices = vertices;
      mesh.RecalculateNormals();
      mesh.RecalculateBounds();
      mf.sharedMesh = mesh;
      isDirty = false;
    }

    private void UpdateRing(float t, ref int idx) {
      GetPoint(t, out Vector3 pos, out Vector3 normal, out Vector3 binormal);
      for (int i = 0; i < segmentsAround; i++) {
        Vector2 vector = rotatedRadius[i];
        Vector3 vector2 = pos + vector.x * normal + vector.y * binormal;
        vertices[idx++] = vector2;
      }
    }

    public virtual void GetPoint(float dist, out Vector3 pos, out Vector3 normal, out Vector3 binormal) {
      pos = (normal = (binormal = Vector3.zero));
    }

    public void OnBecameInvisible() {
      visible = false;
    }

    public void OnBecameVisible() {
      visible = true;
    }

    public virtual void CheckDirty() {
    }

    public virtual void ReadData() {
    }
  }
}
