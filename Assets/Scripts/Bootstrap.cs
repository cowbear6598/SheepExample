using SoapUtils.SceneSystem;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [Inject] private readonly ISceneService sceneService;
    
    private void Start()
    {
        sceneService.DoLoadScene(0, false);

        Destroy(gameObject);
    }
}