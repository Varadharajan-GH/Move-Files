namespace Move_Files
{
    class VILModel : IProcessModel
    {
        private ProcessSettingKeys settingKeys;

        public VILModel()
        {
            settingKeys = new ProcessSettingKeys
            {
                SourceXMLPath = "vilsourcexmlpath",
                SourceTIFPath = "vilsourcetifpath",
                DestXMLPath = "vildestxmlpath",
                DestTIFPath = "vildesttifpath"
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
