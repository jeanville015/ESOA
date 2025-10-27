using ESOA.Model;
using System.Collections.Generic;

namespace ESOA.Model
{
    public class PaginationResponse<T>
    {
        public Pagination Paging { get; set; }  //{ EndRowNumber: 10; FilterTerm: NULL; PageSize: 10; SearchParameterValue: Null; SortDirection: ASC; SortIndex: 1; StartRowNumber: 1}
        public int TotalRows { get; set; }      //11
        public int FilteredRows { get; set; }   //10
        public List<T> Data { get; set; }       //count: 10 
        public string ListStatus { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
    }
}