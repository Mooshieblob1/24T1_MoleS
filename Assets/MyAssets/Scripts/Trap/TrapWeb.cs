using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class TrapWeb : Trap
    {
        public int min, max; // Minimum and maximum values for currentInt

        public int currentInt; // Current value of the trap
        private Transform player; // Reference to the player's transform
        private bool canGoOpposite = true; // Flag to determine if the player can go in the opposite direction

        private bool cBeforeOrAfter; // Flag to check if the trap should be activated before or after the player

        void Start()
        {
            IsCheckTile(transform.position); // Check if there are any tiles to destroy at the trap's position
        }

        protected override void StartCall(Transform cPlayer)
        {
            if (checkBeforeOrAfter == true)
            {
                currentInt = Random.Range(min, max + 1); // Set a random value for currentInt within the specified range
                player = cPlayer; // Set the player reference
                StartCoroutine(CorUpdate()); // Start the coroutine to update the trap
            }
        }

        public IEnumerator CorUpdate()
        {
            while (currentInt > 0)
            {
                player.GetComponent<PlayerController>().isAllowedToMove = false; // Disable player movement
                player.GetComponent<PlayerController>().orientation.rotation = Quaternion.Euler(new Vector3(0, 0, 0)); // Reset player rotation

                if (player.GetComponent<PlayerController>()._inputM.x < 0 && canGoOpposite)
                {
                    currentInt--; // Decrease currentInt
                    canGoOpposite = false; // Set to false so the player can't go right next time
                }
                else if (player.GetComponent<PlayerController>()._inputM.x > 0 && !canGoOpposite)
                {
                    currentInt--; // Decrease currentInt
                    canGoOpposite = true; // Set to true so the player can go right next time
                }

                yield return null;
            }

            player.GetComponent<PlayerController>().isAllowedToMove = true; // Enable player movement
            player = null; // Reset player reference
            this.gameObject.SetActive(false); // Deactivate the trap game object
        }

        void IsCheckTile(Vector3 targetPos)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(targetPos, 0.3f); // Get all colliders within a circle at targetPos with radius 0.3f

            foreach (Collider2D c in colliders)
            {
                if (c.GetComponent<TileDestroyer>() != null)
                {
                    TileDestroyer coll = c.GetComponent<TileDestroyer>();
                    coll.DestroyTile(targetPos); // Destroy the tile at targetPos
                }
            }
        }
    }
}
