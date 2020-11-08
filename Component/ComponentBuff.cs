using Game;
using GameEntitySystem;
using System.Collections.Generic;
using TemplatesDatabase;
using Engine;
using System;

namespace Mekiasm
{
    //buff系统
    public class ComponentBuff:Component,IUpdateable
    {
        public ComponentPlayer componentPlayer;
        public List<mProgessWidget> mProgessWidgets = new List<mProgessWidget>();
        public StackPanelWidget buffView = new StackPanelWidget() { Direction=LayoutDirection.Horizontal,HorizontalAlignment=WidgetAlignment.Center};
        public int UpdateOrder => 999;
        public double lastTime = 0;
        public int maxtime = 20;
        public List<Buff> buffList = new List<Buff>();
        public float baseSpeed = 0f;
        public SubsystemTime subsystemTime;
        public struct Buff {
            public int TotalTime;
            public int RemainTime;
            public BuffTYpe bufftype;
            public bool isSet;
        }        
        public enum BuffTYpe {
            N0, 
            N1, 
            HealthNumUp,
            MoveSpeedUp,
            N4,
            DenfendNumUP,
            N6,
            N7,
            N8,
            N9,
            N10,
            N11
        }
        public Buff makeBuff(int tTime,int Rtime,BuffTYpe buffTYpe,bool isset) {
            Buff buff = new Buff() {TotalTime=tTime,RemainTime=Rtime,bufftype=buffTYpe,isSet=isset};
            return buff;        
        }
        public override void Load(ValuesDictionary valuesDictionary, IdToEntityMap idToEntityMap)
        {
            componentPlayer = Entity.FindComponent<ComponentPlayer>();
            componentPlayer.ComponentGui.ControlsContainerWidget.Children.Add(buffView);
            subsystemTime = Project.FindSubsystem<SubsystemTime>();
            buffList.Add(makeBuff(maxtime,maxtime,BuffTYpe.MoveSpeedUp,false));
            buffList.Add(makeBuff(40, 40, BuffTYpe.DenfendNumUP,false));
            buffList.Add(makeBuff(60, 60, BuffTYpe.HealthNumUp,false));
            base.Load(valuesDictionary, idToEntityMap);
        }
        public void doAc() {
            for(int i=0;i<buffList.Count;i++)
            {
                buffList[i]=makeBuff(buffList[i].TotalTime, buffList[i].RemainTime-1, buffList[i].bufftype, buffList[i].isSet);
                if (buffList[i].RemainTime - 1 <= 0) { 
                    buffList.RemoveAt(i);                     
                    componentPlayer.ComponentLocomotion.WalkSpeed = baseSpeed;
                }
                Log.Information("sp:"+baseSpeed);
            }
        }
        public void Update(float dt)
        {
            buffView.Children.Clear();
            for(int i= 0; i < buffList.Count; i++) {
                BuffView abuffView = new BuffView();
                abuffView.setData(buffList[i]);
                buffView.Children.Add(abuffView);
                if (!buffList[i].isSet&& buffList[i].bufftype==BuffTYpe.MoveSpeedUp)
                {
                    baseSpeed = componentPlayer.ComponentLocomotion.WalkSpeed;
                    componentPlayer.ComponentLocomotion.WalkSpeed = baseSpeed * 2f;
                    buffList[i] = makeBuff(buffList[i].TotalTime, buffList[i].RemainTime - 1, buffList[i].bufftype,true);
                }
            }
            if (subsystemTime.PeriodicGameTimeEvent(1d,0d)) {
                doAc();
            }
        }
    }
}
