using UnityEditor;
using UnityEngine;

namespace MoreMountains.Tools
{

    [CustomPropertyDrawer(typeof(MMHiddenAttribute))]

    public class MMHiddenAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

        }
    }
}