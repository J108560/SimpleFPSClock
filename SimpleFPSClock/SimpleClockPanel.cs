using System;
using ColossalFramework.UI;
using UnityEngine;

namespace SimpleFPSClock
{
    public class SimpleClockPanel : UIPanel
    {
        // UI components on the panel
        private UILabel m_clockLabel;
        private UILabel m_clockBackgroundLabel;
        // flags for dragging the panel
        private UIDragHandle m_clockdragHandle;
        // variables for doing something
        private float clocktimeformat;
        // default panel position
        public static float DefaultClockPanelPositionX = UIView.GetAView().GetScreenResolution().x - 350f;
        public static float DefaultClockPanelPositionY = UIView.GetAView().GetScreenResolution().y - 93.5f;
        public override void Start()
        {
            // do base processing
            base.Start();
            try
            {
                // set panel properties
                name = "SimpleClockPanel";
                width = 160;
                height = 40;
                // move panel to initial position according to the config
                SimpleFPSClockConfiguration config = Configuration<SimpleFPSClockConfiguration>.Load();
                MoveClockPanelToPosition(config.ClockPanelPositionX, config.ClockPanelPositionY);

                // Add Labels
                // Background first, because opacity under text
                m_clockBackgroundLabel = AddUIComponent<UILabel>();
                m_clockBackgroundLabel.autoSize = false;
                m_clockBackgroundLabel.width = 160;
                m_clockBackgroundLabel.height = 50;
                m_clockBackgroundLabel.backgroundSprite = "MenuPanel2"; //GenericPanel, MenuPanel, MenuPanel2 
                m_clockBackgroundLabel.color = new Color32(16, 16, 16, 128);
                m_clockBackgroundLabel.opacity = 0.25f;
                m_clockBackgroundLabel.relativePosition = new Vector2(0, 0);
                // Text secound, opacity textpanel
                m_clockLabel = AddUIComponent<UILabel>();
                m_clockLabel.useOutline = false;
                m_clockLabel.outlineColor = Color.grey;
                m_clockLabel.text = "";
                m_clockLabel.textScale = 1f;
                m_clockLabel.textAlignment = UIHorizontalAlignment.Center;
                m_clockLabel.verticalAlignment = UIVerticalAlignment.Middle;
                m_clockLabel.autoSize = false;
                m_clockLabel.width = 160;
                m_clockLabel.height = 50;
                m_clockLabel.relativePosition = new Vector2(0f, 1f);                
                // Add a UIDragHandle to make the panel moveable InputKey keysFPSHide
                m_clockdragHandle = AddUIComponent<UIDragHandle>();
                m_clockdragHandle.width = 160;
                m_clockdragHandle.height = 50;
                m_clockdragHandle.tooltip = "Drag to move panel";
                m_clockdragHandle.relativePosition = new Vector2(0, 0);
                m_clockdragHandle.eventMouseUp += delegate
                {
                    SimpleFPSClockConfiguration.Instance.ClockPanelPositionX = absolutePosition.x;
                    SimpleFPSClockConfiguration.Instance.ClockPanelPositionY = absolutePosition.y;
                    SimpleFPSClockConfiguration.Instance.Save();
                };
                // show or hide the panel according to the config
                isVisible = SimpleFPSClockConfiguration.Instance.ShowClockPanel;
                // show or hide the panel outlineColor according to the config
                m_clockLabel.useOutline = SimpleFPSClockConfiguration.Instance.ClockPanelUseOutlineColor;
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        public override void Update()
        {
            // do base processing
            base.Update();
            m_clockBackgroundLabel.opacity = SimpleFPSClockConfiguration.Instance.ClockPanelOpacity / 100f;
            m_clockLabel.textScale = SimpleFPSClockConfiguration.Instance.ClockPanelTextScale / 100f;
            m_clockLabel.opacity = SimpleFPSClockConfiguration.Instance.ClockPanelTextOpacity / 100f;
            m_clockLabel.useOutline = SimpleFPSClockConfiguration.Instance.ClockPanelUseOutlineColor;
            clocktimeformat = SimpleFPSClockConfiguration.Instance.ClockPanelTimeFormat;
            {
                // Change Timeformat

                if (clocktimeformat >= 5f) m_clockLabel.text = DateTime.Now.ToString("HH:mm") + " Uhr";
                else if (clocktimeformat >= 4f) m_clockLabel.text = DateTime.Now.ToString("h:mm:ss tt");
                else if (clocktimeformat >= 3f) m_clockLabel.text = DateTime.Now.ToString("h:mm tt");
                else if (clocktimeformat >= 2f) m_clockLabel.text = DateTime.Now.ToString("HH:mm:ss");
                else m_clockLabel.text = DateTime.Now.ToString("HH:mm");
            }
            isVisible = SimpleFPSClockConfiguration.Instance.ShowClockPanel;
        }
        public void MoveClockPanelToPosition(float x, float y)
        {
            relativePosition = new Vector3(x, y);
        }

    }
}