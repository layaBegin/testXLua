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
        Debug.Log("===����ί��");
    }

    private void OnDestroy()
    {
        //Diopose disposeDel = luaenv.Global.Get<Diopose>("DisposeDel");
        //disposeDel();
        ////�������ҲҪ�ͷ�
        //disposeDel = null;

        //�����ַ����ͷ�lua�е�ί�У����򱨴�
        LuaFunction luaFunction = luaenv.Global.Get<LuaFunction>("DisposeDel");
        luaFunction.Call();
        //luaFunction = null;
        //�ͷ������
        luaenv.Dispose();
    }
}

namespace MyType
{
    //luaҪ���ã�ö��ǰ����ϴ�����
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
    public static Del del;//һ����̬��ί�б���
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
        Debug.Log("===��Աί��");
    }
}

[LuaCallCSharp]
public class TestCS1
{
    /// <summary>
    /// ����Ĭ�ϲ����ķ���
    /// </summary>
    /// <param name="name"></param>
    public static void Test(string name = "��ɽ")
    {
        Debug.Log(name);
    }


    /// <summary>
    /// һ���ɱ�����ķ���
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
