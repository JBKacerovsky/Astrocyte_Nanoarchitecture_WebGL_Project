using System;
using UnityEngine;

namespace JBK.AstrocyteViewer.DataFunctions
{
    // ColorArray is a serializable wrapper for Color[] arrays.
    [Serializable]
    public class ColorArray
    {
        public Color[] colors;
        
        public ColorArray(Color[] colors)
        {
            this.colors = colors;
        }
    }
}