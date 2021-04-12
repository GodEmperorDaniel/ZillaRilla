using UnityEngine;

namespace Entities.Commands
{
    public abstract class Command : MonoBehaviour
    {
        public virtual void Execute()
        { }
    }
}