using System;
using UnityEngine;

namespace Game
{
    public class LevelView : MonoBehaviour
    {
        public int totalLevel;
        
        [Serializable]
        public class LevelData
        {
            public GameObject  groupObj;
            public Transform[] layerTrans;
        }

        public LevelData[] levelData = new LevelData[5];
    }
}