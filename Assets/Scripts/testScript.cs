using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class testScript : MonoBehaviour
{
   
    private void Start()
    {
        TextAsset myTextAsset = Resources.Load("dictionary") as TextAsset;
        string myString = myTextAsset.text;

        Debug.Log(myString);
    }
}
