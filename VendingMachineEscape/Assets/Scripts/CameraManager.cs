using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Cam {
  public class CameraManager : MonoBehaviour {
    private CinemachineFreeLook cinemachineCam;

    private void Start() {
      cinemachineCam = GetComponent<CinemachineFreeLook>();
    }

    private void Update() { }

    public void adjustCameraRotation(float angle) {
      float velocity = 0;
      float newAngle = Mathf.SmoothDampAngle(cinemachineCam.m_XAxis.Value, angle, ref velocity, 0.1f, 300);
      cinemachineCam.m_XAxis.Value = newAngle;
    }

    public Vector3 forward {
      get { return Camera.main.transform.forward; }
    }

    public Vector3 right {
      get { return Camera.main.transform.right; }
    }
  }
}
