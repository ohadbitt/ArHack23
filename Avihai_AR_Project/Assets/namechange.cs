using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class namechange : MonoBehaviour
{
    private string input;
   public void ReadStringInput(string s)
    {
        ARLocationReporter.name = s;
    }
    public void ReadStringInput2(string s)
    {
        ARLocationReporter.name = s;
    }
}
