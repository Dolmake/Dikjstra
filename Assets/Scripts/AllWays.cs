using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AllWays {

    private class Label
    {
        public struct Connection
        {
            public string label;
            public int weight;
            public Connection(string label, int weight)
            {
                this.label = label; this.weight = weight;
            }
        }
      
         
        public string name;       
        public Label(string name)
        {
            this.name = name;
            
        }

        public List<Connection> connections = new List<Connection>();

        
    }
    public static void Calculate(string start, string target, Graph graph)
    {
        //Inicializamos las etiquetas con distancias infinitas
        Dictionary<string, Label> labels = new Dictionary<string, Label>();
        foreach (KeyValuePair<string, Nodo> n in graph.Nodos)
        {
            Label l = new Label(n.Value.name);
            foreach (KeyValuePair<string, int> conn in n.Value.Connections)
            {
                l.connections.Add(new Label.Connection(conn.Key, conn.Value));
            }

            labels.Add(l.name, l);
        }
        List<string> ways = new List<string>();
        string way = "";
        Recursive(start, target, labels,way, ways);
        ShowResults(ways);
    }

    private static void ShowResults(List<string> ways)
    {
        foreach (string s in ways)
        { Debug.Log(s); }
    }

   

    static void Recursive(string current,string target, Dictionary<string, Label> labels, string way, List<string> ways)
    {
        Label label = labels[current];
        way += label.name + ",";
        if (current == target)
            ways.Add(way);
        else
        {
            foreach (Label.Connection conn in label.connections)
            {
                if (!way.Contains(conn.label))
                {                   
                    Recursive(conn.label, target, labels, way, ways);
                }
            }
        }
    }
}
