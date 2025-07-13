using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInitiator : MonoBehaviour
{
    [Header("Environment Setup")]
    [SerializeField] private bool isDevelopmnet = true;
    // [SerializeField] private GameState initialState = GameState.InitialMenu;

    [Header("Prefabs for Runtime Instantiation")]
    // [SerializeField] private 
    // [SerializeField] private Env _environment;
    // [SerializeField] private GameManager _gameManager;
    // [SerializeField] private GameplayManager _gameplayManager;
    // [SerializeField] private MenuManager _menuManager;
    // [SerializeField] public bool inGameplay = false;
    // [SerializeField] private bool isGenerated;
    // enum Env
    // {
    //     Dev,
    //     Prod,
    // }
    // async void Start()
    // {
    //     Debug.Log("Binding Objects");
    //     BindObjects();
    //     // Loading Screen

    //     Debug.Log("Initializing and Creating Game Objects");
    //     await IntializeObjects();
    //     await CreateObjects();

    //     Debug.Log("Preparing Game");
    //     await Preparegame();
    //     BeginGame();
    // }

    // private void BindObjects()
    // {
    //     // Need a reference, requires to add manually
    //     if (_environment != Env.Dev)
    //     {
    //         _gameManager = Instantiate(_gameManager);
    //         _gameplayManager = Instantiate(_gameplayManager);
    //     }
    //     else
    //     {
    //         // For Dev: Try to auto-locate scene objects
    //         if (_gameManager == null) _gameManager = FindObjectOfType<GameManager>();
    //         if (_gameplayManager == null) _gameplayManager = FindObjectOfType<GameplayManager>();
    //     }
    // }
    // private async UniTask IntializeObjects()
    // {
    //     // Intiaalizing Game Data and other services 
    //     if (_gameManager != null) await GameManager.Instance.Initialize();
    //     if (_gameplayManager != null) await GameplayManager.Instance.Initialize();
    // }
    // private async UniTask CreateObjects()
    // {
    //     // Creation of game objects e.g. UI, player background, spawinning of enemies4
    //     // if (!isGenerated) GenerateTestData();
    //     await UniTask.CompletedTask;

    // }
    // private async UniTask Preparegame()
    // {

    //     // Preparation -- Setting up, states of characters and more
    //     if (_gameplayManager != null)
    //     {
    //         await GameplayManager.Instance.Setup();
    //         await GameplayManager.Instance.gameplayUI.Initialize();
    //     }
    //     if (_menuManager != null) await MenuManager.Instance.menuUIController.Initialize();
    //     await UniTask.CompletedTask;
    // }
    // private void BeginGame()
    // {
    //     Debug.Log("Game Started");
    //     // Game logic -- actual trigger of the game or so
    // }

    // No team ID provided --- disabled for now
    // private void GenerateTestData()
    // {
    //     // Create a new team
    //     GameManager.Instance.TeamManager.CreateTeam();

    //     // Create characters based on character templates
    //     foreach (var characterService in GameManager.Instance.characterTemplates)
    //     {
    //         GameManager.Instance.CharacterManager.CreateCharacter(characterService);
    //     }

    //     // Get the list of created characters
    //     var characters = GameManager.Instance.CharacterManager.GetCharacters();

    //     // Ensure there are characters to assign to slots
    //     for (int i = 0; i < characters.Count; i++)  // Fix: use '<' to avoid out of range
    //     {
    //         GameManager.Instance.TeamManager.AssignedCharacterToSlot(0, i, characters[i]);
    //     }

    //     // Set the active team (0 here represents the first team)
    //     GameManager.Instance.TeamManager.SetActiveTeam(0);
    //     isGenerated = true;
    // }
}
