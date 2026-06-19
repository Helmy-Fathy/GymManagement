using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Common
{
    public sealed record Result(bool success, string? error = null, ResultKind kind = ResultKind.Ok)
    {
        public static Result Ok() => new Result(success: true);

        public static Result Fail(string error, ResultKind kind = ResultKind.Conflict)
            => new(false, error, kind);

        public static Result NotFound(string error = "Not Found") => new(false, error, ResultKind.NotFound);

        public static Result Validation(string error) => new(false, error, ResultKind.ValidationFailed);
    }

    public sealed record Result<T>(bool success, T? value, string? error = null, ResultKind kind = ResultKind.Ok)
    {
        public static Result<T> Ok(T value) => new(true, value);

        public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict)
            => new(false, default, error, kind);

        public static Result<T> NotFound(string error = "Not Found") => new(false, default, error, ResultKind.NotFound);

        public static Result<T> Validation(string error) => new(false, default, error, ResultKind.ValidationFailed);
    }
}
