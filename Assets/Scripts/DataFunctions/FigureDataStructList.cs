using System;
using System.Collections.Generic;
using Accord.IO;
using UnityEngine;

namespace JBK.AstrocyteViewer.DataFunctions
{
    [Serializable]
    public class FigureDataStructList
    {
        // FigureDataStructList reads and stores all data specified by Xfigure .mat files. 
        // Each cell of the Xfigure cell array is read and stored as a FigureDataStruct object in this list
        
        [Tooltip("WARNING! Opening the list in the inspector can cause the editor to freeze for a few seconds! " +
                 "Large amounts of data have to be serialized!")]
        public List<FigureDataStruct> dataStructList; 
        
        public FigureDataStructList(string path)
        {
            dataStructList = new List<FigureDataStruct>(); 
            var inputMatReader = new MatReader(path);
            var inputMatNode = inputMatReader.Fields[inputMatReader.FieldNames[0]];

            foreach (var field in inputMatNode.Fields)
            {
                var matlabStruct = inputMatNode.Fields[field.Key];
                dataStructList.Add(new FigureDataStruct(matlabStruct));    // read all variables in this node and add to list
            }
        }
    }
}