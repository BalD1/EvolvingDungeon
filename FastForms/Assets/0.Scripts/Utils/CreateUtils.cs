using UnityEngine;

namespace StdNounou
{
    public static class CreateUtils
    {
        public static TextMesh CreateWorldText(string _text, Vector3 _position, int _fontSize, Color _color, TextAnchor textAnchor, TextAlignment textAlignment, int _sortingOrder, float lifeTime)
        {
            GameObject gO = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gO.transform;
            transform.position = _position;
            TextMesh textMesh = gO.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = _text;
            textMesh.fontSize = _fontSize;
            textMesh.color = _color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = _sortingOrder;

            if (lifeTime > 0)
            {
                System.Action onEnd = () => GameObject.Destroy(gO);
                Timer t = new Timer(lifeTime, onEnd);
                t.Start();
            }

            return textMesh;
        }
        public static TextMesh CreateWorldText(string _text, Vector3 _position, int _fontSize, Color _color, TextAnchor textAnchor, TextAlignment textAlignment, int _sortingOrder)
            => CreateWorldText(_text, _position, _fontSize, _color, textAnchor, textAlignment, _sortingOrder, -1);
    }

}