namespace Move_Files
{
    public interface IProcessModel
    {
        System.Windows.Forms.RadioButton GetRadioButton();
        string GetProcessName();
        string GetSourceXMLPathKey();
        string GetSourceTIFPathKey();
        string GetDestXMLPathKey();
        string GetDestTIFPathKey();

        string GetCopiedXMLLogPrefix();
        string GetCopiedTIFLogPrefix();
        string GetErrorXMLLogPrefix();
        string GetErrorTIFLogPrefix();
    }
}
