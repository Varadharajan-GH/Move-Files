namespace Move_Files
{
    public class ValidationModel
    {
        private ProcessSettingKeys settingKeys;

        public ValidationModel()
        {
            settingKeys = new ProcessSettingKeys
            {
                SourceXMLPath = "validationsourcexmlpath",
                SourceTIFPath = "validationsourcetifpath",
                DestXMLPath = "validationdestxmlpath",
                DestTIFPath = "validationdesttifpath"
            };
        }

        public string GetDestTIFPathKey()
        {
            return settingKeys.DestTIFPath;
        }

        public string GetDestXMLPathKey()
        {
            return settingKeys.DestXMLPath;
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
