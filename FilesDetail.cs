using System.Collections.Generic;

namespace Move_Files
{
    public class FilesDetail
    {
        public List<string> filesList;
        public long totalSize;
        public FilesDetail(List<string> fList, long tSize)
        {
            filesList = fList;
            totalSize = tSize;
        }
    }
}
