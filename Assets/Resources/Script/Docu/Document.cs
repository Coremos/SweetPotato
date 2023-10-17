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
    /// JSON 파일들이 모여있을 파일 경로를 의미합니다.
    /// </summary>
    private static readonly string FILE_PATH = "Document";

    /// <summary>
    /// fileName 이름에 따라, 어떤 파일을 긁어올지 결정합니다.
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
