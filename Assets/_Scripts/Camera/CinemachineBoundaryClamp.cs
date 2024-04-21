using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineBoundaryClamp : CinemachineExtension
{
    public Transform minXTransform; // 最小X轴边界
    public Transform maxXTransform; // 最大X轴边界
    public Transform minZTransform; // 最小Z轴边界
    public Transform maxZTransform; // 最大Z轴边界

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
