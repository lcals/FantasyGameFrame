using UnityEngine.Events;
using UnityEngine.UI;

namespace Fantasy.Logic.Achieve
{
    public static class ButtonExpand
    {
        public static Button AddListener(this Button button, UnityAction call,bool removeAllListeners=false)
        {
            if (removeAllListeners)
            {
                button.onClick.RemoveAllListeners();
            }
            button.onClick.AddListener(call);
            return button;
        }
   


     
    }
}