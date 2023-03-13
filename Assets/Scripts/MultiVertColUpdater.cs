using System.Collections.Generic;
using JBK.AstrocyteViewer.DataFunctions;
using UnityEngine;
using UnityEngine.UI;

namespace JBK.AstrocyteViewer
{
    public class MultiVertColUpdater : MonoBehaviour
    {
        public Slider slider;
        public GameObject SliderCanvas;

        private List<ColorArray> _multiVertColorList;
        private MeshFilter multiVertMeshFilter;

        private void Start()
        {
            slider.maxValue = 0;
        }

        public void UpdateVertexColors()
        {
            var tab = (int)slider.value;

            multiVertMeshFilter.mesh.colors = _multiVertColorList[tab].colors;
        }

        public void SetStuff(List<ColorArray> colors, GameObject go)
        {
            _multiVertColorList = colors; 
            multiVertMeshFilter = go.GetComponent<MeshFilter>();

            SliderCanvas.SetActive(true);
            slider.maxValue = _multiVertColorList.Count - 1;
            slider.value = 0;
        }
    }
}
