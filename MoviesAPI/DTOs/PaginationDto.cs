using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.DTOs
{
    public class PaginationDto
    {
        private int recordsPerPage=3;
        private readonly int maxAmount = 50;

        public int Page { get; set; } = 1;

        public int RecordsPerPage
        {
            get { return recordsPerPage; }
            set { recordsPerPage = value>maxAmount ? maxAmount : value; }
        }

    }
}
