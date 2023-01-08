using System;
using UnityEngine.Events;

namespace Assets
{
    [Serializable]
    public class HealthChangeEvent : UnityEvent<int, int>
    {
    }
}
