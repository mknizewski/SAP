namespace SAP.Web.Areas.Admin.Models
{
    public class TestCompilerViewModel
    {
        public string Output { get; set; }
        public bool HasError { get; set; }
        public string ErrorString { get; set; }
        public string Language { get; set; }

        public static TestCompilerViewModel Inicialize(string output, bool hasError, string errorString, string lang)
        {
            TestCompilerViewModel viewModel = new TestCompilerViewModel();

            viewModel.Output = output;
            viewModel.HasError = hasError;
            viewModel.ErrorString = errorString;
            viewModel.Language = lang;

            return viewModel;
        }
    }
}