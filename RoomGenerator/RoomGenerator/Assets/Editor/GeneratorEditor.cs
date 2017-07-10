using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateMap))]
public class GeneratorEditor : Editor {

	GenerateMap generatorScript;



	public override void OnInspectorGUI ()
	{

		generatorScript = (GenerateMap)target;

		DrawDefaultInspector ();

		if (GUILayout.Button ("Generate")) {
			generatorScript.Generate ();
		}


	}


}
