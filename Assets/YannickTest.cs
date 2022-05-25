using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class YannickTest : EditorWindow
{
    List<Module> joysticks = new List<Module>();
    public SerializeList<Module> Modules = new SerializeList<Module>();

    [MenuItem("YEET/YannickTest")]
    static void Init() {
        var window = GetWindow(typeof(YannickTest));
        window.Show();
    }

    // Start is called before the first frame update
    void Awake()
    {
        joysticks.Add(new JoyStick());
    }

    private void OnGUI() {
        if(GUILayout.Button("Yannick Drukt Hier Op")) {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Movement/Joystick"), false, addJoystick, "Joystick");
            menu.ShowAsContext();
        }

        foreach (var item in Modules.list) {
            DrawModules(item);
        }
    }

    private void DrawModules(Module module) {
        GUILayout.Space(10);
        module.folded = EditorGUILayout.Foldout(module.folded, "Module Settings", true);

        var type = module.GetType();
        var properties = type.GetMembers();

        for (int i = 6; i < properties.Length; i++) {
            if (module.folded) {
                if (GUILayout.Button(properties[i].Name)) {

                }
            }
        }
    }

    private void addJoystick(object name) {
        var joystick = new JoyStick();

        Modules.list.Add(joystick);
    }
}

[System.Serializable]
public class SerializeList<T> {
    public List<T> list = new List<T>();
}

[System.Serializable]
public abstract class Module {
    public string name;
    public string name2;
    public string name3;
    public bool folded = true;
}

public class JoyStick : Module {
    public string settingName;
    public int value;

    public bool end;
}

public class Wheel : Module { 

}