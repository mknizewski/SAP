namespace SAP.Web.Models
{
    public class ProgramViewModel
    {
        public string OutputData { get; set; }
        public string File { get; set; }
        public double ExecutedTime { get; set; }
        public bool HasErrors { get; set; }
        public string ErrorData { get; set; }
        public double MemoryUsed { get; set; }
    }
}