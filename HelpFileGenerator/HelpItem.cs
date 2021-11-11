using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpFileGenerator
{
    public record HelpItem(string Name, string Description, bool IsAlias) {

        public override string ToString()
        {
            return $"{(IsAlias ? "*" : "")}{Name} — {Description}";
        }
    }
}
