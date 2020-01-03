namespace Move_Files
{
    public class FCRModel : IProcessModel
    {
        private ProcessSettingKeys settingKeys;

        public FCRModel()
        {
            settingKeys = new ProcessSettingKeys
            {
                SourceXMLPath = "fcrsourcexmlpath",
                SourceTIFPath = "fcrsourcetifpath",
                DestXMLPath = "fcrdestxmlpath",
                DestTIFPath = "fcrdesttifpath"
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
