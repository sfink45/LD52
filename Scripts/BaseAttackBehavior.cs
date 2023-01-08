using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class BaseAttackBehavior : MonoBehaviour
    {
        protected bool _isStartUpPlaying;

        public bool IsDone { get; protected set; }

        public abstract void HandleUpdate(float timeDelta);

        public abstract void ResetBehavior();

        public abstract void StartupAttack(EnemyBaseController parent);
    }
}
