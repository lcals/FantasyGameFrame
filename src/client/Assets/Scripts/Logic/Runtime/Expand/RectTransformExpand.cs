using UnityEngine;

namespace Fantasy.Logic.Achieve
{
    public static class RectTransformExpand
    {
        private static readonly Vector2Int SizeDelta = new(1920, 1080);
        public static void Init(this  RectTransform rectTransform)
        {
            rectTransform.localScale=Vector3.one;
            rectTransform.localRotation=Quaternion.identity;
            rectTransform.localPosition=Vector3.zero;
            rectTransform.sizeDelta = SizeDelta;
        }
    }
}