using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class Trap : MonoBehaviour
    {
        public virtual void SetStart(Transform cPlayer) { StartCall(cPlayer); }
        protected virtual void StartCall(Transform cPlayer) { /* Implement in child class */ }
    }
}
