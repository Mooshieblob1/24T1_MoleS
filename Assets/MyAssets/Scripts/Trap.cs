using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class Trap : MonoBehaviour
    {
        public enum TrapType { Boulder, Web };
        public TrapType trapType;

        public int min, max;
        public int currentInt;

        public Transform player;

        public void SetStart(Transform cPlayer) { currentInt = Random.Range(min, max + 1); player = cPlayer; StartCoroutine(CorUpdate()); }

        public IEnumerator CorUpdate()
        {
            while (currentInt > 0)
            {
                player.GetComponent<PlayerController>().isAllowedToMove = false;

                yield return null;
            }

            player.GetComponent<PlayerController>().isAllowedToMove = true;
            player = null;
            this.gameObject.SetActive(false);
        }
    }
}
