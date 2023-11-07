using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK.Domain.Common
{
    public class GetRequestBase
    {
        public string KeySearch { get; set; } = null;
        public string[] OrderBy { get; set; } = null;

        private DateTime _fromDate = DateTime.MinValue;
        public DateTime? FromDate
        {
            get
            {
                return _fromDate;
            }
            set
            {
                _fromDate = value ?? DateTime.MaxValue;
            }
        }

        private DateTime _toDate = DateTime.Now;
        public DateTime? ToDate 
        { 
            get
            {
                return _toDate;
            }
            set
            {
                _toDate = value ?? DateTime.Now;
            }
        }
    }
}
