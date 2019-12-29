using System.Xml;
using System.Xml.XPath;

namespace Move_Files
{
    public class MySettings
    {
        private string settingsFilePath;

        public MySettings(string settingsFile)
        {
            settingsFilePath = settingsFile;
            if (!System.IO.File.Exists(settingsFilePath))
            {                
                using (XmlWriter writer = XmlWriter.Create(settingsFilePath, new XmlWriterSettings { Indent = true }))
                {
                    writer.WriteStartElement("settings");
                    writer.WriteStartElement("sourcexmlpath");
                    writer.WriteEndElement();
                    writer.WriteStartElement("sourcetifpath");
                    writer.WriteEndElement();
                    writer.WriteStartElement("destxmlpath");
                    writer.WriteEndElement();
                    writer.WriteStartElement("desttifpath");
                    writer.WriteEndElement();
                    writer.WriteStartElement("xmllog");
                    writer.WriteEndElement();
                    writer.WriteStartElement("tiflog");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    writer.Flush();
                }
            }
        }
        #region Read Data From XML
        /// <summary>
        /// Reads the data of specified node provided in the parameter
        /// </summary>
        /// <param name="settingName">Node to be read</param>
        /// <returns>string containing the value</returns>
        public string ReadSetting(string settingName)
        {
            try
            {
                //settingsFilePath is a string variable storing the path of the settings file 
                XPathDocument doc = new XPathDocument(settingsFilePath);
                XPathNavigator nav = doc.CreateNavigator();
                // Compile a standard XPath expression
                XPathExpression expr;
                expr = nav.Compile(@"/settings/" + settingName);
                XPathNodeIterator iterator = nav.Select(expr);
                // Iterate on the node set
                while (iterator.MoveNext())
                {
                    return iterator.Current.Value;
                }
                return string.Empty;
            }
            catch
            {
                //do some error logging here. Leaving for you to do 
                return string.Empty;
            }
        }

        /// <summary>
        /// Writes the updated value to XML
        /// </summary>
        /// <param name="settingName">Node of XML to read</param>
        /// <param name="settingValue">Value to write to that node</param>
        /// <returns></returns>
        public bool WriteSetting(string settingName, string settingValue)
        {
            try
            {
                //settingsFilePath is a string variable storing the path of the settings file 
                XmlTextReader reader = new XmlTextReader(settingsFilePath);
                XmlDocument doc = new XmlDocument();
                doc.Load(reader);
                //we have loaded the XML, so it's time to close the reader.
                reader.Close();
                XmlNode oldNode;
                XmlElement root = doc.DocumentElement;
                oldNode = root.SelectSingleNode("/settings/" + settingName);
                if(oldNode == null)
                {
                    XmlElement elem= doc.CreateElement(settingName);
                    elem.InnerText = settingValue;
                    oldNode = elem;
                }                
                root.AppendChild(oldNode);
                oldNode.InnerText = settingValue;
                doc.Save(settingsFilePath);
                return true;
            }
            catch
            {
                //properly you need to log the exception here. But as this is just an
                //example, I am not doing that. 
                return false;
            }
        }
        #endregion
    }
}
