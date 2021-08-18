using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private FileParser fp;

    void Start()
    {
        // Read in Dialogue Data.
        fp = new FileParser();
        fp.OpenFile("Dialogue\\RTG_01_Dialogue.csv");
        //fp.PrintData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
