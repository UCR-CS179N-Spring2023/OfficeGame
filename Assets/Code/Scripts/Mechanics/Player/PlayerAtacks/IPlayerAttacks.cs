using UnityEngine;

namespace PlayerMechanics
{
    public interface IPlayerAttacks
    {
        public bool attacked { get; }
        public bool ranged { get;  }
        public bool hit { get; }
    }
}
