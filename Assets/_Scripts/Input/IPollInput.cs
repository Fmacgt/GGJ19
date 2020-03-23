
using System.Collections;
using UnityEngine;


namespace GGJ2019
{
    public abstract class IPollInput : MonoBehaviour
    {
        public abstract TouchInputSet poll();
        public abstract void resetInputStates();
    }
}
