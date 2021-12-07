using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;



public class TestCsCallLua : MonoBehaviour
{
    LuaEnv luaenv = new LuaEnv();
    [CSharpCallLua]
    public delegate void Func1();
    [CSharpCallLua]
    public delegate string Func2();
    [CSharpCallLua]
    public delegate void Func3(string str);
    
    [CSharpCallLua]
    public delegate void Func4(ref string str1, out string str43, out bool boo1, out string str2);
    [CSharpCallLua]
    public delegate void Func5(params string[] args);
    void Start()
    {
        //ע�⣺Lua �ļ��Ǵ��ϵ���˳��ִ�е�
        //Lua�ļ�������.lua.txt ��β-----------
        //�ұ������Resources�ļ����£�����������Resources�£������Զ��������

        //string lua = "print('Hello XLua!')";//����һ��lua�����
        //luaenv.DoString(lua);

        //luaenv.DoString("require('test1')");//�ļ���������.lua.txt�ĺ�׺���������Resources�ļ�����,��ȡLua�ļ�


        //luaenv.AddLoader(TestLoader);//ע��һ���Զ���ļ�����,ע��֮�����еĽű����÷�����
        //luaenv.DoString("require('testLoader')");


        luaenv.DoString("require('testCsCallLua')");

        //-----��ȡlua����Ĳ���-------------------
        //int a = luaenv.Global.Get<int>("a");
        //Debug.Log("==��ȡ��������"+a);
        //string b = luaenv.Global.Get<string>("b");
        //Debug.Log("==��ȡ������b��" + b);
        //bool c = luaenv.Global.Get<bool>("c");
        //Debug.Log("==��ȡ������c��" + c);

        //-----ӳ��table����
        //1. ��tableӳ�䵽һ������߽ṹ����
        //ֻ���table���ַ���������Ԫ��ӳ�䵽class�ӳ�䵽������������class�Ĺ����ֶ�����Ӧ�ϵ��ֶ���
        //���table��Ԫ�ض࣬������Ĳ����ǣ����tableԪ���٣�class�Ķ�������ֶξ�Ĭ��ֵ
        //��������ӳ�����, ����ֵ����,���Ը������ȥ������
        //TestTable ta = luaenv.Global.Get<TestTable>("ta");
        //ʹ�ýӿ�ȡ�� table
        //ITestTable ta = luaenv.Global.Get<ITestTable>("ta");
        //Debug.Log(ta.key + " -- " + ta.key2 + " --- " + ta.key3);
        //ta.func1();

        

        //3. ӳ�䵽 �ֵ� �� List��  ֵ����
        //ӳ�䵽�ֵ�ʱ��ֻ�ܰ�table���������������ֵ��������һ�£�����Ӧ��ֵ������Ҳһ�µ��������ӳ�����,�����ĺ��Ե�
        //Dictionary<object, object> dic = luaenv.Global.Get<Dictionary<object, object>>("ta");
        //foreach (var item in dic.Keys)
        //{
        //    Debug.Log("key: " + item + "  vlaue: " + dic[item]);
        //}

        //List<int> t = luaenv.Global.Get<List<int>>("ta");
        //foreach(var item in t)
        //{
        //    Debug.Log("item:"+item);
        //}

        //4. ӳ�䵽LuaTable������,ͨ��Get������ȡֵ
        //LuaTable table = luaenv.Global.Get<LuaTable>("ta");
        //string key = table.Get<string>("key");
        //bool key1 = table.Get<bool>("key3");
        //Debug.Log("---key:" + key);
        //Debug.Log("---key1:" + key1);
        //Func1 tableFunc = table.Get<Func1>("func1");
        //tableFunc();

        //����ӳ�䵽C#
        //1.ʹ�� delegate
        //Func1 func = luaenv.Global.Get<Func1>("globalfunc1");
        //func();
        //Func2 func2 = luaenv.Global.Get<Func2>("globalfunc2");
        //Debug.Log(func2());
        //Func3 func3 = luaenv.Global.Get<Func3>("globalfunc3");
        //func3("֮��������");

        //��lua�ж෵��ֵ�������ʹ��out ref�������ն������ֵ
        //���ί���з���ֵ��ô�෵��ֵ�ĵ�һ���Ƿ���ֵ��Ȼ��ӵڶ�����ʼ���ζ�Ӧ�����б����out��ref����
        //���ί��û�з���ֵ���ӵ�һ���������ζ�Ӧ�����б��out��ref����
        //Func4 func4 = luaenv.Global.Get<Func4>("globalfunc4");
        //string str1 = "";
        //string str = "";
        //bool b1 = false;
        //string str2 = "";
        //func4(ref str1, out str, out b1, out str2);
        //Debug.Log(str1);
        //Debug.Log(str);
        //Debug.Log(b1);
        //Debug.Log(str2);

        //Func5 func5 = luaenv.Global.Get<Func5>("global");
        //func5("str", "jave", "C#");

        //2.ʹ��LuaFunction ӳ�䷽��, �ٶȱ�ί��Ҫ��
        //LuaFunction luafunc = luaenv.Global.Get<LuaFunction>("globalfunc1");
        //luafunc.Call();

        //LuaFunction luafunc = luaenv.Global.Get<LuaFunction>("global");
        //luafunc.Call( new string[] { "�Ǻ���", "ds", "sdf" });
        //LuaFunction luafunc4 = luaenv.Global.Get<LuaFunction>("globalfunc4");
        //object[] obj_arr = luafunc4.Call();
        //foreach (var item in obj_arr)
        //{
        //    Debug.Log(item);
        //}
    }

    private void OnDestroy()
    {
        luaenv.Dispose();
    }

    private byte[] TestLoader(ref string fileName)
    {
        string path = Application.streamingAssetsPath + "/" + fileName + ".lua.txt";

        StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8);
        try
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(sr.ReadToEnd());
            return bytes;
        }
        catch (System.Exception)
        {
        }
        return null;
    }

    //ע�⣬���Ҫʹ�ýӿ�ȥ����table��ô������������s
    [CSharpCallLua]
    interface ITestTable
    {
        string key { get; set; }
        string key2 { get; set; }
        bool key3 { get; set; }
        void func1();
    }
}

class TestTable
{
    public string key;
    public string key2;
    public bool key3;
    public bool key4;

    public void func1()
    {

    }

    public void Test()
    {
        Debug.Log(this.key + " -- " + this.key2 + " --- " + this.key3 + " --- " + this.key4);
    }
}
