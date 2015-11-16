using UnityEngine;
using System.Collections;

public class DebugStuff : MonoBehaviour {

    public Vector2 offset;
    public Vector2 size;
    Vector2 screenSize;
    string date;
    public GUIStyle style;

    void Start ()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
    }

	void OnGUI ()
    {
        date = System.DateTime.Now.ToString();
        GUI.Box(new Rect(offset.x, screenSize.y - offset.y, size.x, size.y), date, style);
    }
}
