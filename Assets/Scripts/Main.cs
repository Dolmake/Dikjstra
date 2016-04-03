using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {

    public static Main SINGLETON { get; private set; }
    void Awake()
    {
        SINGLETON = this;
    }

    public string GraphResourceName;
    Graph _graph;

	// Use this for initialization
	void Start () {
        _graph = GetComponent<Graph>();

        ReadGraph();
       
	}


    public bool Reload = false;
    public bool Dikjstra = false;
    public bool FindWays = false;
    public bool A_Star = false;
    public string StartNode;
    public string EndNode;
	// Update is called once per frame
	void Update () {
        if (Reload)
        {
            Reload = false;
            ReadGraph();           
        }
        if (Dikjstra)
        {
            Dikjstra = false;           
            DKjistra.Calculate(StartNode,_graph);
        }
        if (FindWays)
        {
            FindWays = false;
            AllWays.Calculate(StartNode, EndNode, _graph);
        }
        if (A_Star)
        {
            A_Star = false;
            Astar.Calculate(StartNode, EndNode, _graph);
        }
	}

  
   

    private void ReadGraph()
    {
        if (_graph)
            _graph.ReadFromResources(GraphResourceName);
    }
	
}
