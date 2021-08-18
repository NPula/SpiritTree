using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;

public class FileParser
{
    private System.IO.StreamReader input = null;
    public List<string> dataList;

    public void OpenFile(string filePath)
    {
        dataList = new List<string>();
        input = File.OpenText(Application.streamingAssetsPath + "/" + filePath);
        try
        {
            input.ReadLine(); // throw away the first line.
            while (!input.EndOfStream)
            {
                ParseData(input.ReadLine());
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
        finally
        {
            if (input != null)
            {
                input.Close();
            }
        }
    }

    private void ParseData(string data)
    {
        string[] splitData = data.Split(',');

        for (int i = 0; i < splitData.Length; i++)
        {
            dataList.Add(splitData[i]);
        }
    }

    public void PrintData()
    {
        foreach (string d in dataList)
        {
            Debug.Log(d);
        }
    }
}
