using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Astar {

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
        public int iteracion = 0;

        //G Coste acumulado
        public float cost = 0;//G
        public float G { get { return cost; } set { cost = value; } }

        //H
        public float heuristicCost(Label target) //H
        { return (target.worldPosition - worldPosition).magnitude;  }
        public float H(Label target) { return heuristicCost(target); }

        //F Coste acumulado + Coste Heuristico, se usa para seleccionar
        // el siguiente candidato
        public float estimatedCost = 0;
        public float F { get { return estimatedCost; } set { estimatedCost = value; } }


        public string name;
        public string parent;
        public Vector3 worldPosition;
        public Label(string name, Vector3 worldPosition)
        {
            this.name = name;
            this.worldPosition = worldPosition;
           
        }

        public List<Connection> connections = new List<Connection>();

        public override string ToString()
        {
            return name + "," + cost;
        }
    }
    public static void Calculate(string start, string target, Graph graph)
    {
        //Inicializamos las etiquetas con distancias 0
        Dictionary<string, Label> labels = new Dictionary<string, Label>();
        foreach (KeyValuePair<string, Nodo> n in graph.Nodos)
        {
            Label l = new Label(n.Value.name,n.Value.transform.position);
            foreach (KeyValuePair<string, int> conn in n.Value.Connections)
            {
                l.connections.Add(new Label.Connection(conn.Key, conn.Value));
            }

            labels.Add(l.name, l);
        }
        //La distancia a mi mismo es 0
        labels[start].G = 0;
        labels[start].F = labels[start].G + labels[start].H(labels[target]);

        List<Label> OpenSet = new List<Label>();// Caminos abiertos
        List<Label> CloseSet = new List<Label>();// Caminos cerrados

        //Me añado a la lista de caminos abiertos        
        OpenSet.Add(labels[start]);

        while (OpenSet.Count != 0)
        {
            Label current = GetMinimunF(OpenSet);
            if (current.name == target)//Hemos llegado...
            {
                //salimos
                ShowResults(current, labels);
                return;
            }
            CloseSet.Add(current);//Añadimos current a Cerrados

            //Tratamos los siguientes
            foreach(Label.Connection conn in current.connections)
            {
                Label successor = labels[conn.label];
                //Calculamos el siguiente peso si tomaramos este camino
                float tentative_G = current.G + conn.weight;

                //Si es un camino cerrado o si ya tiene un peso calculado mejor
                if (CloseSet.Contains(successor) && tentative_G >= successor.G)
                    continue;//Salimos

                //Si no ha sido añadido a caminos abiertos o si el peso
                //anteriormente calculado es peor
                if (!OpenSet.Contains(successor) || tentative_G < successor.G)
                {
                    //Actualizamos su predecesor
                    successor.parent = current.name;
                    successor.G = tentative_G; //Actualizamos su peso
                    successor.F = successor.G + successor.H(labels[target]);//Actualizamos su F
                    if (!OpenSet.Contains(successor))
                        OpenSet.Add(successor);
                }
            }
        }
        return;

    }

    private static void ShowResults(Label current,Dictionary<string, Label> labels )
    {
        string way = "";

        bool end = false;
        while (!end)
        {
            way += current.name + ",";
            end = string.IsNullOrEmpty(current.parent) || !labels.ContainsKey(current.parent);
            if (!end)
                current = labels[current.parent];
        }
        Debug.Log(way);
    }

     static Label GetMinimunF(List<Label> OpenSet)
    {
        Label min = OpenSet[0];
        int index = 0;
        for (int i = 0; i < OpenSet.Count; ++i)
        {
            if (OpenSet[i].F < min.F)
            {
                index = i;
                min = OpenSet[i];
            }
        }
        OpenSet.RemoveAt(index);
        return min;
    }
}
