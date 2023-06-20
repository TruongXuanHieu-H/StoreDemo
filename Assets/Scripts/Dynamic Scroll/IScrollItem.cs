using UnityEngine;

namespace DynamicScroll
{
    public interface IScrollItem
    {
        void Reset();
        int CurrentIndex { get; set; }
        RectTransform RectTransform { get; }
    }
}