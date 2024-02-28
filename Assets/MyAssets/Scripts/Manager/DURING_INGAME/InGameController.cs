using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MoleSurvivor
{
    public class InGameController : MonoBehaviour
    {
        public static InGameController Instance { get; private set; }

        private void Awake()
        {
            //If there is more than one instance, destroy the extra else Set the static instance to this instance
            if (Instance != null && Instance != this) { Destroy(this.gameObject); } else { Instance = this; DontDestroyOnLoad(gameObject); }
        }

        [Header("INPUT MANAGER")]
        public InputManager inputManager;

        [Header("MAIN CAMERA")]
        public Transform inGameCamera;
        public int eachLayerSize = 30;
        public int endGoal;
        public float duration;

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

        void Start()
        {
            if (player1 != null && p1Active) { SetStartPlayer(player1, inputManager.p1_Left, inputManager.p1_Right); }
            if (player2 != null && p2Active) { SetStartPlayer(player2, inputManager.p2_Left, inputManager.p2_Right); }
            if (player3 != null && p3Active) { SetStartPlayer(player3, inputManager.p3_Left, inputManager.p3_Right); }
            if (player4 != null && p4Active) { SetStartPlayer(player4, inputManager.p4_Left, inputManager.p4_Right); }

            // Start the coroutine
            StartCoroutine(MoveCameraCoroutine());
        }

        void Update()
        {
            if (player1 != null && p1Active) player1.SetUpdate();
            if (player2 != null && p2Active) player2.SetUpdate();
            if (player3 != null && p3Active) player3.SetUpdate();
            if (player4 != null && p4Active) player4.SetUpdate();

            //// Use DoTween for movement
            //Sequence mySequence = DOTween.Sequence();
            //mySequence.Append(inGameCamera.DOMove(new Vector3(0, -endGoal, 0), speedCamera));
        }

        IEnumerator MoveCameraCoroutine()
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = inGameCamera.transform.position;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.fixedDeltaTime;
                inGameCamera.transform.position = Vector3.Lerp(startingPosition, new Vector3(0, endGoal, 0), elapsedTime / duration);
                yield return new WaitForFixedUpdate();
            }

            // Ensure the camera reaches exactly the target position
            inGameCamera.transform.position = new Vector3(0, endGoal, 0);
        }

        void SetStartPlayer(PlayerController player, KeyCode leftControl, KeyCode rightControl)
        {
            player.singleMovement = soloMode;
            player.SetStart();

            player.InputLeftDownUpdate(leftControl);
            player.InputRightDownUpdate(rightControl);
        }
    }
}
