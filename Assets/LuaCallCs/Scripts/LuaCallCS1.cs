using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
public class LuaCallCS1 : MonoBehaviour
{
    LuaEnv luaenv = new LuaEnv();

    [CSharpCallLua]
    public delegate void Diopose();
    // Start is called before the first frame update
    void Start()
    {
        luaenv.DoString("require('LuaCallCS1')");

        //TestDelegate.del += test1;
        //TestDelegate.del();
    }

    void test1()
    {
        Debug.Log("===测试委托");
    }

    private void OnDestroy()
    {
        //Diopose disposeDel = luaenv.Global.Get<Diopose>("DisposeDel");
        //disposeDel();
        ////这个方法也要释放
        //disposeDel = null;

        //换这种方法释放lua中的委托，否则报错
        LuaFunction luaFunction = luaenv.Global.Get<LuaFunction>("DisposeDel");
        luaFunction.Call();
        //luaFunction = null;
        //释放虚拟机
        luaenv.Dispose();
    }
}

namespace MyType
{
    //lua要调用，枚举前需加上此特性
    [LuaCallCSharp]
    public enum PlayerType
    {
        ZhangShan,
        LiShi,
        WangWu,
        ZhaoLiu,
    }

}

[LuaCallCSharp]
public class TestEnum
{
    public static void Method(MyType.PlayerType type)
    {
        Debug.Log("TestEnum Method :" + type);
    }
}

[LuaCallCSharp]
public class TestDelegate
{
    public delegate void Del();
    public static Del del;//一个静态的委托变量
    public static void Test()
    {
        Debug.Log("TestDelegate Test");
    }
    public static void Test1()
    {
        Debug.Log("TestDelegate Test1");
    }
}

[LuaCallCSharp]
public class TestDelegate1
{
    public delegate void Del1() ;

    public Del1 del1;

    public void test()
    {
        Debug.Log("===成员委托");
    }
}

[LuaCallCSharp]
public class TestCS1
{
    /// <summary>
    /// 带有默认参数的方法
    /// </summary>
    /// <param name="name"></param>
    public static void Test(string name = "张山")
    {
        Debug.Log(name);
    }


    /// <summary>
    /// 一个可变参数的方法
    /// </summary>
    /// <param name="arg"></param>
    public static void Test2(params string[] arg)
    {
        foreach (var item in arg)
        {
            Debug.Log(item);
        }
    }
}
