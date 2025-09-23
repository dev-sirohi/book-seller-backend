namespace Common
{
    public sealed class ResultWrapper
    {
        private bool _success = true;
        private object _data = new { };
        private string? _message;
        private bool _returnInvalidHttp = true;
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }
        public string? Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public object Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public bool ReturnInvalidHttp
        {
            get { return _returnInvalidHttp; }
            set { _returnInvalidHttp = value; }
        }
    }
}
