using System;
using System.Collections.Generic;
using System.Text;

namespace Nikan.ViewModel
{
    public class PaginationSettings
    {

        public long TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public long CurrentPage { get; set; }
        public long MaxPagerItems { get; set; }
        public bool ShowFirstLast { get; set; }
        public bool ShowNumbered { get; set; }
        public bool UseReverseIncrement { get; set; }
        public bool SuppressEmptyNextPrev { get; set; }
        public bool SuppressInActiveFirstLast { get; set; }
        public bool RemoveNextPrevLinks { get; set; }


    }
}
