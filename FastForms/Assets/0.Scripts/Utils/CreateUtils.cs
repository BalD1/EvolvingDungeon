using UnityEngine;

namespace StdNounou
{
    public static class CreateUtils
    {
        public static TextMesh CreateWorldText(string _text, Vector3 _localPosition, int _fontSize, Color _color, TextAnchor textAnchor, TextAlignment textAlignment, int _sortingOrder)
        {
            GameObject gO = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gO.transform;
            transform.localPosition = _localPosition;
            TextMesh textMesh = gO.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = _text;
            textMesh.fontSize = _fontSize;
            textMesh.color = _color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = _sortingOrder;

            return textMesh;
        }
    }

}