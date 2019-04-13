using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
public class testScript : MonoBehaviour
{
    public static List<string> textArray;

    public Text textComp;

    public int[] rowsToReadFrom = new int[1] {0};

    public string FileName;

    private TextAsset myTextAsset;

    private int index = 0;


    private void Start()
    {
        myTextAsset = Resources.Load("dictionary") as TextAsset;
        string myString = myTextAsset.text;
    }

    public void readTextFile()
    {
        textComp.text = "";
        rowsToReadFrom[0] = index;
        textArray = myTextAsset.text.Split('\n').ToList();
        textComp.text += textArray[rowsToReadFrom[0]] + "\n";

        index += 1;

    }
}
