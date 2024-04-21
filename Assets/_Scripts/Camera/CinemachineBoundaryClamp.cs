using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineBoundaryClamp : CinemachineExtension
{
    public Transform minXTransform; // ��СX��߽�
    public Transform maxXTransform; // ���X��߽�
    public Transform minZTransform; // ��СZ��߽�
    public Transform maxZTransform; // ���Z��߽�

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Finalize)
        {
            Vector3 position = state.RawPosition;

            position.x = Mathf.Clamp(position.x, minXTransform.position.x, maxXTransform.position.x);
            position.z = Mathf.Clamp(position.z, minZTransform.position.z, maxZTransform.position.z);

            state.RawPosition = position;
        }
    }
}
