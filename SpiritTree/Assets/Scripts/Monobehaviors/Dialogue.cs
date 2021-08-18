using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public string[] dialogue;
    public GameObject textObject;
    public Text text;

    private void Start()
    {
        text.text = dialogue[0];
    }
}
