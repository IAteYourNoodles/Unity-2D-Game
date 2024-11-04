using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyIndicator : MonoBehaviour
{

    public KeyCode key;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            GetComponent<Text>().color = Color.red;
        }
        else if (Input.GetKeyUp(key))
        {
            GetComponent<Text>().color = Color.black;
        }
    }
}
