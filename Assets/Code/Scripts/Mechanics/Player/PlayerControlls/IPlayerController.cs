using UnityEngine;

namespace PlayerMechanics
{
    public interface IPlayerController
    {
        public Vector2 velocity { get; }
        public Vector2 position { get; }
        public bool jumpingThisFrame { get; }
        public bool landingThisFrame { get; }
        public bool grounded { get; }
        public bool moveEnabled {get; set;}
        public bool controlEnabled {get; set;}

        public void Teleport(Vector2 moveTo);
        public void KnockBack(Vector2 knockBack);
    }
}
