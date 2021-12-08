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

            // 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            scriptEnv.SetMetaTable(meta);
            meta.Dispose();

            scriptEnv.Set("this", this);
            //将面板数据写入Lua
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
            Debug.Log("测试Event函数");
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
            //关联绑定 
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



    //定义猫叫委托
    public delegate void CatCallEventHandler();

    [LuaCallCSharp]
    public class Cat
    {
        //1.事件(event)是委托(delegate)类型的一个实例，委托(delegate)是一种类型，
        //2.限制权限，只允许在事件声明类里面去invoke和赋值，不允许外面，甚至子类调用。
        //定义猫叫事件
        public event CatCallEventHandler CatCall;
        public void OnCatCall()
        {
            Debug.Log("猫叫了一声");
            //CatCall?.Invoke();等价于下面这种写法
            if(CatCall != null)
            {
                CatCall();
            }
        }
    }

    [LuaCallCSharp]
    public class Mouse
    {
        //定义老鼠跑掉方法
        public void MouseRun()
        {
            Debug.Log("老鼠跑了");
        }
    }

    [LuaCallCSharp]
    public class People
    {
        //定义主人醒来方法
        public void WakeUp()
        {
            Debug.Log("主人醒了");
        }
    }


}




