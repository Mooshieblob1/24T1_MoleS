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
            if (Instance != null && Instance != this) { Destroy(this.gameObject); } else { Instance = this; }
        }

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

        [TitleGroup("BOX PLAYER/PLAYER START POSITION", "Set all the players start position depending on how many players")]
        [FoldoutGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder")]
        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 1 Player")] 
        [HideLabel]
        public int 
        Only1Player;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 2 Player")]
        [HideLabel]
        [SerializeField]
        public int
        Only2Player1,
        Only2Player2;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 3 Player")]
        [HideLabel]
        [SerializeField]
        public int
        Only3Player1,
        Only3Player2,
        Only3Player3;

        [BoxGroup("BOX PLAYER/PLAYER START POSITION/Collapse Folder/Only have 4 Player")]
        [HideLabel]
        [SerializeField]
        public int
        Only4Player1,
        Only4Player2,
        Only4Player3,
        Only4Player4;


        [TitleGroup("BOX PLAYER/START SPEED & ROTATION", "Set all the players the Speed and Rotation on the start")]
        [BoxGroup("BOX PLAYER")]
        public float 
        playerMoveSpeed,
        playerRotateSpeed,
        playerRotateDelay;

        //[TitleGroup("BOX PLAYER/CHECK PLAYER ACTIVE", "Set how many players in the game")]
        //[BoxGroup("BOX PLAYER")]
        [HideInInspector]
        public bool 
        p1Active,
        p2Active,
        p3Active,
        p4Active;

        [TitleGroup("BOX PLAYER/CHECK PLAYER ACTIVE", "Set how many players in the game")]
        [BoxGroup("BOX PLAYER")]
        public int playersActive;

        int P1Pos, P2Pos, P3Pos, P4Pos;

        [TitleGroup("BOX PLAYER/SET MODE", "Singleplayer / Multiplayer")]
        [BoxGroup("BOX PLAYER")]
        public bool soloMode;

        private void Start() 
        {
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
            SetPlayerPosition(playersActive);
            SetPlayerActivation(playersActive);

            if (player1 != null && p1Active) { SetStartPlayer(player1, P1Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode); }
            if (player2 != null && p2Active) { SetStartPlayer(player2, P2Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode); }
            if (player3 != null && p3Active) { SetStartPlayer(player3, P3Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode); }
            if (player4 != null && p4Active) { SetStartPlayer(player4, P4Pos, playerMoveSpeed, playerRotateSpeed, playerRotateDelay, soloMode); }

            // Start the coroutine
            //StartCoroutine(MoveCameraCoroutine());
        }

        public void SetPlayerActivation(int numberOfPlayers)
        {
            // First, deactivate all players
            p1Active = p2Active = p3Active = p4Active = false;

            // Then, activate players based on the number of players
            switch (numberOfPlayers)
            {
                case 1:
                    p1Active = true;
                    player1.AssignGamepad(0);
                    break;
                case 2:
                    p1Active = p2Active = true;
                    player1.AssignGamepad(0);
                    player2.AssignGamepad(1);
                    break;
                case 3:
                    p1Active = p2Active = p3Active = true;
                    player1.AssignGamepad(0);
                    player2.AssignGamepad(1);
                    player3.AssignGamepad(2);
                    break;
                case 4:
                    p1Active = p2Active = p3Active = p4Active = true;
                    player1.AssignGamepad(0);
                    player2.AssignGamepad(1);
                    player3.AssignGamepad(2);
                    player4.AssignGamepad(3);
                    break;
                default:
                    Debug.LogError("Unsupported number of players: " + numberOfPlayers);
                    break;
            }

            // Update PlayerControllers' active status based on the active flags
            player1.gameObject.SetActive(p1Active);
            player2.gameObject.SetActive(p2Active);
            player3.gameObject.SetActive(p3Active);
            player4.gameObject.SetActive(p4Active);
        }

        public void SetPlayerPosition(int numberOfPlayers)
        {
            switch (numberOfPlayers)
            {
                case 1:
                    P1Pos = Only1Player;
                    // Optionally reset other player values
                    P2Pos = P3Pos = P4Pos = 0; // Or any default value
                    break;
                case 2:
                    P1Pos = Only2Player1;
                    P2Pos = Only2Player2;
                    // Optionally reset other player values
                    P3Pos = P4Pos = 0; // Or any default value
                    break;
                case 3:
                    P1Pos = Only3Player1;
                    P2Pos = Only3Player2;
                    P3Pos = Only3Player3;
                    // Optionally reset other player value
                    P4Pos = 0; // Or any default value
                    break;
                case 4:
                    P1Pos = Only4Player1;
                    P2Pos = Only4Player2;
                    P3Pos = Only4Player3;
                    P4Pos = Only4Player4;
                    break;
                default:
                    Debug.LogError("Unsupported number of players: " + numberOfPlayers);
                    // Optionally reset all player values
                    P1Pos = P2Pos = P3Pos = P4Pos = 0; // Or any default value
                    break;
            }
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

        void SetStartPlayer(PlayerController player, int pPosition, float pMoveSpeed, float pRotateSpeed, float pRotateDelay, bool solo)
        {
            player.transform.position = new Vector3(pPosition, player.transform.position.y, player.transform.position.z);

            player.moveSpeed = pMoveSpeed;
            player.rotateDuration = pRotateSpeed;
            player.rotateDelay = pRotateDelay;

            player.singleMovement = solo;
            player.SetStart();
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
