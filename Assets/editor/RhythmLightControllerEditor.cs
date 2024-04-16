using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RhythmController))]
public class RhythmLightControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // 绘制默认的Inspector

        RhythmController controller = (RhythmController)target;

        if (GUILayout.Button("Update Material"))
        {
            controller.UpdateMaterial(); // 当按钮被点击时，调用UpdateMaterial方法
        }
    }
}