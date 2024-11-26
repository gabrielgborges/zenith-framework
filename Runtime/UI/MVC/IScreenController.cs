
public interface IScreenController  
{
   public void Initialize<T>(T screenView) where T : ScreenViewBase;
}
