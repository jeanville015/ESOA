namespace ESOA.Model
{
  public class Pagination
  {
    public int StartRowNumber { get; set; }
    public int EndRowNumber { get; set; }
    public int PageSize { get; set; }
    public int SortIndex { get; set; }
    public string SortDirection { get; set; }
    public string FilterTerm { get; set; }
    public string DateFrom {get; set;} 
    public string DateTo {get; set;} 

    public Pagination()
    {
      SortDirection = "ASC";
      SortIndex = 0;
      StartRowNumber = 1;
      EndRowNumber = 10;
      PageSize = 10;
      FilterTerm = "";
      DateFrom = DateTime.MinValue.ToString();
      DateTo = DateTime.MinValue.ToString(); 
    }
  }
}