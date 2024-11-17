namespace Task11
{
    public class PriorityQueueException: Exception
    {
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        } 
        public PriorityQueueException() : base() {}
        public PriorityQueueException(string message) : base(message) {this.Message = message;}
    }
}

