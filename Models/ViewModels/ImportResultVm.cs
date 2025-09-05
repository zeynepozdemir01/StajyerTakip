namespace StajyerTakip.Models.ViewModels
{
    public class ImportResultVm
    {
        public int TotalRows { get; set; }
        public int Inserted { get; set; }
        public int Skipped { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
