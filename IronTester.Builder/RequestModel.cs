namespace IronTester.Builder
{
    public class RequestModel
    {
        public bool WasProcessed { get; set; }
        public string SourceCodeLocation { get; set; }
        public bool IsValid { get; set; }
        public string ValidationFailReason { get; set; }
    }
}
