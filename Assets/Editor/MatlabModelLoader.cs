using System;
using System.Collections.Generic;
using JBK.AstrocyteViewer.DataFunctions;
using UnityEditor;
using UnityEngine;

namespace JBK.AstrocyteViewer.Editor.Editor
{
    public class MatlabModelLoader : EditorWindow
    {   
        private string _xFigureFilePath;
        
        // materials
        private Material opaqueSingleColor;
        private Material singleColor;
        private Material opaqueVertexColors;
        private Material vertexColors;
        private Transform multiMeshContainer;
        private Transform meshContainer;
        private Dictionary<string, Action<FigureDataStruct>> _matTypes;

        [MenuItem("JBK/MatlabModelLoader")]
        private static void ShowWindow()
        {
            var window = GetWindow<MatlabModelLoader>();
            window.titleContent = new GUIContent("MatlabModelLoader");
            window.Show();
        }

        private void OnEnable()
        {
            _matTypes = new Dictionary<string, Action<FigureDataStruct>>
            {
                {"VertexColorMesh", FVmeshVertexColor},
                {"Scatter3D", Scatter3},
                {"MultiVertexColorMesh", FVmeshMultiVertColor},
                {"Graph", DrawGraph},
                {"SingleColorMesh", FVmeshSingleColor},
                {"SetCamDistance", CamDistSetter}, 
                {"ConnectionDictionary", HandleShootConnections}
            };
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
            
            EditorGUILayout.Space(30f);
            multiMeshContainer = EditorGUILayout.ObjectField("Multi Mesh Container", multiMeshContainer, typeof(Transform), true) as Transform;
            meshContainer = EditorGUILayout.ObjectField("Mesh Container", meshContainer, typeof(Transform), true) as Transform;
            EditorGUILayout.Space(5f);
            opaqueSingleColor = EditorGUILayout.ObjectField("Opaque Single Color", opaqueSingleColor, typeof(Material), false) as Material;
            singleColor = EditorGUILayout.ObjectField("Single Color", singleColor, typeof(Material), false) as Material;
            opaqueVertexColors = EditorGUILayout.ObjectField("Opaque Vertex Colors", opaqueVertexColors, typeof(Material), false) as Material;
            vertexColors = EditorGUILayout.ObjectField("Vertex Colors", vertexColors, typeof(Material), false) as Material;
        }

        private void LoadModel()
        {
            var figureDataStructContainer = new FigureDataStructContainer(_xFigureFilePath);

            foreach (var figureData in figureDataStructContainer.DataStructList)
            {
                Debug.Log($"Building Figure element of type: {figureData.Type}");
                _matTypes[figureData.Type](figureData);
            }
        }

        private void FVmeshSingleColor(FigureDataStruct fv)
        {
            var meshInstance =
                BuilderFunctions.InstantiateMesh(fv.Vertices, fv.Faces, meshContainer, fv.Shootability[0], fv.id[0]);
            BuilderFunctions.AddMat(fv.Opacity, meshInstance, singleColor, opaqueSingleColor);
            meshInstance.GetComponent<MeshRenderer>().material.SetColor("_color", fv.SingleColor);
        }

        private void FVmeshVertexColor(FigureDataStruct fv)
        {
            var meshInstance =
                BuilderFunctions.InstantiateMesh(fv.Vertices, fv.Faces, meshContainer, fv.Shootability[0], fv.id[0]);
            BuilderFunctions.AddMat(fv.Opacity, meshInstance, vertexColors, opaqueVertexColors);
            meshInstance.GetComponent<MeshFilter>().mesh.colors = fv.vertColorList[0].colors;
        }

        private void FVmeshMultiVertColor(FigureDataStruct fv)
        {
            var meshInstance = BuilderFunctions.InstantiateMesh(fv.Vertices, fv.Faces, multiMeshContainer,
                fv.Shootability[0], fv.id[0]);
            var multiVertColorList = fv.vertColorList;
            multiMeshContainer.GetComponent<MultiVertColUpdater>().SetStuff(multiVertColorList, meshInstance);
            meshInstance.GetComponent<MeshFilter>().sharedMesh.colors = multiVertColorList[0].colors;
            BuilderFunctions.AddMat(fv.Opacity, meshInstance, vertexColors, opaqueVertexColors);
        }
        
        private void Scatter3(FigureDataStruct sc) => BuilderFunctions.SpawnScatterSpheres(sc.Vertices, sc.PointSize, sc.SingleColor, opaqueSingleColor, meshContainer, sc.Shootability, sc.id);

        private void HandleShootConnections(FigureDataStruct d)
        {
            // do nothing. This void only exists to fit the dictionary pattern. 
        }

        // below are functions that are not used in the current version of the program. But the signatures need to 
        // be preserved for backwards compatibility with older exported xFigure .mat files.
        private void DrawGraph(FigureDataStruct gr) { }

        private void CamDistSetter(FigureDataStruct cd) {}
    }
}