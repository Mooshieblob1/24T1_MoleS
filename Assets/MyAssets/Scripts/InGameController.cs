using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoleSurvivor
{
    public class InGameController : MonoBehaviour
    {
        public static InGameController Instance { get; private set; }

        private void Awake()
        {
            //If there is more than one instance, destroy the extra else Set the static instance to this instance
            if (Instance != null && Instance != this) { Destroy(this); } else { Instance = this; }
        }

        [Header("CHECK PLAYER")]
        public PlayerController player1;
        public PlayerController player2;
        public PlayerController player3;
        public PlayerController player4;

        public bool p1Active;
        public bool p2Active;
        public bool p3Active;
        public bool p4Active;

        public bool soloMode;

        [Space]

        [Header("TILE CONTROLS")]
        public TileDestroyer tileTypeDestroy;

        void Start()
        {
            if (player1 != null && p1Active) { SetStartPlayer(player1); }
            if (player2 != null && p2Active) { SetStartPlayer(player2); }
            if (player3 != null && p3Active) { SetStartPlayer(player3); }
            if (player4 != null && p4Active) { SetStartPlayer(player4); }
        }

        void Update()
        {
            if (player1 != null && p1Active) player1.SetUpdate();
            if (player2 != null && p2Active) player2.SetUpdate();
            if (player3 != null && p3Active) player3.SetUpdate();
            if (player4 != null && p4Active) player4.SetUpdate();
        }

        void SetStartPlayer(PlayerController player)
        {
            player.singleMovement = soloMode;
            player.SetStart();
            player.InputLeftDownUpdate(KeyCode.LeftArrow);
            player.InputRightDownUpdate(KeyCode.RightArrow);
        }
    }
}
