namespace BMJ.Authenticator.Application.Common.Models
{
    public class Result
    {
        private bool succeeded;
        private string[] errors;
        internal Result(bool succeeded, IEnumerable<string> errors)
        {
            this.succeeded = succeeded;
            this.errors = errors.ToArray();
        }

        public bool IsSucceeded  => succeeded;

        public string[] GetErrors => errors;

        public static Result Success()
        {
            return new Result(true, Array.Empty<string>());
        }

        public static Result Failure(IEnumerable<string> errors)
        {
            return new Result(false, errors);
        }
    }
}
