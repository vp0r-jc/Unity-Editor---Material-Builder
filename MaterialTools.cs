using UnityEngine;
using UnityEditor;

public class MaterialTool : EditorWindow
{
    [SerializeField] Material material;
    [SerializeField] string path;

    GameObject gameObject;
    Editor gameObjectEditor;
    Texture2D previewBackgroundTexture;

    [MenuItem("Window/MatTool")]
    public static void DrawWindow()
    {
        GetWindow<MaterialTool>("Material-Tool");
    }


    private void OnGUI()
    {
        //Window Code
        GUILayout.Label("Material Build Tool V1", EditorStyles.helpBox);

        GUILayout.Label(" ", EditorStyles.wordWrappedLabel);
        GUILayout.Label("Paste Material Path", EditorStyles.wordWrappedLabel);
        path = EditorGUILayout.TextField("Set Path", path);

        GUILayout.Label(" ", EditorStyles.wordWrappedLabel);

        GUILayout.Label("Load Material", EditorStyles.wordWrappedLabel);
        material = (Material)EditorGUILayout.ObjectField(material, typeof(Material), true);
        //material.path

        GUILayout.Label(" ", EditorStyles.wordWrappedLabel);

        if (GUILayout.Button("Build Material"))
        {
            var assets = AssetDatabase.FindAssets("t:Texture2D", new[] { path });
            //var assets = AssetDatabase.FindAssets("t:Texture2D");
            foreach (var guid in assets)
            {
                var clip = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(guid));
                //Debug.Log(clip);
                if (clip.name.Contains("_AL"))
                {
                    material.SetTexture("_BaseMap", clip);
                    Debug.Log("ALBEDO SET");
                }
                else if (clip.name.Contains("_AO"))
                {
                    material.SetTexture("_OcclusionMap", clip);
                    Debug.Log("AO SET");
                }
                else if (clip.name.Contains("_MT"))
                {
                    material.SetTexture("_MetallicGlossMap", clip);
                    Debug.Log("METAL SET");
                }
                else if (clip.name.Contains("_NM"))
                {
                    material.SetTexture("_BumpMap", clip);
                    Debug.Log("NORMAL SET");
                }
                else
                {
                    Debug.LogWarning("no match");
                }


            }
        }
        GUILayout.Label(" ", EditorStyles.wordWrappedLabel);

        GUILayout.Label("Load 3D Model for Preview", EditorStyles.wordWrappedLabel);

        GUILayout.Label(" ", EditorStyles.wordWrappedLabel);

        EditorGUI.BeginChangeCheck();

        gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);

        if (EditorGUI.EndChangeCheck())
        {
            if (gameObjectEditor != null) DestroyImmediate(gameObjectEditor);
        }

        GUIStyle bgColor = new GUIStyle();

        bgColor.normal.background = previewBackgroundTexture;

        if (gameObject != null)
        {
            if (gameObjectEditor == null)

                gameObjectEditor = Editor.CreateEditor(gameObject);
            gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(200, 200), bgColor);
        }

    }
}
