using ColossalFramework;
using ColossalFramework.UI;
using ICities;
using System;
using System.Reflection;
using UnityEngine;
using static ItemClass;

namespace SimpleFPSClock
{
    public class SimpleFPSClockMenu : IUserMod
    {
        AssemblyName name = Assembly.GetExecutingAssembly().GetName();
        private static readonly string[] ClockPanelformatLabel = new string[5] { "24h", "24h with second", "12h", "12h with second", "German special" };
        private static readonly float[] ClockPanelformatValues = new float[5] { 1f, 2f, 3f, 4f, 5f };

        private string Version => (name.Version.Major + "." + name.Version.Minor + "." + name.Version.Build);
        public string Name => ("Simple FPSClock " + Version);
        public string Description => "Display clock and frames per second (FPS)";

        // the panel to display things, gets created in SimpleFPSClockLoading
        public static SimpleFPSPanel PanelFPS;
        public static SimpleClockPanel PanelClock;
        public UIPanel? m_resetpanel = null;
        public UILabel? m_resetspace = null;
        public UICheckBox m_resetbutton = null;
        public void OnSettingsUI(UIHelperBase helper)
        {            
            // -----------------------
            // General settings
            // ----------------------- 
            UIHelper generalSettings = (UIHelper)helper.AddGroup("SIMPLE FPSCLOCK GENERAL SETTINGS" + "\n" + "(v. " + Version + "." + name.Version.Revision + ")");
            generalSettings.AddSpace(5);            
            bool btnShowClockPanel = SimpleFPSClockConfiguration.Instance.ShowClockPanel;
            UICheckBox showClockCheckbox = generalSettings.AddCheckbox("Show Clock Panel", btnShowClockPanel, delegate (bool selShowClockPanel)
            {
                SimpleFPSClockConfiguration.Instance.ShowClockPanel = selShowClockPanel;
                SimpleFPSClockConfiguration.Instance.Save();
            }) 
                as UICheckBox;
                showClockCheckbox.autoSize = false;
                showClockCheckbox.width = 300f;
            // Container + Panel
            UIComponent showClockContainer = showClockCheckbox.parent;
            UIPanel showClockPanel = showClockContainer.AddUIComponent<UIPanel>();
            showClockPanel.width = showClockContainer.width;
            showClockPanel.height = showClockCheckbox.height;
            UIButton resetClockBtn = generalSettings.AddButton("Reset Clock Position", () =>
            {
                // save the default position in the config file
                SimpleFPSClockConfiguration.SaveClockPanelPosition(SimpleClockPanel.DefaultClockPanelPositionX, SimpleClockPanel.DefaultClockPanelPositionY);
                SimpleFPSClockConfiguration.Instance.Save();
                // if there is a panel, move it to the default position
                if (PanelClock != null)
                    PanelClock.MoveClockPanelToPosition(SimpleClockPanel.DefaultClockPanelPositionX, SimpleClockPanel.DefaultClockPanelPositionY);
            }) as UIButton;
            resetClockBtn.AlignTo(showClockPanel, UIAlignAnchor.TopLeft);
            resetClockBtn.autoSize = false;
            resetClockBtn.width = 250f;
            resetClockBtn.textHorizontalAlignment = UIHorizontalAlignment.Center;
            resetClockBtn.relativePosition += new UnityEngine.Vector3(showClockCheckbox.width + 125, -showClockCheckbox.height * 1.75f);

            //generalSettings.AddSpace(2);
            bool btnShowFPSPanel = SimpleFPSClockConfiguration.Instance.ShowFPSPanel;
            UICheckBox showFPSCheckbox = generalSettings.AddCheckbox("Show FPS Panel", btnShowFPSPanel, delegate (bool selShowFPSPanel)
            {
                SimpleFPSClockConfiguration.Instance.ShowFPSPanel = selShowFPSPanel;
                SimpleFPSClockConfiguration.Instance.Save();
            })
                as UICheckBox;
            showFPSCheckbox.autoSize = false;
            showFPSCheckbox.width = 300f;
            // Container + Panel
            UIComponent showFPSContainer = showFPSCheckbox.parent;
            UIPanel showFPSPanel = showFPSContainer.AddUIComponent<UIPanel>();
            showFPSPanel.width = showFPSContainer.width;
            showFPSPanel.height = showFPSCheckbox.height;
            UIButton resetFPSBtn = generalSettings.AddButton("Reset FPS Position", () =>
            {
                // save the default position in the config file
                SimpleFPSClockConfiguration.SaveFPSPanelPosition(SimpleFPSPanel.DefaultFPSPanelPositionX, SimpleFPSPanel.DefaultFPSPanelPositionY);
                SimpleFPSClockConfiguration.Instance.Save();
                // if there is a panel, move it to the default position
                if (PanelFPS != null)
                    PanelFPS.MoveFPSPanelToPosition(SimpleFPSPanel.DefaultFPSPanelPositionX, SimpleFPSPanel.DefaultFPSPanelPositionY);
            }) as UIButton;
            resetFPSBtn.AlignTo(showFPSPanel, UIAlignAnchor.TopLeft);
            resetFPSBtn.autoSize = false;
            resetFPSBtn.width = 250f;
            resetFPSBtn.textHorizontalAlignment = UIHorizontalAlignment.Center;
            resetFPSBtn.relativePosition += new UnityEngine.Vector3(showFPSCheckbox.width + 125, -showFPSCheckbox.height * 1.75f);

            // -----------------------
            // Clock specific settings
            // -----------------------
            UIHelper clockSettings = (UIHelper)helper.AddGroup("CLOCK SPECIFIC SETTINGS");
            UIPanel panelconfig2 = (UIPanel)clockSettings.self;
            SettingsSlider.Create(clockSettings, LayoutDirection.Horizontal, "Scale Text", 1.1f, 225, 200, 80f, 200f, 10f, SimpleFPSClockConfiguration.Instance.ClockPanelTextScale, delegate (float selClockPanelTextScale)
            {
                //selectedTextScaleValue = sel.ToString();
                SimpleFPSClockConfiguration.Instance.ClockPanelTextScale = selClockPanelTextScale;
                SimpleFPSClockConfiguration.Instance.Save();
            });
            SettingsSlider.Create(clockSettings, LayoutDirection.Horizontal, "Opacity Text", 1.1f, 225, 200, 10f, 100f, 5f, SimpleFPSClockConfiguration.Instance.ClockPanelTextOpacity, delegate (float selClockPanelTextOpacity)
            {
                SimpleFPSClockConfiguration.Instance.ClockPanelTextOpacity = selClockPanelTextOpacity;
                SimpleFPSClockConfiguration.Instance.Save();
            });
            SettingsSlider.Create(clockSettings, LayoutDirection.Horizontal, "Opacity Background", 1.1f, 225, 200, 0f, 100f, 5f, SimpleFPSClockConfiguration.Instance.ClockPanelOpacity, delegate (float selClockPanelOpacity)
            {                
                SimpleFPSClockConfiguration.Instance.ClockPanelOpacity = selClockPanelOpacity;
                SimpleFPSClockConfiguration.Instance.Save();
            });
            // clockSettings.AddSpace(5);
            // Dropdownselection timeformat + parent for outline value
            int selectedOptionIndex = GetSelectedOptionIndex(ClockPanelformatValues, SimpleFPSClockConfiguration.Instance.ClockPanelTimeFormat);
            UIDropDown clockTimeformatDropdown = clockSettings.AddDropdown("Time format", ClockPanelformatLabel, selectedOptionIndex, delegate (int selClockPanelTimeFormat)
            {
                SimpleFPSClockConfiguration.Instance.ClockPanelTimeFormat = ClockPanelformatValues[selClockPanelTimeFormat];
                SimpleFPSClockConfiguration.Instance.Save();
            }) as UIDropDown;
            clockTimeformatDropdown.width = 225f;
            // Container + Panel
            UIComponent clockTimeformatContainer = clockTimeformatDropdown.parent;
            UIPanel clockTimeformatPanel = clockTimeformatContainer.AddUIComponent<UIPanel>();
            clockTimeformatPanel.width = clockTimeformatContainer.width;
            clockTimeformatPanel.height = clockTimeformatDropdown.height;
            // outline value as a child of dropdownselection
            bool btnClockPanelUseOutlineColor = SimpleFPSClockConfiguration.Instance.ClockPanelUseOutlineColor;
            UICheckBox outlinesClockCheckbox = clockSettings.AddCheckbox("Use Outline ON/OFF", btnClockPanelUseOutlineColor, delegate (bool selClockPanelUseOutlineColor)
            {
                SimpleFPSClockConfiguration.Instance.ClockPanelUseOutlineColor = selClockPanelUseOutlineColor;
                SimpleFPSClockConfiguration.Instance.Save();
            })
                as UICheckBox;
            outlinesClockCheckbox.tooltip = "Set if text for clock panel should use outline";
            outlinesClockCheckbox.AlignTo(clockTimeformatPanel, UIAlignAnchor.TopLeft);
            outlinesClockCheckbox.relativePosition += new UnityEngine.Vector3(clockTimeformatDropdown.width + 50, -clockTimeformatDropdown.height);
            outlinesClockCheckbox.width = 250f;
            outlinesClockCheckbox.autoSize = false;

            // FPS specific settings
            UIHelper fpsSettings = (UIHelper)helper.AddGroup("FPS SPECIFIC SETTINGS");
            UIPanel panelconfig3 = (UIPanel)fpsSettings.self;
            
            fpsSettings.AddSpace(5);
            SettingsSlider.Create(fpsSettings, LayoutDirection.Horizontal, "Scale Text", 1.1f, 225, 200, 80f, 200f, 10f, SimpleFPSClockConfiguration.Instance.FPSPanelTextScale, delegate (float selFPSPanelTextScale)
            {
                //selectedTextScaleValue = sel.ToString();
                SimpleFPSClockConfiguration.Instance.FPSPanelTextScale = selFPSPanelTextScale;
                SimpleFPSClockConfiguration.Instance.Save();
            });
            SettingsSlider.Create(fpsSettings, LayoutDirection.Horizontal, "Opacity Text", 1.1f, 225, 200, 10f, 100f, 5f, SimpleFPSClockConfiguration.Instance.FPSPanelTextOpacity, delegate (float selFPSPanelTextOpacity)
            {
                SimpleFPSClockConfiguration.Instance.FPSPanelTextOpacity = selFPSPanelTextOpacity;
                SimpleFPSClockConfiguration.Instance.Save();
            });
            SettingsSlider.Create(fpsSettings, LayoutDirection.Horizontal, "Opacity Background", 1.1f, 225, 200, 0f, 100f, 5f, SimpleFPSClockConfiguration.Instance.FPSPanelOpacity, delegate (float selFPSPanelOpacity)
            {
                SimpleFPSClockConfiguration.Instance.FPSPanelOpacity = selFPSPanelOpacity;
                SimpleFPSClockConfiguration.Instance.Save();
            });
            fpsSettings.AddSpace(5);
            // Checkbox group FPS
            bool btnFPSColoredValue = SimpleFPSClockConfiguration.Instance.FPSColoredValue;
            UICheckBox coloredFPSCheckbox = fpsSettings.AddCheckbox("Show value in color", btnFPSColoredValue, delegate (bool selFPSColoredValue)
            {
                SimpleFPSClockConfiguration.Instance.FPSColoredValue = selFPSColoredValue;
                SimpleFPSClockConfiguration.Instance.Save();
            }) as UICheckBox;
            coloredFPSCheckbox.tooltip = "Set to show value in red, yellow, green";
            fpsSettings.AddSpace(5);
            bool btnFPSTextSuffix = SimpleFPSClockConfiguration.Instance.FPSTextSuffix;
            UICheckBox suffixFPSCheckbox = fpsSettings.AddCheckbox("Show value suffix \"FPS\"", btnFPSTextSuffix, delegate (bool selFPSTextSuffix)
            {
                SimpleFPSClockConfiguration.Instance.FPSTextSuffix = selFPSTextSuffix;
                SimpleFPSClockConfiguration.Instance.Save();
            }) as UICheckBox;
            suffixFPSCheckbox.tooltip = "Set to show value plus \"FPS\" extension";
            fpsSettings.AddSpace(5);
            bool btnFPSPanelUseOutlineColor = SimpleFPSClockConfiguration.Instance.FPSPanelUseOutlineColor;
            UICheckBox outlinesFPSCheckbox = fpsSettings.AddCheckbox("Use Outline ON/OFF", btnFPSPanelUseOutlineColor, delegate (bool selFPSPanelUseOutlineColor)
            {
                SimpleFPSClockConfiguration.Instance.FPSPanelUseOutlineColor = selFPSPanelUseOutlineColor;
                SimpleFPSClockConfiguration.Instance.Save();
            }) as UICheckBox;
            outlinesFPSCheckbox.tooltip = "Set if text for FPS panel should use outline";
            //fpsSettings.AddSpace(5);


        }
        
        private int GetSelectedOptionIndex(float[] option, float value)
        {
            int num = Array.IndexOf(option, value);
            if (num < 0)
            {
                num = 0;
            }
            return num;
        }
        //public static void ShowMessage(string message)
        //{
        //    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Message, message);
        //    DebugOutputPanel.Show();
        //}
    }
}