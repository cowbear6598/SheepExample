using UnityEngine;

namespace TimeSystem
{
    public interface ITimeService
    {
        float GetTime();
        float GetDeltaTime();
    }
    
    public class TimeService : ITimeService
    {
        public float GetTime() => Time.time;
        public float GetDeltaTime() => Time.deltaTime;
    }
}