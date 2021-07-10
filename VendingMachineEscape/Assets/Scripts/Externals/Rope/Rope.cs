// using Multiplayer;
using UnityEngine;

namespace HumanAPI {
  public class Rope : RopeRender {
    [Tooltip("A list of points to use in connection with the rope")]
    public Transform[] handles;

    [Tooltip("Whether or not the start  of the rope should be a fixed location")]
    public bool fixStart;

    [Tooltip("Whether or not hte end of the rope should be a fixed location")]
    public bool fixEnd;

    [Tooltip("The start joint is locked")]
    public bool fixStartDir;

    [Tooltip("The end joint is fixed")]
    public bool fixEndDir;

    [Tooltip("Start location for the Rope via a connected Rigid body")]
    public Rigidbody startBody;

    [Tooltip("End location for the Rope via a connected Rigid body")]
    public Rigidbody endBody;

    [Tooltip("How much to divide up the rope")]
    public int rigidSegments = 10;

    [Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
    public float segmentMass = 20f;

    [Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
    public float lengthMultiplier = 1f;

    [Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
    public PhysicMaterial ropeMaterial;

    protected Transform[] bones;

    // protected NetBodySleep[] boneSleep;

    [Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
    public Vector3[] bonePos;

    [Tooltip("Use_this_in_order_to_print_debug_info_to_the_Log")]
    public Vector3[] boneRot;

    [Tooltip("Print stuff from this script to the Log")]
    public bool showDebug;

    private bool initialized = false;

    private Vector3[] boneForward;

    private Vector3[] boneRight;

    private Vector3[] boneUp;

    protected Vector3[] originalPositions;

    protected float boneLen;

    // private NetStream originalState;

    // private NetVector3Encoder posEncoder = new NetVector3Encoder(500f, 18, 4, 8);

    // private NetVector3Encoder diffEncoder = new NetVector3Encoder(3.90625f, 11, 3, 7);

    // private NetQuaternionEncoder rotEncoder = new NetQuaternionEncoder(9, 4, 6);

    public virtual Vector3[] GetHandlePositions() {
      if (showDebug) {
        Debug.Log(base.name + " Get Handle positions ");
      }
      Vector3[] array = new Vector3[handles.Length];
      for (int i = 0; i < handles.Length; i++) {
        array[i] = handles[i].position;
      }
      return array;
    }

    public override void OnEnable() {
      EnsureInitialized();
    }

    private void EnsureInitialized() {
      if (!initialized) {
        initialized = true;
        Initialize();
      }
    }

    private void Initialize() {
      if (showDebug) {
        Debug.Log(base.name + " OnEnable ");
      }

      Vector3[] handlePositions = GetHandlePositions();
      if (handlePositions.Length < 2)
        return;
      Vector3 begHandle = handlePositions[0];
      Vector3 endHandle = handlePositions[handlePositions.Length - 1];
      float length = 0f;
      for (int i = 0; i < handlePositions.Length - 1; i++) {
        length += (handlePositions[i] - handlePositions[i + 1]).magnitude;
      }
      length *= lengthMultiplier;
      boneLen = length / (float)rigidSegments;
      bones = new Transform[rigidSegments];
      boneRight = new Vector3[rigidSegments];
      boneUp = new Vector3[rigidSegments];
      originalPositions = new Vector3[bones.Length];
      boneForward = new Vector3[rigidSegments];

      Vector3 a = (endHandle - begHandle) / rigidSegments;
      Quaternion rotation = Quaternion.LookRotation(a.normalized);
      Vector3 zero = Vector3.zero;
			// Setting up rigid segments (bones)
      for (int j = 0; j < rigidSegments; j++) {
        zero = begHandle + a * (0.5f + (float)j);
        GameObject go = base.gameObject;
        go = new GameObject("bone" + j);
        go.transform.SetParent(base.transform, worldPositionStays: true);
        originalPositions[j] = zero;
        go.transform.position = zero;
        go.transform.rotation = rotation;
        bones[j] = go.transform;
        go.tag = "Target";
        go.layer = 14;
        Rigidbody rdb = go.AddComponent<Rigidbody>();
        rdb.mass = segmentMass;
        rdb.drag = 0.1f;
        rdb.angularDrag = 0.1f;
        CapsuleCollider col = go.AddComponent<CapsuleCollider>();
        col.direction = 2;
        col.radius = radius;
        col.height = boneLen + radius * 2f;
        col.sharedMaterial = ropeMaterial;
        if (j != 0) {
          ConfigurableJoint joint = go.AddComponent<ConfigurableJoint>();
          joint.connectedBody = bones[j - 1].GetComponent<Rigidbody>();
          ConfigurableJointMotion jointMotion = joint.zMotion = ConfigurableJointMotion.Locked;
          jointMotion = (joint.xMotion = (joint.yMotion = jointMotion));
          jointMotion = (joint.angularZMotion = ConfigurableJointMotion.Limited);
          jointMotion = (joint.angularXMotion = (joint.angularYMotion = jointMotion));

          joint.angularXLimitSpring = new SoftJointLimitSpring {
            spring = 100f,
            damper = 10f
          };
          joint.angularYZLimitSpring = new SoftJointLimitSpring {
            spring = 100f,
            damper = 10f
          };
          joint.lowAngularXLimit = new SoftJointLimit {
            limit = -20f
          };
          joint.highAngularXLimit = new SoftJointLimit {
            limit = 20f
          };
          joint.angularYLimit = new SoftJointLimit {
            limit = 20f
          };
          joint.angularZLimit = new SoftJointLimit {
            limit = 20f
          };
          joint.angularXDrive = new JointDrive {
            positionSpring = 50f
          };
          joint.angularYZDrive = new JointDrive {
            positionSpring = 50f
          };
          joint.axis = new Vector3(0f, 0f, 1f);
          joint.secondaryAxis = new Vector3(1f, 0f, 0f);
          joint.autoConfigureConnectedAnchor = false;
          joint.anchor = new Vector3(0f, 0f, (0f - boneLen) / 2f);
          joint.connectedAnchor = new Vector3(0f, 0f, boneLen / 2f);
          joint.projectionMode = JointProjectionMode.PositionAndRotation;
          joint.projectionDistance = 0.02f;
        }
      }
      
			float num2 = (0f - boneLen) / 2f / lengthMultiplier;
      int num3 = -1;
      zero = Vector3.zero;
      Vector3 vector3 = Vector3.zero;
      for (int k = 0; k < rigidSegments; k++) {
        Vector3 vector4;
        for (; num2 <= 0f; num2 += vector4.magnitude) {
          num3++;
          vector4 = handlePositions[num3 + 1] - handlePositions[num3];
          vector3 = vector4.normalized;
          rotation = Quaternion.LookRotation(vector3);
          zero = handlePositions[num3] - num2 * vector3;
        }
        bones[k].transform.position = zero;
        bones[k].transform.rotation = rotation;
        zero += vector3 * boneLen / lengthMultiplier;
        num2 -= boneLen / lengthMultiplier;
      }
      if (fixStart && bones != null && bones.Length > 0) {
        if (showDebug) {
          Debug.Log(base.name + " Fix Start ");
        }
        ConfigurableJoint startJoint = bones[0].gameObject.AddComponent<ConfigurableJoint>();
        ConfigurableJointMotion jointMotion = startJoint.zMotion = ConfigurableJointMotion.Locked;
        jointMotion = (startJoint.xMotion = (startJoint.yMotion = jointMotion));
        startJoint.projectionMode = JointProjectionMode.PositionAndRotation;
        startJoint.projectionDistance = 0.02f;
        if (fixStartDir) {
          jointMotion = (startJoint.angularZMotion = ConfigurableJointMotion.Locked);
          jointMotion = (startJoint.angularXMotion = (startJoint.angularYMotion = jointMotion));
        }
        startJoint.anchor = new Vector3(0f, 0f, (0f - boneLen) / 2f);
        startJoint.autoConfigureConnectedAnchor = false;
        if (startBody != null) {
          startJoint.connectedBody = startBody;
          startJoint.connectedAnchor = startBody.transform.InverseTransformPoint(begHandle);
        } else {
          startJoint.connectedAnchor = begHandle;
        }
      }
      if (fixEnd) {
        if (showDebug) {
          Debug.Log(base.name + " Fix End ");
        }
        ConfigurableJoint endJoint = bones[bones.Length - 1].gameObject.AddComponent<ConfigurableJoint>();
        ConfigurableJointMotion jointMotion = endJoint.zMotion = ConfigurableJointMotion.Locked;
        jointMotion = (endJoint.xMotion = (endJoint.yMotion = jointMotion));
        endJoint.projectionMode = JointProjectionMode.PositionAndRotation;
        endJoint.projectionDistance = 0.02f;
        if (fixEndDir) {
          jointMotion = (endJoint.angularZMotion = ConfigurableJointMotion.Locked);
          jointMotion = (endJoint.angularXMotion = (endJoint.angularYMotion = jointMotion));
        }
        endJoint.anchor = new Vector3(0f, 0f, boneLen / 2f);
        endJoint.autoConfigureConnectedAnchor = false;
        if (endBody != null) {
          endJoint.connectedBody = endBody;
          endJoint.connectedAnchor = endBody.transform.InverseTransformPoint(endHandle);
        } else {
          endJoint.connectedAnchor = endHandle;
        }
      }
      
			if (bonePos == null || bonePos.Length != rigidSegments) {
        bonePos = new Vector3[rigidSegments];
        boneRot = new Vector3[rigidSegments];
      } else {
        for (int l = 0; l < rigidSegments; l++) {
          bones[l].transform.position = base.transform.TransformPoint(bonePos[l]);
          bones[l].transform.rotation = base.transform.rotation * Quaternion.Euler(boneRot[l]);
        }
      }
      base.OnEnable();
    }

    private void OnDestroy() {
      if (showDebug) {
        Debug.Log(base.name + " OnDestroy ");
      }
    }

    public override void CheckDirty() {
      base.CheckDirty();
      EnsureInitialized();
      Rigidbody component = bones[0].GetComponent<Rigidbody>();
      if (!component.IsSleeping() && !component.isKinematic) {
        isDirty = true;
      }
    }

    public override void ReadData() {
      for (int i = 0; i < rigidSegments; i++) {
        Quaternion lhs = Quaternion.Inverse(base.transform.rotation);
        bonePos[i] = base.transform.InverseTransformPoint(bones[i].position);
        Quaternion rotation = lhs * bones[i].rotation;
        boneRot[i] = rotation.eulerAngles;
        boneForward[i] = rotation * Vector3.forward;
        boneUp[i] = rotation * Vector3.up;
        boneRight[i] = rotation * Vector3.right;
      }
    }

    public override void GetPoint(float dist, out Vector3 pos, out Vector3 normal, out Vector3 binormal) {
      int num = Mathf.FloorToInt(dist * (float)rigidSegments - 0.5f);
      float num2 = dist * (float)rigidSegments - 0.5f - (float)num;
      int num3 = Mathf.Clamp(num, 0, rigidSegments - 1);
      int num4 = Mathf.Clamp(num + 1, 0, rigidSegments - 1);
      if (num == -1) {
        normal = boneRight[num3];
        binormal = boneUp[num3];
        pos = bonePos[num3] + boneForward[num3] * boneLen * (num2 - 1f);
        return;
      }
      if (num == rigidSegments - 1) {
        normal = boneRight[num3];
        binormal = boneUp[num3];
        pos = bonePos[num3] + boneForward[num3] * boneLen * num2;
        return;
      }
      float num5 = num2 * num2;
      float num6 = num5 * num2;
      float d = 2f * num6 - 3f * num5 + 1f;
      float d2 = num6 - 2f * num5 + num2;
      float d3 = -2f * num6 + 3f * num5;
      float d4 = num6 - num5;
      Vector3 a = boneForward[num3] * boneLen;
      Vector3 a2 = boneForward[num4] * boneLen;
      pos = d * bonePos[num3] + d2 * a + d3 * bonePos[num4] + d4 * a2;
      normal = d * boneRight[num3] + d3 * boneRight[num4];
      binormal = d * boneUp[num3] + d3 * boneUp[num4];
    }

    private void FixedUpdate() {
      EnsureInitialized();
    }

    public void Respawn(Vector3 offset) {
      if (showDebug) {
        Debug.Log(base.name + " Respawn ");
      }
      // ResetState(0, 0);
      for (int i = 0; i < rigidSegments; i++) {
        bones[i].position += offset;
      }
      ForceUpdate();
    }
  }
}
