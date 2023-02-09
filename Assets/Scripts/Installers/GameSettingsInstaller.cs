using System;
using Card;
using Game;
using Holder;
using UnityEngine;
using Zenject;

namespace Installers
{
    [CreateAssetMenu(menuName = "Data/GameSettings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField] private GameSettings   gameSettings;
        [SerializeField] private CardSettings   cardSettings;
        [SerializeField] private HolderSettings holderSettings;
        [SerializeField] private LevelSettings  levelSettings;


        public override void InstallBindings()
        {
            Container.BindInstance(gameSettings.scoreSettings).IfNotBound();

            Container.BindInstance(cardSettings.typeSettings).IfNotBound();

            Container.BindInstance(holderSettings.cardSettings).IfNotBound();
            
            Container.BindInstance(levelSettings.levelSettings).IfNotBound();
        }

        [Serializable]
        public class GameSettings
        {
            public GameScoreHandler.Settings scoreSettings;
        }

        [Serializable]
        public class CardSettings
        {
            public CardTypeHandler.Settings typeSettings;
        }

        [Serializable]
        public class HolderSettings
        {
            public HolderCardHandler.Settings cardSettings;
        }
        
        [Serializable]
        public class LevelSettings
        {
            public GameLevelHandler.Settings levelSettings;
        }
    }
}