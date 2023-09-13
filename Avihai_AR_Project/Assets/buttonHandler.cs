using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonHandler : MonoBehaviour
{
    public void Next()
    {
        // Go next scene
        GameObject go = GameObject.Find("LoginScreenCanvas");
        if (!go)
        {
            return;
        }

        var obj = GameObject.Find("login");
        var comp = obj.GetComponents<TMPro.TMP_InputField>()[0];
        if (comp != null)
        {
            ARLocationReporter.name = comp.text;
        }
        go.SetActive(false);
    }
}
