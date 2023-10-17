using JsonFx.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Document
{
    public class Wrapper<T>
    {
        public T[] items;
    }

    /// <summary>
    /// JSON ���ϵ��� ������ ���� ��θ� �ǹ��մϴ�.
    /// </summary>
    private static readonly string FILE_PATH = "Document";

    /// <summary>
    /// fileName �̸��� ����, � ������ �ܾ���� �����մϴ�.
    /// </summary>
    protected static T[] Extract<T>(string fileName)
    {
        try
        {
            string jsonStr = Resources.Load<TextAsset>(string.Format("{0}/{1}", FILE_PATH, fileName)).text;
            if(string.IsNullOrEmpty(jsonStr))
                throw new Exception("jsonStr is null or Empty");

            T[] WrapJsons = JsonReader.Deserialize<T[]>(jsonStr);
            if(WrapJsons == null)
                throw new Exception("WrapJsons is null");

            return WrapJsons;
        }
        catch(Exception e)
        {
#if UNITY_EDITOR
            Debug.LogError("[Message]\n" + e.Message + "\n[StackTrace]\n" + e.StackTrace);
#endif
            return null;
        }
    }
    public static T[] OutExtract<T>(string fileName)
    {
        try
        {
            string jsonStr = Resources.Load<TextAsset>(string.Format("{0}/{1}", FILE_PATH, fileName)).text;
            if (string.IsNullOrEmpty(jsonStr))
                throw new Exception("jsonStr is null or Empty");

            T[] WrapJsons = JsonUtility.FromJson<T[]>(jsonStr);
            if (WrapJsons == null)
                throw new Exception("WrapJsons is null");

            return WrapJsons;
        }
        catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.LogError("[Message]\n" + e.Message + "\n[StackTrace]\n" + e.StackTrace);
#endif
            return null;
        }
    }
}
