using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Mycoshiro.Dataverse.LINQPad
{
    partial class CDSTemplate
    {
        public CDSTemplate(List<(EntityMetadata entityMetadata, List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata)> metadata) => Metadata = metadata;

        public List<(EntityMetadata entityMetadata, List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata)> Metadata { get; private set; }
    }
}
