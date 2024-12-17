using System;

/// <summary>
/// Interface for a Screen Service that provides methods to manage and load all screens in the project.
/// </summary>
public interface IScreenService : IService
{
    
    /// <summary>
    /// Loads a prefab from the Resources folder that matches the name of the specified <see cref="GameScreen"/> enum value 
    /// with the suffix "Screen" appended to it. <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the screen controller that will be attached to the loaded prefab.</typeparam>
    /// <param name="gameScreen">The <see cref="GameScreen"/> enum value corresponding to the screen to be loaded.</param>
    /// <param name="openedCallback">
    /// A callback invoked after the prefab is successfully opened, passing the screen controller of type <typeparamref name="T"/>.
    /// </param>
    public void LoadScreen<T>(GameScreen gameScreen, Action<T> openedCallback) where T : ScreenControllerBase;
}
