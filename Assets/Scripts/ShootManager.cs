using System.Collections.Generic;
using JBK.AstrocyteViewer.DataFunctions;
using UnityEngine;

namespace JBK.AstrocyteViewer
{
    public static class ShootManager
    {
        public static Dictionary<string, string[]> DirectConnectionDictionary = new Dictionary<string, string[]>();
        public static Dictionary<string, string[]> InDirectConnectionDictionary = new Dictionary<string, string[]>();

        public static void ShootConnections(string shot)
        {
            if (DirectConnectionDictionary.ContainsKey(shot))
            {
                // this all seems a bit inefficient but it does not have to run very often or very fast.
                // for now this should be good enough. Maybe I will come up with a cleaner solution later.....
                var shotState = GameObject.Find(shot).GetComponent<EmissionController>().emission;
                foreach (var vic in DirectConnectionDictionary[shot])
                {
                    GameObject.Find(vic).GetComponent<EmissionController>().SetEmission(shotState);
                    ShootConnections(vic); // ensures that vic's direct connections are also set off
                }
            } 
        }

        public static void ShootInDirectConnections(string shot)
        {
            if (InDirectConnectionDictionary.ContainsKey(shot))
            {
                var shotState = GameObject.Find(shot).GetComponent<EmissionController>().emission;
                foreach (var vic in InDirectConnectionDictionary[shot])
                {
                    GameObject.Find(vic).GetComponent<EmissionController>().SetEmission(shotState);
                    ShootConnections(vic); // ensures that vic's direct connections are also set off
                }
            } 
        }

        public static void EmptyDictionaries()
        {
            DirectConnectionDictionary.Clear();
            InDirectConnectionDictionary.Clear(); 
        }
    }
}
