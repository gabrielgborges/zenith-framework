using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServicesInitializer : MonoBehaviour
{
    [SerializeReference] private BaseService[] _services;

    private void Awake()
    {
        SetupServices();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnDestroy()
    {
        DisposeServices();
    }

    private void SetupServices()
    {
        foreach (BaseService service in _services)
        {
            service.Setup();
        }
    }

    private void DisposeServices()
    {
        foreach (BaseService service in _services)
        {
            service.Dispose();
        }
    }
}
