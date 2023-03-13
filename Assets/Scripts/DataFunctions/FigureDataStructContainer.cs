using System.Collections.Generic;
using Accord.IO;

namespace JBK.AstrocyteViewer.DataFunctions
{
    public class FigureDataStructContainer
    {
        // FigureDataStructContainer reads and stores all data specified by Xfigure .mat files. 
        // Each cell of the Xfigure cell array is read and stored as a FigureDataStruct object in this list

        public readonly List<FigureDataStruct> DataStructList; 
        
        public FigureDataStructContainer(string path)
        {
            DataStructList = new List<FigureDataStruct>(); 
            var inputMatReader = new MatReader(path);
            var inputMatNode = inputMatReader.Fields[inputMatReader.FieldNames[0]];

            foreach (var field in inputMatNode.Fields)
            {
                var matlabStruct = inputMatNode.Fields[field.Key];
                DataStructList.Add(new FigureDataStruct(matlabStruct));    // read all variables in this node and add to list
            }
        }
    }
}