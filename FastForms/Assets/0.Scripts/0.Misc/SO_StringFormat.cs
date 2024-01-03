using UnityEngine;

[CreateAssetMenu(fileName = "New String Format", menuName = "Scriptable/StringFormat")]
public class SO_StringFormat : ScriptableObject
{
    [field: SerializeField, TextArea(10, 1000)] public string Format { get; private set; }
    [field: SerializeField, TextArea(10, 1000)] public string RichFormat { get; private set; }
}