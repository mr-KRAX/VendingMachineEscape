using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game {
  public class CameraManager : MonoBehaviour {
    static private CameraManager _instance;
    static public CameraManager CM { get => _instance; }


    /// <summary>
    /// Camera components for configuration
    /// </summary>
    private CinemachineVirtualCamera cVirtualCam;
    private CinemachineTransposer cTransposer;
    private CinemachineComposer cComposer;

    /// <summary>
    /// 2D camera orientation (from top view)
    /// </summary>
    private Vector3 forward2D = new Vector3(-1, 0, 1);
    private Vector3 right2D = new Vector3(1, 0, 1);

    /// <summary>
    /// Camera rotation variables for smooth transition
    /// </summary>
    private Vector3 newFollowOffset;
    private Vector3 newForward2D;
    private Vector3 newRight2D;
    private float initialHorizontalDamping;
    private int rotationDirection; // 1 - clockwise, -1 - counterclockwise
    private float rotationSpeed = 1.5f; // degrees per update;
    private bool rotationInProgress = false;

    private void Start() {
      if (!_instance)
        _instance = this;

      cVirtualCam = GetComponent<CinemachineVirtualCamera>();
      cTransposer = cVirtualCam.GetCinemachineComponent<CinemachineTransposer>();
      cComposer = cVirtualCam.GetCinemachineComponent<CinemachineComposer>();
      initialHorizontalDamping = cComposer.m_HorizontalDamping;
    }

    private void Update() {
      if (rotationInProgress)
        AdjustCamRotation();
    }

    private void AdjustCamRotation() {
      if (cTransposer.m_FollowOffset == newFollowOffset) {
        forward2D = newForward2D;
        right2D = newRight2D;

        rotationInProgress = false;
        cComposer.m_HorizontalDamping = initialHorizontalDamping;
        return;
      }
      // cTransposer.m_FollowOffset = Vector3.LerpUnclamped(cTransposer.m_FollowOffset, newFollowOffset, 0.1f);
      Quaternion q = Quaternion.AngleAxis(rotationDirection * rotationSpeed, Vector3.up);
      cTransposer.m_FollowOffset = q * cTransposer.m_FollowOffset;
    }

    public void RotateCameraClockwise() {
      if (rotationInProgress)
        return;
      rotationDirection = 1;
      Quaternion q = Quaternion.AngleAxis(90, Vector3.up);
      newForward2D = q * forward2D;
      newRight2D = q * right2D;
      newFollowOffset = q * cTransposer.m_FollowOffset;
      cComposer.m_HorizontalDamping = 0;
      rotationInProgress = true;
    }

    public void RotateCameraCounterclockwise() {
      if (rotationInProgress)
        return;
      rotationDirection = -1;
      Quaternion q = Quaternion.AngleAxis(-90, Vector3.up);
      newForward2D = q * forward2D;
      newRight2D = q * right2D;
      newFollowOffset = q * cTransposer.m_FollowOffset;

      cComposer.m_HorizontalDamping = 0;
      rotationInProgress = true;
    }

    public Vector3 Forward2D { get => forward2D; }
    public Vector3 Right2D { get => right2D; }
  }
}
