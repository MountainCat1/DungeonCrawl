using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities.Editor
{
    public class ReplaceFontsEditor : EditorWindow
    {
        private Font _uiFont;
        private TMP_FontAsset _tmpFont;

        private bool _replaceUIText = true;
        private bool _replaceTmpugui = true;
        private bool _replaceTMP3D = true;

        [MenuItem("Tools/Replace Fonts In Scene & Prefabs")]
        public static void ShowWindow()
        {
            GetWindow<ReplaceFontsEditor>("Replace Fonts");
        }

        private void OnGUI()
        {
            GUILayout.Label("Font Replacement Tool", EditorStyles.boldLabel);

            _uiFont = (Font)EditorGUILayout.ObjectField("UI Font", _uiFont, typeof(Font), false);
            _tmpFont = (TMP_FontAsset)EditorGUILayout.ObjectField("TMP Font Asset", _tmpFont, typeof(TMP_FontAsset), false);

            EditorGUILayout.Space();
            GUILayout.Label("Replace in:", EditorStyles.boldLabel);
            _replaceUIText = EditorGUILayout.Toggle("UI Text", _replaceUIText);
            _replaceTmpugui = EditorGUILayout.Toggle("TextMeshProUGUI", _replaceTmpugui);
            _replaceTMP3D = EditorGUILayout.Toggle("TextMeshPro (3D)", _replaceTMP3D);

            EditorGUILayout.Space();

            if (GUILayout.Button("Replace Fonts In Scene"))
            {
                ReplaceFontsInScene();
            }

            if (GUILayout.Button("Replace Fonts In Prefabs"))
            {
                ReplaceFontsInPrefabs();
            }
        }

        private void ReplaceFontsInScene()
        {
            int count = 0;

            if (_replaceUIText && _uiFont != null)
            {
                var uiTexts = Object.FindObjectsByType<Text>(FindObjectsSortMode.None);
                foreach (var text in uiTexts)
                {
                    Undo.RecordObject(text, "Replace UI Font");
                    text.font = _uiFont;
                    EditorUtility.SetDirty(text);
                    count++;
                }
            }

            if (_tmpFont != null)
            {
                if (_replaceTmpugui)
                {
                    var tmpUIs = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);
                    foreach (var tmp in tmpUIs)
                    {
                        Undo.RecordObject(tmp, "Replace TMP UGUI Font");
                        tmp.font = _tmpFont;
                        EditorUtility.SetDirty(tmp);
                        count++;
                    }
                }

                if (_replaceTMP3D)
                {
                    var tmp3Ds = Object.FindObjectsByType<TextMeshPro>(FindObjectsSortMode.None);
                    foreach (var tmp in tmp3Ds)
                    {
                        Undo.RecordObject(tmp, "Replace TMP 3D Font");
                        tmp.font = _tmpFont;
                        EditorUtility.SetDirty(tmp);
                        count++;
                    }
                }
            }

            Debug.Log($"[Scene] Replaced fonts on {count} text components.");
        }

        private void ReplaceFontsInPrefabs()
        {
            string[] prefabGUIDs = AssetDatabase.FindAssets("t:Prefab");
            int count = 0;

            foreach (var guid in prefabGUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                bool modified = false;

                GameObject root = PrefabUtility.LoadPrefabContents(path);

                if (_replaceUIText && _uiFont != null)
                {
                    foreach (var text in root.GetComponentsInChildren<Text>(true))
                    {
                        text.font = _uiFont;
                        EditorUtility.SetDirty(text);
                        modified = true;
                        count++;
                    }
                }

                if (_tmpFont != null)
                {
                    if (_replaceTmpugui)
                    {
                        foreach (var tmp in root.GetComponentsInChildren<TextMeshProUGUI>(true))
                        {
                            tmp.font = _tmpFont;
                            EditorUtility.SetDirty(tmp);
                            modified = true;
                            count++;
                        }
                    }

                    if (_replaceTMP3D)
                    {
                        foreach (var tmp in root.GetComponentsInChildren<TextMeshPro>(true))
                        {
                            tmp.font = _tmpFont;
                            EditorUtility.SetDirty(tmp);
                            modified = true;
                            count++;
                        }
                    }
                }

                if (modified)
                {
                    PrefabUtility.SaveAsPrefabAsset(root, path);
                }

                PrefabUtility.UnloadPrefabContents(root);
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"[Prefabs] Replaced fonts on {count} text components.");
        }
    }
}