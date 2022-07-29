using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay {

    public class Environment : MonoBehaviour {

        public bool isGround;

        protected bool _isBraking;
        public bool IsBraking => _isBraking;

        public virtual Vector2 GetEnvironmentVelocity() {
            return new Vector2(0, 0);
        }
    }
}
