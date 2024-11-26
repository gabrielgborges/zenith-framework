using UnityEngine;

public class ScreenViewBase : MonoBehaviour
{
    protected IScreenController _controller;
    
    private void Awake()
    {
       _controller = GetComponent<IScreenController>();
       Initialize();
       InitializeController();
    }

    protected virtual void Initialize()
    {
        
    }
    
    protected virtual void InitializeController()
    {
        
    }
}
