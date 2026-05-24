using System;
using System.Collections.Generic;
using System.Text;

namespace WMSCommon.Contracts
{
    public interface ISyncEntity
    {
        public int Version { get; set; }
        public bool IsDeleted { get; set; }
    }
}
