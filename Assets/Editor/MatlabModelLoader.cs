using JBK.AstrocyteViewer.DataFunctions;
using UnityEditor;
using UnityEngine;

namespace JBK.AstrocyteViewer.Editor.Editor
{
    public class MatlabModelLoader : EditorWindow
    {   
        private string _xFigureFilePath;

        [MenuItem("JBK/MatlabModelLoader")]
        private static void ShowWindow()
        {
            var window = GetWindow<MatlabModelLoader>();
            window.titleContent = new GUIContent("MatlabModelLoader");
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("XFigure File");
            var filePath = EditorGUILayout.TextField(_xFigureFilePath);
            if (GUILayout.Button("select file"))
            { 
                filePath = EditorUtility.OpenFilePanel("Select XFigure File", "~", "*mat");
            }
            if (!string.IsNullOrEmpty(filePath)) _xFigureFilePath = filePath;
            
            if (GUILayout.Button("Load Model")) LoadModel();
        }

        private void LoadModel()
        {
            var figureDataStructList = new FigureDataStructContainer(_xFigureFilePath);
            Debug.Log($"figureDataStructList is loaded with {figureDataStructList.DataStructList.Count} elements");
        }
    }
}