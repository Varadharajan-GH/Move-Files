
namespace Move_Files
{
    public class VILModel : IProcessModel
    {
        private readonly ProcessSettingKeys settingKeys;
        public System.Windows.Forms.RadioButton processRadio;

        public VILModel(System.Windows.Forms.RadioButton rb)
        {
            settingKeys = new ProcessSettingKeys
            {
                SourceXMLPath = "vilsourcexmlpath",
                SourceTIFPath = "vilsourcetifpath",
                DestXMLPath = "vildestxmlpath",
                DestTIFPath = "vildesttifpath"
            };
            processRadio = rb;
        }

        public string GetCopiedTIFLogPrefix()
        {
            return "vilcopiedtif";
        }

        public string GetCopiedXMLLogPrefix()
        {
            return "vilcopiedxml";
        }

        public string GetErrorTIFLogPrefix()
        {
            return "vilerrortif";
        }

        public string GetErrorXMLLogPrefix()
        {
            return "vilerrorxml";
        }

        public string GetDestTIFPathKey()
        {
            return settingKeys.DestTIFPath;
        }

        public string GetDestXMLPathKey()
        {
            return settingKeys.DestXMLPath;
        }

        public string GetProcessName()
        {
            return "VIL";
        }

        public System.Windows.Forms.RadioButton GetRadioButton()
        {
            return processRadio;
        }

        public string GetSourceTIFPathKey()
        {
            return settingKeys.SourceTIFPath;
        }

        public string GetSourceXMLPathKey()
        {
            return settingKeys.SourceXMLPath;
        }

    }
}
