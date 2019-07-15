using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using CostsAnalyse.Models.Data;

namespace CostsAnalyse.Services.Managers
{
    public class StateManager
    {
        private readonly BinaryFormatter _bf = new BinaryFormatter();

        public void SaveState(ParseState state)
        {
            using (FileStream fs = new FileStream("state.data", FileMode.Open, FileAccess.Write))
            {
                _bf.Serialize(fs,state);
            }
        }

        public ParseState RecoverState()
        {
            if (File.Exists("state.data"))
            {
                using (FileStream fs = new FileStream("state.data", FileMode.Open, FileAccess.Read))
                {
                    return (ParseState) _bf.Deserialize(fs);
                }
            } 
            return null;
        }
    }
}