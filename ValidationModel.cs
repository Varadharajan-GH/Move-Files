namespace Move_Files
{
    public class ValidationModel : IProcessModel
    {
        private readonly ProcessSettingKeys settingKeys;
        public System.Windows.Forms.RadioButton processRadio;

        public ValidationModel(System.Windows.Forms.RadioButton rb)
        {
            settingKeys = new ProcessSettingKeys
            {
                SourceXMLPath = "validationsourcexmlpath",
                SourceTIFPath = "validationsourcetifpath",
                DestXMLPath = "validationdestxmlpath",
                DestTIFPath = "validationdesttifpath"
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
            return "Validation";
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
            return "validationcopiedtif";
        }

        public string GetCopiedXMLLogPrefix()
        {
            return "validationcopiedxml";
        }

        public string GetErrorTIFLogPrefix()
        {
            return "validationerrortif";
        }

        public string GetErrorXMLLogPrefix()
        {
            return "validationerrorxml";
        }
    }
}
