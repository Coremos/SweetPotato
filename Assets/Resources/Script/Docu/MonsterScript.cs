using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : Document
{
    public static readonly string FILE_NAME = "monster";

    // 공용 변수 선언부, json 배열 데이터가들어갑니다.
    private static Dictionary<int, MonsterScript> dataDic = null;
    private static List<MonsterScript> dataList = null;

    public static bool Load()
    {
        try
        {
            dataDic = new Dictionary<int, MonsterScript>();
            dataList = new List<MonsterScript>();

            MonsterScript[] cropScripts = Extract<MonsterScript>(FILE_NAME);
            if (cropScripts == null || cropScripts.Length <= 0)
                throw new Exception("MonsterScript is Empty");

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
    public static MonsterScript Get(int id)
    {
        if (dataDic.TryGetValue(id, out MonsterScript data))
            return data;

        return null;
    }

    // 스택 변수 선언부, json 변수명 목록이 들어가면 됩니다.
    public int seq { get; set; }
    public string name { get; set; }
    public string type { get; set; }
    public float hp { get; set; }
    public float speed { get; set; }
    public float atk { get; set; }
    public float reach { get; set; }
}
