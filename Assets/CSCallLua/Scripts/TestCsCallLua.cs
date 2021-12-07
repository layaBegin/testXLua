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
        //注意：Lua 文件是从上到下顺序执行的
        //Lua文件必须以.lua.txt 结尾-----------
        //且必须放在Resources文件夹下，如果不想放在Resources下，可以自定义加载器

        //string lua = "print('Hello XLua!')";//这是一个lua的语句
        //luaenv.DoString(lua);

        //luaenv.DoString("require('test1')");//文件名必须是.lua.txt的后缀，必须放在Resources文件夹下,获取Lua文件


        //luaenv.AddLoader(TestLoader);//注册一个自定义的加载器,注册之后所有的脚本都得放里面
        //luaenv.DoString("require('testLoader')");


        luaenv.DoString("require('testCsCallLua')");

        //-----获取lua里面的参数-------------------
        //int a = luaenv.Global.Get<int>("a");
        //Debug.Log("==获取到变量："+a);
        //string b = luaenv.Global.Get<string>("b");
        //Debug.Log("==获取到变量b：" + b);
        //bool c = luaenv.Global.Get<bool>("c");
        //Debug.Log("==获取到变量c：" + c);

        //-----映射table类型
        //1. 把table映射到一个类或者结构体中
        //只会把table中字符串索引的元素映射到class里，映射到索引名字能与class的公共字段名对应上的字段上
        //如果table的元素多，多出来的不考虑，如果table元素少，class的多出来的字段就默认值
        //方法不能映射过来, 属于值传递,所以搞个方法去接收它
        //TestTable ta = luaenv.Global.Get<TestTable>("ta");
        //使用接口取接 table
        //ITestTable ta = luaenv.Global.Get<ITestTable>("ta");
        //Debug.Log(ta.key + " -- " + ta.key2 + " --- " + ta.key3);
        //ta.func1();

        

        //3. 映射到 字典 或 List中  值传递
        //映射到字典时，只能把table中索引的类型于字典键的类型一致，并对应的值的类型也一致的情况才能映射过来,其他的忽略掉
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

        //4. 映射到LuaTable类型中,通过Get方法获取值
        //LuaTable table = luaenv.Global.Get<LuaTable>("ta");
        //string key = table.Get<string>("key");
        //bool key1 = table.Get<bool>("key3");
        //Debug.Log("---key:" + key);
        //Debug.Log("---key1:" + key1);
        //Func1 tableFunc = table.Get<Func1>("func1");
        //tableFunc();

        //方法映射到C#
        //1.使用 delegate
        //Func1 func = luaenv.Global.Get<Func1>("globalfunc1");
        //func();
        //Func2 func2 = luaenv.Global.Get<Func2>("globalfunc2");
        //Debug.Log(func2());
        //Func3 func3 = luaenv.Global.Get<Func3>("globalfunc3");
        //func3("之后，又疼了");

        //当lua中多返回值的情况，使用out ref参数接收多个返回值
        //如果委托有返回值那么多返回值的第一个是返回值，然后从第二个开始依次对应参数列表里的out或ref参数
        //如果委托没有返回值，从第一个参数依次对应参数列表的out或ref参数
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

        //2.使用LuaFunction 映射方法, 速度比委托要慢
        //LuaFunction luafunc = luaenv.Global.Get<LuaFunction>("globalfunc1");
        //luafunc.Call();

        //LuaFunction luafunc = luaenv.Global.Get<LuaFunction>("global");
        //luafunc.Call( new string[] { "呵呵哒", "ds", "sdf" });
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

    //注意，如果要使用接口去接受table那么必须加这个特性s
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
