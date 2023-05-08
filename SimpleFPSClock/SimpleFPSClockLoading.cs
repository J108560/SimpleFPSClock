using ColossalFramework.UI;
using ICities;

namespace SimpleFPSClock;

public class SimpleFPSClockLoading : LoadingExtensionBase
{
	public override void OnLevelLoaded(LoadMode mode)
	{
		base.OnLevelLoaded(mode);
		UIView aView = UIView.GetAView();
		SimpleFPSClockMenu.PanelFPS = (SimpleFPSPanel)aView.AddUIComponent(typeof(SimpleFPSPanel));
        

        SimpleFPSClockMenu.PanelClock = (SimpleClockPanel)aView.AddUIComponent(typeof(SimpleClockPanel));
	}
}
