using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game {
  public class CameraManager : MonoBehaviour {
    static private CameraManager _instance;
    static public CameraManager CM { get => _instance; }

    private CinemachineFreeLook cCam;
    private CinemachineCameraOffset cCamOffset;
    private float ccOffsetNew;
    private float ccOffsetLimit;
    private bool ccOffsetIsChangeing = false;
    private float ccOffestChangeSpeed = 300f;

    private void Awake() {
      if (!_instance)
        _instance = this;
    }

    private void Start() {
      cCam = GetComponent<CinemachineFreeLook>();
      cCamOffset = GetComponent<CinemachineCameraOffset>();
      ccOffsetLimit = Mathf.Abs(cCamOffset.m_Offset.x);
    }

    private void Update() {
      if (ccOffsetIsChangeing) {
        if (Mathf.Approximately(ccOffsetNew, cCamOffset.m_Offset.x)) {
          cCamOffset.m_Offset.x = ccOffsetNew;
          ccOffsetIsChangeing = false;
          return;
        }
        float velocity = 0;
        cCamOffset.m_Offset.x = Mathf.SmoothDamp(cCamOffset.m_Offset.x, ccOffsetNew, ref velocity, 0.05f, ccOffestChangeSpeed);
      }
    }

    public void adjustCameraRotation(float angle) {
      float velocity = 0;
      float newAngle = Mathf.SmoothDampAngle(cCam.m_XAxis.Value, angle, ref velocity, 0.1f, 300);
      cCam.m_XAxis.Value = newAngle;
    }

    public void ChangeLookSide() {
      ccOffsetNew = -Mathf.Sign(cCamOffset.m_Offset.x) * ccOffsetLimit;
      ccOffsetIsChangeing = true;
    }

    public Vector3 forward {
      get { return Camera.main.transform.forward; }
    }

    public Vector3 right {
      get { return Camera.main.transform.right; }
    }
  }
}
