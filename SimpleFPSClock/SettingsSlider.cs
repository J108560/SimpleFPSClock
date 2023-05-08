using ColossalFramework.UI;
using UnityEngine;

namespace SimpleFPSClock
{
    /* 
    * Code adapted from Transfer Manager under MIT license https://github.com/Sleepy334/TransferManagerCE/blob/main/TransferManagerCE%202.3/LICENSE
    */
    public class SettingsSlider
    {
		const int iSLIDER_PANEL_HEIGHT = 30;
		public UIPanel? m_panel = null;
		public UILabel? m_label = null;
        public UILabel? m_labelMIN = null;
        public UILabel? m_labelMAX = null;
        public UILabel? m_labelCurrent = null;
        public UILabel? m_labelCurrentVal = null;
        public UILabel? m_labelCurrentSuffix = null;
        private UISlider? m_slider = null;
		private string m_sText = "";
		private float m_fValue;
		private ICities.OnValueChanged? m_eventCallback = null;
		private bool m_bPercent = false;

        public bool Percent
		{
			get 
			{ 
				return m_bPercent; 
			}
			set
			{
				m_bPercent = value;
                UpdateLabel();
			}
		}

        public static SettingsSlider Create(UIHelper helper, LayoutDirection direction, string sText, float fTextScale, int iLabelWidth, int iSliderWidth, float fMin, float fMax, float fStep, float fDefault, ICities.OnValueChanged eventCallback)
		{
			SettingsSlider oSlider = new SettingsSlider();
			oSlider.m_fValue = fDefault;
			oSlider.m_eventCallback = eventCallback;
			oSlider.m_sText = sText;

			// Panel
			oSlider.m_panel = ((UIPanel) helper.self).AddUIComponent<UIPanel>();
			oSlider.m_panel.autoLayout = true;
			oSlider.m_panel.autoLayoutDirection = direction;
			oSlider.m_panel.autoSize = false;
			oSlider.m_panel.width = iLabelWidth + iSliderWidth + 300;
			oSlider.m_panel.height = (direction == LayoutDirection.Horizontal) ? iSLIDER_PANEL_HEIGHT : 2 * iSLIDER_PANEL_HEIGHT;
			//oSlider.m_panel.backgroundSprite = "InfoviewPanel";
			//oSlider.m_panel.color = Color.red;

			// Label
			oSlider.m_label = oSlider.m_panel.AddUIComponent<UILabel>();
			if (oSlider.m_label is not null)
			{
				oSlider.m_label.autoSize = false;
				oSlider.m_label.width = (direction == LayoutDirection.Vertical) ? oSlider.m_panel.width : iLabelWidth;
				oSlider.m_label.height = 28;
				oSlider.m_label.text = oSlider.m_sText + ":";
                oSlider.m_label.textColor = Color.white;
                oSlider.m_label.textScale = fTextScale;
                //oSlider.m_label.backgroundSprite = "InfoviewPanel";
                //oSlider.m_label.color = Color.blue;
            }
            
            // Label Minvalue
            oSlider.m_labelMIN = oSlider.m_panel.AddUIComponent<UILabel>();
            if (oSlider.m_labelMIN is not null)
            {
                oSlider.m_labelMIN.autoSize = false;
                oSlider.m_labelMIN.width = (direction == LayoutDirection.Vertical) ? oSlider.m_panel.width : 40;
                oSlider.m_labelMIN.height = 28;
                oSlider.m_labelMIN.text = fMin.ToString() + "%";
                oSlider.m_labelMIN.textAlignment = UIHorizontalAlignment.Center;
                oSlider.m_labelMIN.textColor = Color.white;
				oSlider.m_labelMIN.opacity = 0.5f;
                oSlider.m_labelMIN.textScale = fTextScale;
                //oSlider.m_label.backgroundSprite = "InfoviewPanel";
                //oSlider.m_label.color = Color.blue;
            }
            // Slider bar
            oSlider.m_slider = CreateSlider(oSlider.m_panel, (direction == LayoutDirection.Vertical) ? oSlider.m_panel.width : iSliderWidth, 30, fMin, fMax);
			oSlider.m_slider.value = fDefault;
			oSlider.m_slider.stepSize = fStep;
			oSlider.m_slider.color = Color.gray;			
            //oSlider.m_slider.size = new Vector2(iSliderWidth, 8);
            oSlider.m_slider.eventValueChanged += delegate (UIComponent c, float val)
			{
				oSlider.OnSliderValueChanged(val);
			};
            oSlider.m_labelMAX = oSlider.m_panel.AddUIComponent<UILabel>();
            if (oSlider.m_labelMAX is not null)
            {
                oSlider.m_labelMAX.autoSize = false;
                oSlider.m_labelMAX.width = (direction == LayoutDirection.Vertical) ? oSlider.m_panel.width : 60;
                oSlider.m_labelMAX.height = 28;
                oSlider.m_labelMAX.text = fMax.ToString() + "%";
                oSlider.m_labelMAX.textAlignment = UIHorizontalAlignment.Center;
                oSlider.m_labelMAX.textColor = Color.white;
				oSlider.m_labelMAX.opacity = 0.5f;
                oSlider.m_labelMAX.textScale = fTextScale;
            }
            // Current value string colored
            // Label Currenttext
            oSlider.m_labelCurrent = oSlider.m_panel.AddUIComponent<UILabel>();
            if (oSlider.m_labelCurrent is not null)
            {
                oSlider.m_labelCurrent.autoSize = false;
                oSlider.m_labelCurrent.width = (direction == LayoutDirection.Vertical) ? oSlider.m_panel.width : 100;
                oSlider.m_labelCurrent.height = 28;
                oSlider.m_labelCurrent.text = "(Current:";
                oSlider.m_labelCurrent.textAlignment = UIHorizontalAlignment.Right;
                oSlider.m_labelCurrent.textColor = Color.white;
                oSlider.m_labelCurrent.opacity = 0.5f;
                oSlider.m_labelCurrent.textScale = fTextScale;
            }
            // Label Currentvalue
            oSlider.m_labelCurrentVal = oSlider.m_panel.AddUIComponent<UILabel>();
            if (oSlider.m_labelCurrentVal is not null)
            {
                oSlider.m_labelCurrentVal.autoSize = false;
                oSlider.m_labelCurrentVal.width = (direction == LayoutDirection.Vertical) ? oSlider.m_panel.width : 50;
                oSlider.m_labelCurrentVal.height = 28;
                oSlider.m_labelCurrentVal.text = fDefault + "%";
                oSlider.m_labelCurrentVal.textAlignment = UIHorizontalAlignment.Right;
                oSlider.m_labelCurrentVal.textColor = Color.green;
                oSlider.m_labelCurrentVal.textScale = fTextScale;
            }
            // Label CurrentvalueSuffix
            oSlider.m_labelCurrentSuffix = oSlider.m_panel.AddUIComponent<UILabel>();
            if (oSlider.m_labelCurrentSuffix is not null)
            {
                oSlider.m_labelCurrentSuffix.autoSize = false;
                oSlider.m_labelCurrentSuffix.width = (direction == LayoutDirection.Vertical) ? oSlider.m_panel.width : 20;
                oSlider.m_labelCurrentSuffix.height = 28;
                oSlider.m_labelCurrentSuffix.text = ")";
                oSlider.m_labelCurrentSuffix.textAlignment = UIHorizontalAlignment.Left;
                oSlider.m_labelCurrentSuffix.textColor = Color.white;
                oSlider.m_labelCurrentSuffix.opacity = 0.5f;
                oSlider.m_labelCurrentSuffix.textScale = fTextScale;
            }
            // Replace slider handle with a new one
            GameObject.Destroy(oSlider.m_slider.thumbObject.gameObject);
			UISprite thumb = oSlider.m_slider.AddUIComponent<UISprite>();
			thumb.size = new Vector2(16, 16);
			thumb.position = new Vector2(0, 0);
			thumb.color = Color.white;
			thumb.spriteName = "InfoIconBaseHovered";
			oSlider.m_slider.thumbObject = thumb;

            oSlider.UpdateLabel();

            return oSlider;
		}

