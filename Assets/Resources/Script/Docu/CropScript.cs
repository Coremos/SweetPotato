using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropScript : Document
{
    public static readonly string FILE_NAME = "crop";

    // ���� ���� �����, json �迭 �����Ͱ����ϴ�.
    private static Dictionary<int, CropScript> dataDic = null;
    private static List<CropScript> dataList = null;

    public static bool Load()
    {
        try
        {
            dataDic = new Dictionary<int, CropScript>();
            dataList = new List<CropScript>();

            CropScript[] cropScripts = Extract<CropScript>(FILE_NAME);
            if (cropScripts == null || cropScripts.Length <= 0)
                throw new Exception("unitScripts is Empty");

            foreach (var script in cropScripts)
            {
                if (dataDic.ContainsKey(script.seq))
                    throw new Exception("same key found in unitScripts");

                dataDic.Add(script.seq, script);
                dataList.Add(script);
            }
        }
        catch (Exception e)
        {
#if UNITY_EDITOR
            Debug.LogError("[Message]\n" + e.Message + "\n[StackTrace]\n" + e.StackTrace);
#endif
            dataDic = null;
            dataList = null;
            return false;
        }

        return true;
    }
    public static CropScript Get(int id)
    {
        if (dataDic.TryGetValue(id, out CropScript data))
            return data;

        return null;
    }

    // ���� ���� �����, json ������ ����� ���� �˴ϴ�.
    public int seq { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public float hp { get; set; }
    public float speed { get; set; }
    public float atk { get; set; }
    public float reach { get; set; }
    public float time { get; set; }

}
