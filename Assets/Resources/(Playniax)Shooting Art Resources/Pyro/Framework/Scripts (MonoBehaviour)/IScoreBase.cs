using UnityEngine;

namespace Playniax.Pyro.Framework
{
    public interface IScoreBase
    {
        bool enabled
        {
            get;
            set;
        }
        GameObject friend
        {
            get;
            set;
        }
        GameObject gameObject { get; }
        bool isActiveAndEnabled { get; }

        bool indestructible
        {
            get;
            set;
        }
        GameObject isTargeted
        {
            get;
            set;
        }
        bool isVisible
        {
            get;
        }
        public string material
        {
            get;
            set;
        }
        public int points
        {
            get;
        }
        int structuralIntegrity
        {
            get;
            set;
        }
        void DoDamage(int damage);
        void Ghost();
        void Kill();
    }
}
