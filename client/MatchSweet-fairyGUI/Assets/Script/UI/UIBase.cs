using FairyGUI;

namespace Game.UI
{
    public enum UI_Layer
    {
        Max = int.MaxValue,
        WINDOW = 100,
        BILLBOARD = 1,
    }
    public class UIBase : Window
    {

        protected override void OnShown()
        {
            base.OnShown();
            sortingOrder = (int)UI_Layer.Max;
        }
    }
}

