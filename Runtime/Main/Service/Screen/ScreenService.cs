using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class ScreenService : ServiceBase, IScreenService
{
    public override void Setup()
    {
        ServiceLocator.AddService<IScreenService>(this);
    }

    public override void Dispose()
    {
        ServiceLocator.RemoveService<IScreenService>(this);
    }
 
    public void LoadScreen<T>(GameScreen gameScreen, Action<T> openedCallback) where T : ScreenControllerBase
    {
        T screenController = (T)LoadScreen(gameScreen);
        OpenScreen(screenController, openedCallback);
    }

    private ScreenControllerBase LoadScreen(GameScreen gameScreen) 
    {
        Object screenObject = Resources.Load(gameScreen + "Screen");
        GameObject loadedScreenPrefab = (GameObject)Instantiate(screenObject);
        ScreenControllerBase screenController = loadedScreenPrefab.GetComponent<ScreenControllerBase>();

        return screenController;
    }

    private void OpenScreen<T>(T screenController, Action<T> callback) where T : ScreenControllerBase
    {
        screenController.OnOpen = () => callback.Invoke(screenController);
        screenController.Open();
    }
}
