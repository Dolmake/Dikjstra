using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class Nodo : MonoBehaviour {

    public static char NODE_SEPATOR = ':';
    public static char NODE_CONECTION = ',';
    public static float POSITION_FACTOR = 10;

   

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    Dictionary<string, int> _connections = new Dictionary<string, int>();

    public Dictionary<string, int> Connections
    {
        get { return _connections; }       
    }
    public void AddConnection(string conn)
    {
        if (string.IsNullOrEmpty(conn)) return;

        if (!_connections.ContainsKey(conn))
        {
            _connections.Add(conn,1);
        }
    }



    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(name);
        sb.Append(NODE_SEPATOR);
        int X = Mathf.RoundToInt(this.transform.localPosition.x);
        int Y = Mathf.RoundToInt(this.transform.localPosition.y);
        sb.Append(X);
        sb.Append(NODE_CONECTION);
        sb.Append(Y);
        sb.Append(NODE_CONECTION);


        foreach (KeyValuePair<string, int> pair in _connections)
        {
            sb.Append(pair.Key);
            sb.Append(NODE_CONECTION);
        }
        return sb.ToString();
    }

    public void FromString(string line)
    {
        _connections.Clear();
        string[] parts = line.Split(Nodo.NODE_SEPATOR);
        this.name = parts[0].Trim();
        SendMessage("mNewName", SendMessageOptions.DontRequireReceiver);

        string[] positions = parts[1].Split(NODE_CONECTION);
        float X = float.Parse(positions[0].Trim()) * POSITION_FACTOR;
        float Y = float.Parse(positions[1].Trim()) * POSITION_FACTOR;
        this.transform.localPosition = new Vector3(X, Y, 0);

        string[] connections = parts[2].Split(Nodo.NODE_CONECTION);
        foreach (string conn in connections)
        {
            AddConnection(conn.Trim());
        }
    }
}
