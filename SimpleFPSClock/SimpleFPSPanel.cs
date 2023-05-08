using ColossalFramework.UI;
using UnityEngine;
using System;
using System.Collections;

namespace SimpleFPSClock
{
    /// <summary>
    /// panel to display fps
    /// </summary>
    public class SimpleFPSPanel : UIPanel
    {
        // default panel position
        public static float DefaultFPSPanelPositionX = UIView.GetAView().GetScreenResolution().x - 510f;
        public static float DefaultFPSPanelPositionY = UIView.GetAView().GetScreenResolution().y - 93.5f;

        // UI components on the panel        
        private UILabel m_fpsLabel;
        private UILabel m_fpsBackgroundLabel;

        // flags for dragging the panel
        private UIDragHandle m_fpsdragHandle;

        // variables for doing something
        private float FPSfrequency = 0.5f;
        private int nbDecimal = 1;
        private float accum = 0f;
        private int frames = 0;
        private Color fpscolor = Color.white;
        private string sfFPS = "";
        private bool isFPSColor;
        private bool isFPSSuffix;
        /// Start is called after the panel is created in Loading
        /// set up and populate the panel
        public override void Start()
        {
            // do base processing
            base.Start();
            StartCoroutine((IEnumerator)FPS());
            try
            {
                // set panel properties
                name = "SimpleFPSPanel";
                width = 160;
                height = 40;

                // move panel to initial position according to the config
                SimpleFPSClockConfiguration config = Configuration<SimpleFPSClockConfiguration>.Load();
                MoveFPSPanelToPosition(config.FPSPanelPositionX, config.FPSPanelPositionY);

                // Add Labels
                // Background first, because opacity under text
                m_fpsBackgroundLabel = AddUIComponent<UILabel>();
                m_fpsBackgroundLabel.autoSize = false;
                m_fpsBackgroundLabel.useOutline = true;
                m_fpsBackgroundLabel.outlineColor = fpscolor;
                m_fpsBackgroundLabel.width = 160;
                m_fpsBackgroundLabel.height = 50;
                m_fpsBackgroundLabel.backgroundSprite = "MenuPanel";
                m_fpsBackgroundLabel.color = new Color32(16, 16, 16, 128);
                m_fpsBackgroundLabel.opacity = 0.25f;
                m_fpsBackgroundLabel.relativePosition = new Vector2(0, 0);
                // Text secound, opacity textpanel
                m_fpsLabel = AddUIComponent<UILabel>();
                m_fpsLabel.useOutline = false;
                m_fpsLabel.outlineColor = Color.grey;
                m_fpsLabel.text = sfFPS;
                m_fpsLabel.textScale = 1.5f;                
                m_fpsLabel.textAlignment = UIHorizontalAlignment.Center;
                m_fpsLabel.verticalAlignment = UIVerticalAlignment.Middle;
                m_fpsLabel.autoSize = false;
                m_fpsLabel.width = 160;
                m_fpsLabel.height = 50;
                m_fpsLabel.relativePosition = new Vector2(0f, 1f);
                // Add a UIDragHandle to make the panel moveable InputKey keysFPSHide
                m_fpsdragHandle = AddUIComponent<UIDragHandle>();
                m_fpsdragHandle.width = 160;
                m_fpsdragHandle.height = 50;
                m_fpsdragHandle.tooltip = "Drag to move panel";
                m_fpsdragHandle.relativePosition = new Vector2(0, 0);
                m_fpsdragHandle.eventMouseUp += delegate
                {
                    config.FPSPanelPositionX = absolutePosition.x;
                    config.FPSPanelPositionY = absolutePosition.y;
                    Configuration<SimpleFPSClockConfiguration>.Save();
                };
                // show or hide the panel according to the config
                isVisible = SimpleFPSClockConfiguration.Instance.ShowFPSPanel;
                // show or hide the panel outlineColor according to the config
                m_fpsLabel.useOutline = SimpleFPSClockConfiguration.Instance.FPSPanelUseOutlineColor;
                isFPSColor = SimpleFPSClockConfiguration.Instance.FPSColoredValue;
                if (isFPSColor)
                    m_fpsLabel.textColor = fpscolor;
                else
                    m_fpsLabel.textColor = Color.white;
                isFPSSuffix = SimpleFPSClockConfiguration.Instance.FPSTextSuffix;
                if (isFPSSuffix)
                    m_fpsLabel.suffix = " FPS";
                else
                    m_fpsLabel.suffix = "";
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
        private IEnumerator FPS()
        {
            while (true)
            {
                float fps = accum / (float)frames;
                sfFPS = fps.ToString("f" + Mathf.Clamp(nbDecimal, 0, 10));
                fpscolor = ((fps >= 30f) ? Color.green : ((fps > 10f) ? Color.yellow : Color.red));
                accum = 0f;
                frames = 0;
                yield return new WaitForSeconds(FPSfrequency);
            }
        }
        public override void Update()
        {
            accum += Time.timeScale / Time.deltaTime;
            frames++;
            // do base processing
            base.Update();
            isVisible = SimpleFPSClockConfiguration.Instance.ShowFPSPanel;
            m_fpsBackgroundLabel.opacity = SimpleFPSClockConfiguration.Instance.FPSPanelOpacity / 100;
            isFPSColor = SimpleFPSClockConfiguration.Instance.FPSColoredValue;
                {
                    if (isFPSColor)
                        m_fpsLabel.textColor = fpscolor;
                    else
                        m_fpsLabel.textColor = Color.white;
                }
            m_fpsLabel.text = sfFPS;
            isFPSSuffix = SimpleFPSClockConfiguration.Instance.FPSTextSuffix;
            if (isFPSSuffix)
                m_fpsLabel.suffix = " FPS";
            else
                m_fpsLabel.suffix = "";
            m_fpsLabel.textScale = SimpleFPSClockConfiguration.Instance.FPSPanelTextScale / 100;
            m_fpsLabel.opacity = SimpleFPSClockConfiguration.Instance.FPSPanelTextOpacity / 100;
            m_fpsLabel.useOutline = SimpleFPSClockConfiguration.Instance.FPSPanelUseOutlineColor;
        }
        public void MoveFPSPanelToPosition(float x, float y)
        {
            relativePosition = new Vector3(x,y);
        }
    }
}
