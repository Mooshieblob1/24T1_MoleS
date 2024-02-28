using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace MoleSurvivor
{
    public class InGameController : MonoBehaviour
    {
        public static InGameController Instance { get; private set; }

        private void Awake()
        {
            // Set Parent to Root
            transform.parent = null;

            //If there is more than one instance, destroy the extra else Set the static instance to this instance
            if (Instance != null && Instance != this) { Destroy(this.gameObject); } else { Instance = this; DontDestroyOnLoad(gameObject); }
        }

        private InputManager inputManager;
        private bool setActiveUpdate;

        [BoxGroup("BOX LEVEL SANDWITCH", false)]
        [Title("BOX LEVEL SANDWITCH/LEVEL SANDWITCH")]
        public Transform inGameCamera;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public Transform levelGridParent;
        
        [Space]

        [BoxGroup("BOX LEVEL SANDWITCH")]
        public Transform delayText;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public int countdownStartValue = 3;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public float delayBetweenCountdown = 1f;

        [Space]

        [BoxGroup("BOX LEVEL SANDWITCH")]
        public int eachLayerSize = 30;
        [BoxGroup("BOX LEVEL SANDWITCH")]
        public float levelDuration;

        [Space]

        [BoxGroup("BOX LEVEL SANDWITCH")]
        public Transform[] levelSandwitch;

        Transform[] currentlevelSandwitch;
        int endGoal;

        [BoxGroup("BOX PLAYER", false)]
        [TitleGroup("BOX PLAYER/CHECK PLAYER")]
        public PlayerController 
        player1,
        player2,
        player3,
        player4;

        [Space]

        [BoxGroup("BOX PLAYER")]
        public float playerMoveSpeed;
        [BoxGroup("BOX PLAYER")]
        public float playerRotateSpeed;
        [BoxGroup("BOX PLAYER")]
        public float playerRotateDelay;

        [Space]

        [BoxGroup("BOX PLAYER")]
        public bool p1Active;
        [BoxGroup("BOX PLAYER")]
        public bool p2Active;
        [BoxGroup("BOX PLAYER")]
        public bool p3Active;
        [BoxGroup("BOX PLAYER")]
        public bool p4Active;
        
        [Space]

        [BoxGroup("BOX PLAYER")]
        public bool soloMode;

        private void Start() 
        {
            inputManager = MainManager.Instance.inputManager;

            // First, ensure currentlevelSandwitch is properly initialized.
            currentlevelSandwitch = new Transform[levelSandwitch.Length];

            endGoal = -((levelSandwitch.Length * eachLayerSize) - eachLayerSize);

            // Then, instantiate the objects.
            for (int i = 0; i < levelSandwitch.Length; i++)
            {
                currentlevelSandwitch[i] = Instantiate(levelSandwitch[i]);
                currentlevelSandwitch[i].parent = levelGridParent;
                currentlevelSandwitch[i].transform.localPosition = new Vector3(0, -(i * eachLayerSize), 0);
            }

            if (player1 != null && p1Active) { player1.gameObject.SetActive(true); }
            if (player2 != null && p2Active) { player2.gameObject.SetActive(true); }
            if (player3 != null && p3Active) { player3.gameObject.SetActive(true); }
            if (player4 != null && p4Active) { player4.gameObject.SetActive(true); }

            StartCoroutine(CountdownRoutine(countdownStartValue, delayBetweenCountdown)); 
        }

        void SetStart()
        {
            if (player1 != null && p1Active) { SetStartPlayer(player1, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, inputManager.p1_Left, inputManager.p1_Right); }
            if (player2 != null && p2Active) { SetStartPlayer(player2, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, inputManager.p2_Left, inputManager.p2_Right); }
            if (player3 != null && p3Active) { SetStartPlayer(player3, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, inputManager.p3_Left, inputManager.p3_Right); }
            if (player4 != null && p4Active) { SetStartPlayer(player4, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode, inputManager.p4_Left, inputManager.p4_Right); }

            // Start the coroutine
            StartCoroutine(MoveCameraCoroutine());
        }

        private void Update() { SetUpdate(setActiveUpdate); }

        void SetUpdate(bool active)
        {
            if (active)
            {
                if (player1 != null && p1Active) player1.SetUpdate();
                if (player2 != null && p2Active) player2.SetUpdate();
                if (player3 != null && p3Active) player3.SetUpdate();
                if (player4 != null && p4Active) player4.SetUpdate();
            }
        }

        IEnumerator MoveCameraCoroutine()
        {
            float elapsedTime = 0f;
            Vector3 startingPosition = inGameCamera.transform.position;

            while (elapsedTime < levelDuration)
            {
                elapsedTime += Time.fixedDeltaTime;
                inGameCamera.transform.position = Vector3.Lerp(startingPosition, new Vector3(0, endGoal, startingPosition.z), elapsedTime / levelDuration);
                yield return new WaitForFixedUpdate();
            }

            // Ensure the camera reaches exactly the target position
            inGameCamera.transform.position = new Vector3(0, endGoal, startingPosition.z);
        }

        void SetStartPlayer(PlayerController player, float pMoveSpeed, float pRotateSpeed, float pRotateDelay, bool solo, KeyCode leftControl, KeyCode rightControl)
        {
            player.moveSpeed = pMoveSpeed;
            player.rotateSpeed = pRotateSpeed;
            player.rotateDelay = pRotateDelay;

            player.singleMovement = solo;
            player.SetStart();

            player.InputLeftDownUpdate(leftControl);
            player.InputRightDownUpdate(rightControl);
        }

        IEnumerator CountdownRoutine(int startValue, float delay)
        {
            TextMeshProUGUI countdownText = delayText.GetComponent<TextMeshProUGUI>();

            for (int i = startValue; i > 0; i--)
            {
                // Set text to current countdown value
                countdownText.text = i.ToString();

                // Resize using DoTween
                countdownText.rectTransform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    // Restore original size
                    countdownText.rectTransform.DOScale(1f, 0.2f);
                });

                // Delay between countdown numbers
                yield return new WaitForSeconds(delay);
            }

            // Display "Go!" or any other message
            countdownText.text = "Go!";
            countdownText.rectTransform.DOScale(1.5f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                // Restore original size
                countdownText.rectTransform.DOScale(1f, 0.2f);
                countdownText.gameObject.SetActive(false);
                SetStart();
                setActiveUpdate = true;
            });
        }

        public void SetDestroy()
        {
            Destroy(this.gameObject);
        }
    }
}
