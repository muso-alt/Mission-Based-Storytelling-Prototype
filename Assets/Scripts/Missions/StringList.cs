using System.Collections.Generic;

namespace Unfrozen.Tasks
{
    [System.Serializable]
    public class StringList
    {
        public StringList()
        {
            
        }
        
        public StringList(IEnumerable<string> array)
        {
            Items.AddRange(array);
        }
        
        public List<string> Items = new List<string>();
    }
}