using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Graph : MonoBehaviour  {

    public GameObject NodePrefab;
    public GameObject PrefabConnection;

    Dictionary<string, Nodo> _nodos = new Dictionary<string, Nodo>();

    public Dictionary<string, Nodo> Nodos
    {
        get { return _nodos; }       
    }
   

    public void ReadFromResources(string resourceName)
    {
        while (this.transform.GetChildCount() != 0)
        {
            Destroy(this.transform.GetChild(0));
            this.transform.GetChild(0).parent = null;
        }
       
        _nodos.Clear();
        TextAsset text = Resources.Load(resourceName) as TextAsset;
        if (!text) Debug.Log("No Text File:" + resourceName);

        string[] lines = text.text.Split('\n');
        foreach (string line in lines)
        {           
            
            GameObject o = Instantiate(NodePrefab) as GameObject;
            o.transform.parent = this.transform;
            Nodo nodo = o.GetComponent<Nodo>();
            nodo.FromString(line);
            _nodos.Add(nodo.name, nodo);
            Debug.Log("Nodo added: "+ nodo.ToString());
        }
        foreach (KeyValuePair<string, Nodo> nodo in _nodos)
        {
            CreateConnections(nodo.Value);
        }
    }

    private void CreateConnections(Nodo nodo)
    {
        foreach (KeyValuePair<string, int> conn in nodo.Connections)
        {
            GameObject o = Instantiate(PrefabConnection) as GameObject;
            LineRenderer lr = o.GetComponent<LineRenderer>();
            Debug.Log("Creating COnnection from:"+ nodo.name + " to " + conn.Key);
            lr.SetPosition(0, nodo.transform.position);
            lr.SetPosition(1, _nodos[conn.Key].transform.position);
            o.transform.parent = this.transform;
        }
    }


}
