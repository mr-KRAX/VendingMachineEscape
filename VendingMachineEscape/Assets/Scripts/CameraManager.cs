using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Cam {
  public class CameraManager : MonoBehaviour {
    private CinemachineFreeLook cCam;
    private CinemachineCameraOffset cCamOffset;
    private float ccOffsetNew;
    private bool ccOffsetIsChangeing = false;
    private float ccOffestChangeSpeed = 10f;

    private void Start() {
      cCam = GetComponent<CinemachineFreeLook>();
      cCamOffset = GetComponent<CinemachineCameraOffset>();
    }

    private void Update() {
      if (ccOffsetIsChangeing){
        if (ccOffsetNew == cCamOffset.m_Offset.x){
          ccOffsetIsChangeing = false;
          return;
        }
        float velocity = 0;
        cCamOffset.m_Offset.x = Mathf.SmoothDamp(cCamOffset.m_Offset.x, ccOffsetNew, ref velocity, 0.05f, 300);
      }
     }

    public void adjustCameraRotation(float angle) {
      float velocity = 0;
      float newAngle = Mathf.SmoothDampAngle(cCam.m_XAxis.Value, angle, ref velocity, 0.1f, 300);
      cCam.m_XAxis.Value = newAngle;
    }

    public void ChangeLookSide() {
      ccOffsetNew = -cCamOffset.m_Offset.x;
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
