namespace SimpleFPSClock;

[ConfigurationFileName("SimpleFPSClockConfig.xml")]
public class SimpleFPSClockConfiguration
{
	private static SimpleFPSClockConfiguration instance;

	public float FPSPanelPositionX = 50f;

	public float FPSPanelPositionY = 50f;

	public float ClockPanelPositionX = 50f;

	public float ClockPanelPositionY = 100f;
	public static SimpleFPSClockConfiguration Instance
	{
		get
		{
			if (instance == null)
			{
				instance = Configuration<SimpleFPSClockConfiguration>.Load();
			}
			return instance;
		}
	}
	public bool ConfigUpdated { get; set; }
	public bool ShowClockPanel { get; set; } = true;
	public bool ShowFPSPanel { get; set; } = true;
	public bool FPSColoredValue { get; set; } = true;
    public bool FPSTextSuffix { get; set; } = true;
    public float FPSPanelOpacity { get; set; } = 50f;
    public float FPSPanelTextScale { get; set; } = 100f;
    public float FPSPanelTextOpacity { get; set; } = 100f;
    public bool FPSPanelUseOutlineColor { get; set; } = false;
    public float ClockPanelTimeFormat { get; set; } = 1f;
    public float ClockPanelOpacity { get; set; } = 50f;
    public float ClockPanelTextScale { get; set; } = 100f;
    public float ClockPanelTextOpacity { get; set; } = 100f;
    public bool ClockPanelUseOutlineColor { get; set; } = false;

    public static void SaveFPSPanelPosition(float x, float y)
	{
		SimpleFPSClockConfiguration simpleFPSClockConfiguration = Configuration<SimpleFPSClockConfiguration>.Load();
		simpleFPSClockConfiguration.FPSPanelPositionX = x;
		simpleFPSClockConfiguration.FPSPanelPositionY = y;
		Configuration<SimpleFPSClockConfiguration>.Save();
	}

	public static void SaveClockPanelPosition(float x, float y)
	{
		SimpleFPSClockConfiguration simpleFPSClockConfiguration = Configuration<SimpleFPSClockConfiguration>.Load();
		simpleFPSClockConfiguration.ClockPanelPositionX = x;
		simpleFPSClockConfiguration.ClockPanelPositionY = y;
		Configuration<SimpleFPSClockConfiguration>.Save();
	}

	public void Apply()
	{
		ConfigUpdated = true;
	}

	public void Save()
	{
		Configuration<SimpleFPSClockConfiguration>.Save();
		ConfigUpdated = true;
	}
}
