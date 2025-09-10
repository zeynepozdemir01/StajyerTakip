namespace StajyerTakip.Application.Common
{
    public sealed class Result
    {
        public bool Succeeded { get; }
        public string? Error { get; }

        private Result(bool succeeded, string? error)
        {
            Succeeded = succeeded;
            Error = error;
        }

        public static Result Ok() => new Result(true, null);

        public static Result Fail(string? error) =>
            new Result(false, string.IsNullOrWhiteSpace(error) ? "Bir hata oluştu." : error);
    }

    public sealed class Result<T>
    {
        public bool Succeeded { get; }
        public string? Error { get; }
        public T? Value { get; }

        private Result(bool succeeded, T? value, string? error)
        {
            Succeeded = succeeded;
            Value = value;
            Error = error;
        }

        public static Result<T> Ok(T value) => new Result<T>(true, value, null);

        public static Result<T> Fail(string? error) =>
            new Result<T>(false, default, string.IsNullOrWhiteSpace(error) ? "Bir hata oluştu." : error);
    }
}
