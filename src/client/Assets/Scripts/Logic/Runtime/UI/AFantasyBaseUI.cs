using System;
using AnKuchen.Map;
using UnityEngine;

namespace Fantasy.Logic.Achieve
{
    public abstract class AFantasyBaseUI: MonoBehaviour
    {
        protected UICache GameRootUICache;
        public bool IsOpen;

        private void __internalAwake()
        {
            GameRootUICache = GetComponent<UICache>();
          
        }

     
        public  virtual void Display()
        {
            
            
        }

        public  virtual void Close()
        {
            
        }
    }
}