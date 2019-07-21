using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class input_field_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var input = gameObject.GetComponent<InputField>();
        var se= new InputField.SubmitEvent();
        se.AddListener(SubmitName);
        input.onEndEdit = se;
    }

    // Update is called once per frame
    private void SubmitName(string arg0)
    {
        Debug.Log(arg0);
    }
}
