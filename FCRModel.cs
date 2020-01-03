
namespace Move_Files
{
    public class FCRModel : IProcessModel
    {
        private readonly ProcessSettingKeys settingKeys;
        public System.Windows.Forms.RadioButton processRadio;

        public FCRModel(System.Windows.Forms.RadioButton rb)
        {
            settingKeys = new ProcessSettingKeys
            {
                SourceXMLPath = "fcrsourcexmlpath",
                SourceTIFPath = "fcrsourcetifpath",
                DestXMLPath = "fcrdestxmlpath",
                DestTIFPath = "fcrdesttifpath"
            };
            processRadio = rb;
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
            return "FCR";
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

        public string GetCopiedTIFLogPrefix()
        {
            return "fcrcopiedtif";
        }

        public string GetCopiedXMLLogPrefix()
        {
            return "fcrcopiedxml";
        }

        public string GetErrorTIFLogPrefix()
        {
            return "fcrerrortif";
        }

        public string GetErrorXMLLogPrefix()
        {
            return "fcrerrorxml";
        }
    }
}
