namespace Project.ViewModel
{
    public class BooleanResponse
    {
        public bool IsExist { get; set; } = false;
        public object ErrorData { get; set; }
        public string ErrorMessage { get; set; }
    }
}