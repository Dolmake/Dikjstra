using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DKjistra  {

   
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
        public int accumDistance = 0;
        public int weight = 0;
        public string name;
        public string parent;
        public Label(string name, int distance)
        {
            this.name = name;
            this.accumDistance = distance;
        }

        public List<Connection> connections = new List<Connection>();

        public override string ToString()
        {
            return name + "," + accumDistance; 
        }
    }
    public static void Calculate(string nodo, Graph graph)
    {
        //Inicializamos las etiquetas con distancias infinitas
        Dictionary<string, Label> labels = new Dictionary<string, Label>();
        foreach (KeyValuePair<string, Nodo> n in graph.Nodos)
        {
            Label l = new Label(n.Value.name, int.MaxValue);
            foreach (KeyValuePair<string, int> conn in n.Value.Connections)
            {
                l.connections.Add(new Label.Connection(conn.Key, conn.Value));
            }

            labels.Add(l.name,l);
        }
        //La distancia a mi mismo es 0
        labels[nodo].accumDistance = 0;

        //Me añado a la lista de pendientes
        List<Label> pending_queue = new List<Label>();
        pending_queue.Add(labels[nodo]);       

        while (pending_queue.Count != 0)//Mientras haya etiquetas pendientes
        {  
            //Extraer la etiqueta que tenga la mínima distancia
            Label label = GetLabelWithMininumDistance(pending_queue);
            label.iteracion++; //Aumentamos la iteración, no es necesario pero sirve para Debuguear
           
            //Recalculamos todas las conexiones a la etiqueta actual
            foreach (Label.Connection connection in label.connections)
            {
                //Si existe una etiqueta adyacente cuyo distancia acumulada es mayor...
                Label nodeAdjacent = labels[connection.label];
                if (nodeAdjacent.accumDistance > label.accumDistance + connection.weight)
                {
                    //...la acualizamos
                    nodeAdjacent.accumDistance = label.accumDistance + connection.weight;
                    nodeAdjacent.parent = label.name;
                    //y la añadimos a pendientes porque ha sido modificada
                    pending_queue.Add(nodeAdjacent);
                }
            }
        }

        ShowResults(nodo,labels);
        
    }

    private static void ShowResults(string start, Dictionary<string, Label> labels)
    {
        foreach (KeyValuePair<string, Label> l in labels)
        {
            string s = start + "->";
            Label wayBack = l.Value;            
            while (wayBack.name != start)
            {               
                s += wayBack.name + ",";
                wayBack = labels[wayBack.parent];
            }

            s += start;

            Debug.Log(s + ":" + l.Value.accumDistance);
        }
    }

    static Label GetLabelWithMininumDistance(List<Label> pending_queue)
    {
        if (pending_queue == null || pending_queue.Count == 0) return null;
        int index = 0;
        Label result = pending_queue[0];       
        for (int i = 0; i < pending_queue.Count; ++i)
        {
            if (result.accumDistance > pending_queue[i].accumDistance)
            {
                index = i;
                result = pending_queue[i];
            }
        }
        pending_queue.RemoveAt(index);
        return result;
        
    }
}
