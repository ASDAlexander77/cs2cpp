using System.Collections.Generic;

namespace Il2Native.Logic.Project.Tasks
{
    public class FindBestConfigurations : Task
    {
        public List<string> Properties { get; internal set; }

        public List<string> PropertyValues { get; internal set; }

        public string SupportedConfigurations { get; internal set; }

        public string Configurations { get; internal set; }

        public string BestConfigurations { get; internal set; }

        public override bool Execute()
        {
            this.BestConfigurations = Configurations;
            return true;
        }
    }
}
