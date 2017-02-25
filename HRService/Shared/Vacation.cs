using System;

namespace Shared
{
    public class Vacation
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string Comments { get; set; }
        public DateTime AppliedOn { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovedOn { get; set; }
    }
}