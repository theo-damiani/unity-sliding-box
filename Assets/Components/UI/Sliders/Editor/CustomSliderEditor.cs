using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(CustomSlider)), CanEditMultipleObjects]
public class CustomSliderEditor : SliderEditor
{
    CustomSlider component;

    SerializedProperty valueTMP;
    SerializedProperty numDecimalDigits;
    SerializedProperty snapToDecimal;
    SerializedProperty color;
    SerializedProperty dynamicMaxValue;
    SerializedProperty handleStroke;
    SerializedProperty handleImage;


    protected override void OnEnable()
    {
        base.OnEnable();

        component = (CustomSlider)target;

        valueTMP = serializedObject.FindProperty("valueTMP");
        numDecimalDigits = serializedObject.FindProperty("numDecimalDigits");
        snapToDecimal = serializedObject.FindProperty("snapToDecimal");
        color = serializedObject.FindProperty("color");
        dynamicMaxValue = serializedObject.FindProperty("dynamicMaxValue");
        handleStroke = serializedObject.FindProperty("handleStroke");
        handleImage = serializedObject.FindProperty("handleImage");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();
        EditorGUILayout.PropertyField(valueTMP);
        EditorGUILayout.PropertyField(snapToDecimal);
        EditorGUILayout.PropertyField(numDecimalDigits);
        EditorGUILayout.PropertyField(dynamicMaxValue);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(color);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(handleStroke);
        EditorGUILayout.PropertyField(handleImage);

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            component.ApplyColor();
        }
    }
}
