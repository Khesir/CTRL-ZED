using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameInitiator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private EventSystem _eventSystem;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Env _environment;
    [SerializeField] private CharacterManager _characterManager;
    enum Env
    {
        Dev,
        Prod,
    }
    async void Start()
    {
        BindObjects();
        // Loading Screen
        await IntializeObjects();
        await CreateObjects();
        Preparegame();
        BeginGame();
    }

    private void BindObjects()
    {
        if (_environment != Env.Dev)
        {

            _mainCamera = Instantiate(_mainCamera);
            _eventSystem = Instantiate(_eventSystem);
            _canvas = Instantiate(_canvas);
        }
    }
    private async UniTask IntializeObjects()
    {
        // Handles Services that uses in the game
    }
    private async UniTask CreateObjects()
    {
        // Creation of game objects e.g. UI, player background, spawinning of enemies
        await UniTask.CompletedTask;

    }
    private void Preparegame()
    {
        // Preparation -- Setting up, states of characters and more
    }
    private void BeginGame()
    {
        // Game logic -- actual trigger of the game or so
    }
}
