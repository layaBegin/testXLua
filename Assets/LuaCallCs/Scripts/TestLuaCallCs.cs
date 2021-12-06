using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class TestLuaCallCs : MonoBehaviour
{
    public string str = "传this调用mono属性";

    LuaEnv luaenv = new LuaEnv();

    //System.Action Ad;
    [CSharpCallLua]
    public delegate void Del(TestLuaCallCs lua);
    //Del de;
    [CSharpCallLua]
    public delegate void Del1();

    void Awake()
    {
        //调用静态类
        luaenv.DoString("require('LuaTest1')");
       



        //GameObject obj = Resources.Load<GameObject>("Cube");
        //Instantiate(obj);

        //调用成员属性和成员方法
        //调用成员变量或属性   对象名.变量名或属性名
        //调用成员方法  对象名：方法名()
        //xlua支持派生类的对象调用父类的方法

        //TestB testB = new TestB();
        //testB.method1();
        //Debug.Log("name:"+testB.name);


        //1.通过方法的形式把mono的类对象传递给luas
        //Del set = luaenv.Global.Get<Del>("SetCS");
        //Del1 deb = luaenv.Global.Get<Del1>("PrintStr");  //get方法是将lua里 的函数或字段映射为C#里对应的方法或字段 
        //set(this);  
        //deb();

        //2. 通过 luaenv.Global.Set 的方式向lua里设置一个全局的变量，
        //第一参数是lua的全局变量的名字，第二参数是lua的值
        //luaenv.Global.Set("mono", this);
        //Del1 deb = luaenv.Global.Get<Del1>("PrintLXM1");
        //deb();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//[LuaCallCSharp] 特性：如果想在Lua中调用C#，需使用此特性，可以生成lua中间代码，
//如果不加此特性，不会报错，但会使用反射调用，效率低，而且有些系统不支持反射
[LuaCallCSharp]
public class A
{
    public static string name = "A";

    public static void Method(string a, bool b)
    {
        Debug.Log(a + b);
    }

    public static string Method1(ref string a)
    {
        Debug.Log("C# Method1: " + a);
        return "method1返回值";
    }

    public static void Method2(out string a, string b)
    {
        a = "Method2";
        Debug.Log("C# Method2 out参数: " + a );
        Debug.Log("C# Method2 b: " + b);
    }

    public static void SetOut(out string a)
    {
        a = "SetOut";
    }

    public static void Method3()
    {
        Debug.Log("A Method3()");
    }

    public static void Method3(string a)
    {
        Debug.Log("A Method3() string :a" + a);
    }

    public static void Method3(float a)
    {
        Debug.Log("A Method3() float :a" + a);
    }

    public static void Method3(int a)
    {
        Debug.Log("A Method3() int :a" + a);
    }
}
[LuaCallCSharp]
class TestB : TestC
{
    public string name;


    public void method1()
    {
        Debug.Log("小神童~");
    }

    public TestB()
    {
        this.name = "zhangshan";
    }
}

[LuaCallCSharp]
public class TestC
{
    public void CMethod()
    {
        Debug.Log("调用了父类方法");
    }
}