		public static UISlider CreateSlider(UIPanel parent, float width, float height, float min, float max)
		{
			UIPanel bg = parent.AddUIComponent<UIPanel>();
			bg.name = "sliderPanel";
			bg.autoLayout = false;
			bg.padding = new RectOffset(0, 0, 10, 0);
			bg.size = new Vector2(width, height);
            //bg.backgroundSprite = "InfoviewPanel";
            //bg.color = Color.green;

            UISlider slider = bg.AddUIComponent<UISlider>();
			slider.autoSize = false;
			slider.area = new Vector4(8, 0, bg.width, 15);
			slider.width = bg.width - 10;
			slider.height = 7;
            slider.relativePosition = new Vector3(8, slider.relativePosition.y + 5);
			slider.backgroundSprite = "SubBarButtonBasePressed";
			slider.fillPadding = new RectOffset(6, 6, 0, 0);
			slider.maxValue = max;
			slider.minValue = min;

			UISprite thumb = slider.AddUIComponent<UISprite>();
			thumb.size = new Vector2(16, 16);
			thumb.position = new Vector2(0, 0);
			thumb.spriteName = "InfoIconBaseHovered";

			slider.value = 0.0f;
			slider.thumbObject = thumb;

			return slider;
		}

		public void SetTooltip(string sTooltip)
		{
			if (m_slider is not null)
			{
				m_slider.tooltip = $"{m_fValue}";
			}
		}

		public void OnSliderValueChanged(float fValue)
        {
			m_fValue = fValue;

            UpdateLabel();
			if (m_eventCallback is not null)
            {
				m_eventCallback(fValue);
			}
		}

		private void UpdateLabel()
		{
            /*
			if (m_label is not null)
            {
                m_label.text = $"{m_sText}: {m_fValue}{(Percent ? "%" : "")}";
            }
			*/
            if (m_labelCurrentVal is not null)
            {
                m_labelCurrentVal.text = $"{m_fValue}" + "%";
                //m_label.text = $"{m_sText}: {m_fValue}{(Percent ? "%" : "")}";
            }
        }

		public void Enable(bool bEnable)
		{
			if (m_label is not null)
            {
				m_label.isEnabled = bEnable;
				m_label.disabledTextColor = Color.grey;
			}
			if (m_slider is not null)
            {
				m_slider.isEnabled = bEnable;
			}
		}

		public void SetValue(float fValue)
        {
			if (m_slider is not null)
			{
				m_slider.value = fValue;
			}
		}

		public void Destroy()
        {
			if (m_label is not null)
            {
				UnityEngine.Object.Destroy(m_label.gameObject);
				m_label = null;

			}
			if (m_slider is not null)
			{
				UnityEngine.Object.Destroy(m_slider.gameObject);
				m_slider = null;
			}
		}
	}

    
}
