using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class TestLuaCallCs : MonoBehaviour
{
    public string str = "��this����mono����";

    LuaEnv luaenv = new LuaEnv();

    //System.Action Ad;
    [CSharpCallLua]
    public delegate void Del(TestLuaCallCs lua);
    //Del de;
    [CSharpCallLua]
    public delegate void Del1();

    void Awake()
    {
        //���þ�̬��
        luaenv.DoString("require('LuaTest1')");
       



        //GameObject obj = Resources.Load<GameObject>("Cube");
        //Instantiate(obj);

        //���ó�Ա���Ժͳ�Ա����
        //���ó�Ա����������   ������.��������������
        //���ó�Ա����  ��������������()
        //xlua֧��������Ķ�����ø���ķ���

        //TestB testB = new TestB();
        //testB.method1();
        //Debug.Log("name:"+testB.name);


        //1.ͨ����������ʽ��mono������󴫵ݸ�luas
        //Del set = luaenv.Global.Get<Del>("SetCS");
        //Del1 deb = luaenv.Global.Get<Del1>("PrintStr");  //get�����ǽ�lua�� �ĺ������ֶ�ӳ��ΪC#���Ӧ�ķ������ֶ� 
        //set(this);  
        //deb();

        //2. ͨ�� luaenv.Global.Set �ķ�ʽ��lua������һ��ȫ�ֵı�����
        //��һ������lua��ȫ�ֱ��������֣��ڶ�������lua��ֵ
        //luaenv.Global.Set("mono", this);
        //Del1 deb = luaenv.Global.Get<Del1>("PrintLXM1");
        //deb();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//[LuaCallCSharp] ���ԣ��������Lua�е���C#����ʹ�ô����ԣ���������lua�м���룬
//������Ӵ����ԣ����ᱨ������ʹ�÷�����ã�Ч�ʵͣ�������Щϵͳ��֧�ַ���
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
        return "method1����ֵ";
    }

    public static void Method2(out string a, string b)
    {
        a = "Method2";
        Debug.Log("C# Method2 out����: " + a );
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
        Debug.Log("С��ͯ~");
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
        Debug.Log("�����˸��෽��");
    }
}
