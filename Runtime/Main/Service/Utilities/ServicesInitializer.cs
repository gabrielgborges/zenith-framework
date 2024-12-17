using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServicesInitializer : MonoBehaviour
{
    [SerializeReference] private ServiceBase[] _services;

    private void Awake()
    {
        SetupServices();
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        DisposeServices();
    }

    private void SetupServices()
    {
        foreach (ServiceBase service in _services)
        {
            service.Setup();
        }
    }

    private void DisposeServices()
    {
        foreach (ServiceBase service in _services)
        {
            service.Dispose();
        }
    }
}
