using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XLua;



namespace testEvent
{
    [System.Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
    }

    [LuaCallCSharp]
    public class LuaEvent : MonoBehaviour
    {
        public TextAsset luaScript;
        public Injection[] injections;

        internal static LuaEnv luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
        internal static float lastGCTime = 0;
        internal const float GCInterval = 1;//1 second 

        private Action luaStart;
        private Action luaUpdate;
        private Action luaOnDestroy;

        private LuaTable scriptEnv;

        //public GameObject cube;
        // Start is called before the first frame update
        private void Awake()
        {
            scriptEnv = luaEnv.NewTable();

            // Ϊÿ���ű�����һ�������Ļ�������һ���̶��Ϸ�ֹ�ű���ȫ�ֱ�����������ͻ
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("this", this);
            //���������д��Lua
            foreach (var injection in injections)
            {
                scriptEnv.Set(injection.name, injection.value);
            }

            luaEnv.DoString(luaScript.text, luaScript.name, scriptEnv);

            luaStart = luaEnv.Global.Get<Action>("start");
            luaUpdate = luaEnv.Global.Get<Action>("update");
            luaOnDestroy = luaEnv.Global.Get<Action>("destroy");

            Action luaAwake = scriptEnv.Get<Action>("awake");
            scriptEnv.Get("start", out luaStart);
            scriptEnv.Get("update", out luaUpdate);
            scriptEnv.Get("ondestroy", out luaOnDestroy);

            if (luaAwake != null)
            {
                luaAwake();
            }
            //Button btn = (Button)this.transform.GetComponent(typeof(UnityEngine.UI.Button));
            //btn.onClick.AddListener(onBtnClick);
        }

        void methodTestHander()
        {
            Debug.Log("����Event����");
        }

        void Start()
        {
            if (luaStart != null)
            {
                luaStart();
            }
            //Cat cat = new Cat();
            //Mouse m = new Mouse();
            //People p = new People();
            //������ 
            //cat.CatCall += new CatCallEventHandler(m.MouseRun);
            //cat.CatCall += new CatCallEventHandler(p.WakeUp);
            //cat.OnCatCall();

            //Rigidbody ri = (Rigidbody)this.gameObject.AddComponent(typeof(UnityEngine.Rigidbody));
            //ri.useGravity

            //sp = Resources.Load("Textures/zhaomin", typeof(UnityEngine.Sprite))

             //image.sprite = sp


        }

        // Update is called once per frame
        void Update()
        {
            //this.cube.transform.Rotate(UnityEngine.Vector3.up, UnityEngine.Time.deltaTime * 200);
            if (luaUpdate != null)
            {
                luaUpdate();
            }
            if (Time.time - LuaEvent.lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                LuaEvent.lastGCTime = Time.time;
            }
        }

        private void OnDestroy()
        {
            if (luaOnDestroy != null)
            {
                luaOnDestroy();
            }
            luaStart = null;
            luaUpdate = null;
            luaOnDestroy = null;
            scriptEnv.Dispose();
            injections = null;
        }
    }



    //����è��ί��
    public delegate void CatCallEventHandler();

    [LuaCallCSharp]
    public class Cat
    {
        //1.�¼�(event)��ί��(delegate)���͵�һ��ʵ����ί��(delegate)��һ�����ͣ�
        //2.����Ȩ�ޣ�ֻ�������¼�����������ȥinvoke�͸�ֵ�����������棬����������á�
        //����è���¼�
        public event CatCallEventHandler CatCall;
        public void OnCatCall()
        {
            Debug.Log("è����һ��");
            //CatCall?.Invoke();�ȼ�����������д��
            if(CatCall != null)
            {
                CatCall();
            }
        }
    }

    [LuaCallCSharp]
    public class Mouse
    {
        //���������ܵ�����
        public void MouseRun()
        {
            Debug.Log("��������");
        }
    }

    [LuaCallCSharp]
    public class People
    {
        //����������������
        public void WakeUp()
        {
            Debug.Log("��������");
        }
    }


}




