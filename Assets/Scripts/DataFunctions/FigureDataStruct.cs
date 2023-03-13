using System;
using System.Collections.Generic;
using Accord.IO;
using UnityEngine;

namespace JBK.AstrocyteViewer.DataFunctions
{
    [Serializable]
    public class FigureDataStruct
    {
        // FigureDataStruct reads in and stores all data for a object in the Xfigure .mat file
        // content of one cell of the Xfigure object in the matlab workspace (should be one struct)
        // checks which fields are specified by the matlab struct and saves them in the 
        // appropriate dataformat that is expected by FigureManager.cs and BuilderFunctions.cs
        public string Type;
        public Vector3[] Vertices;
        public int[] Faces;
        public float Opacity;
        public Color SingleColor;
        public int CamDistance;
        public int[] PointSize;
        public List<ColorArray> vertColorList;
        public int[] Shootability; 
        public int[] id; 
        
        public FigureDataStruct(MatNode matNode)
        {
            Type = new List<string>(matNode.Fields["type"].Fields.Keys)[0]; // type MUST be defined for every input object that is why it is the only one not in an if statement

            if (Type == "ConnectionDictionary")
            {
                HandleConnectionDictionaries(matNode);
            }

            if (matNode.Fields.ContainsKey("vertices"))
            {
                Vertices = DataParser.MatrixToVectorArray(matNode.Fields["vertices"].GetValue<double[,]>());
            }

            if (matNode.Fields.ContainsKey("faces"))
            {
                Faces = DataParser.MatrixTo1DArray(matNode.Fields["faces"].GetValue<int[,]>());
            }

            if (matNode.Fields.ContainsKey("opacity"))
            {
                Opacity = (float)matNode.Fields["opacity"].GetValue<double[,]>()[0, 0];
            }

            if (matNode.Fields.ContainsKey("colors"))
            {
                var colorArray = matNode.Fields["colors"].GetValue<double[,]>();
                vertColorList = DataParser.GetVertexColorList(colorArray, matNode.Fields["map"].GetValue<double[,]>());
            }

            if (matNode.Fields.ContainsKey("color"))
            {
                SingleColor = DataParser.GetColor(matNode.Fields["color"].GetValue<double[,]>());
            }

            if (matNode.Fields.ContainsKey("size"))
            {
                PointSize = DataParser.MatrixTo1DArray(matNode.Fields["size"].GetValue<int[,]>());
            }

            if (matNode.Fields.ContainsKey("camDistance"))
            {
                CamDistance = matNode.Fields["camDistance"].GetValue<int[,]>()[0, 0];
            }

            if (matNode.Fields.ContainsKey("shootability"))
            {
                Shootability = DataParser.MatrixTo1DArray(matNode.Fields["shootability"].GetValue<int[,]>()); 
            }

            if (matNode.Fields.ContainsKey("id"))
            {
                id = DataParser.MatrixTo1DArray(matNode.Fields["id"].GetValue<int[,]>()); 
            }
        }

        private static void HandleConnectionDictionaries(MatNode matNode)
        {
            if (matNode.Fields.ContainsKey("DirectConnections"))
            {
                foreach (var targetID in matNode.Fields["DirectConnections"].Fields.Keys)
                {
                    // is this efficient to add straight to the dictionary in ShootManager...?
                    // or would it be better to build a temp Dictionary here and add the whole dictionary to shootmanager...
                    ShootManager.DirectConnectionDictionary.Add(targetID, DataParser.IntMatrixTo1DStringArray(matNode.Fields["DirectConnections"].Fields[targetID].GetValue<int[,]>(), prefix: "target_"));
                }
            }
            
            if (matNode.Fields.ContainsKey("IndirectConnections"))
            {
                foreach (var targetID in matNode.Fields["IndirectConnections"].Fields.Keys)
                {
                    ShootManager.InDirectConnectionDictionary.Add(targetID, DataParser.IntMatrixTo1DStringArray(matNode.Fields["IndirectConnections"].Fields[targetID].GetValue<int[,]>(), prefix: "target_"));
                }
            }
        }
        
    }
}