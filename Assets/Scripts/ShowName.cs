using UnityEngine;
using System.Collections;

public class ShowName : MonoBehaviour {

    TextMesh _textMesh;
	// Use this for initialization
	void Start () {

        _textMesh = GetComponent<TextMesh>();
        if (!_textMesh)
            _textMesh = this.gameObject.AddComponent<TextMesh>();
        mNewName();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void mNewName()
    {       
        if (_textMesh != null)
            _textMesh.text = this.name;
    }
}
