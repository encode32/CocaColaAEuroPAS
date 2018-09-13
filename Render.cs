using System;
using UnityEngine;

namespace Cocacola
{
    public static class Render
    {
        private static Color color;

        private static GUIStyle textStyle = new GUIStyle(GUI.skin.label);
        private static GUIStyle borderTextStyle = new GUIStyle(GUI.skin.label);

        private static Texture2D texture = new Texture2D(1, 1);

        internal static void BoxRect(Rect rect, Color color)
        {
            bool flag = color != Render.color;
            if (flag)
            {
                Render.texture.SetPixel(0, 0, color);
                Render.texture.Apply();
                Render.color = color;
            }
            GUI.DrawTexture(rect, Render.texture);
        }

        internal static void DrawBox(Vector2 pos, Vector2 size, float thick, Color color)
        {
            Render.BoxRect(new Rect(pos.x, pos.y, size.x, thick), color);
            Render.BoxRect(new Rect(pos.x, pos.y, thick, size.y), color);
            Render.BoxRect(new Rect(pos.x + size.x, pos.y, thick, size.y), color);
            Render.BoxRect(new Rect(pos.x, pos.y + size.y, size.x + thick, thick), color);
        }

        internal static void DrawString(Vector2 pos, string text, Color color, bool center = true, int size = 12, int lines = 1)
        {
            Render.textStyle.fontSize = size;
            Render.textStyle.normal.textColor = color;
            GUIContent content = new GUIContent(text);
            if (center)
            {
                pos.x -= Render.textStyle.CalcSize(content).x / 2f;
            }
            Rect position = new Rect(pos.x, pos.y, 300f, (float)lines * 25f);
            GUI.Label(position, content, Render.textStyle);
        }

        internal static void DrawBorderedString(Vector2 pos, string text, Color color, Color borderColor, bool center = true, int size = 12, int lines = 1)
        {
            Render.textStyle.fontSize = size;
            Render.textStyle.normal.textColor = color;
            Render.borderTextStyle.fontSize = size;
            Render.borderTextStyle.normal.textColor = borderColor;

            GUIContent content = new GUIContent(text);
            if (center)
            {
                pos.x -= Render.textStyle.CalcSize(content).x / 2f;
            }
            Rect position = new Rect(pos.x, pos.y, 300f, (float)lines * 25f);

            Rect offPos = position;
            offPos.x = position.x + 1;
            GUI.Label(offPos, content, Render.borderTextStyle);
            offPos.x = position.x - 1;
            GUI.Label(offPos, content, Render.borderTextStyle);
            offPos.x = position.x; offPos.y = position.y + 1;
            GUI.Label(offPos, content, Render.borderTextStyle);
            offPos.y = position.y - 1;
            GUI.Label(offPos, content, Render.borderTextStyle);

            GUI.Label(position, content, Render.textStyle);
        }

        internal static void Draw3DLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
        {
            GameObject go = new GameObject();
            go.transform.position = start;
            go.AddComponent<LineRenderer>();
            LineRenderer lr = go.GetComponent<LineRenderer>();
            lr.material = GG.bumpedDiffuseMat;
            lr.startColor = color;
            lr.endColor = color;
            lr.startWidth = 0.01f;
            lr.endWidth = 0.01f;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            GameObject.Destroy(go, duration);
        }

        internal static void Enable3DLine(GameObject go, Vector3 start, Vector3 end, Color color, float width = 0.01f)
        {
            go.AddComponent<LineRenderer>();
            LineRenderer lr = go.gameObject.GetComponent<LineRenderer>();
            lr.material = GG.bumpedDiffuseMat;
            lr.startColor = Color.red;
            lr.endColor = Color.red;
            lr.startWidth = width;
            lr.endWidth = width;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        internal static void Update3DLine(GameObject go, Vector3 start, Vector3 end)
        {
            LineRenderer lr = go.GetComponent<LineRenderer>();
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
        }

        internal static void Disable3DLine(GameObject go)
        {
            LineRenderer lr = go.GetComponent<LineRenderer>();
            lr.enabled = false;
        }
    }
}
