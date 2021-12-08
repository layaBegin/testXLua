using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;


namespace testUI
{

    [System.Serializable]
    public class Injection
    {
        public string name;
        public GameObject value;
    }

    [LuaCallCSharp]
    public class LuaUI : MonoBehaviour
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
           

        }

        // Update is called once per frame
        void Update()
        {
            //this.cube.transform.Rotate(UnityEngine.Vector3.up, UnityEngine.Time.deltaTime * 200);
            if (luaUpdate != null)
            {
                luaUpdate();
            }
            if (Time.time - LuaUI.lastGCTime > GCInterval)
            {
                luaEnv.Tick();
                LuaUI.lastGCTime = Time.time;
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
}



