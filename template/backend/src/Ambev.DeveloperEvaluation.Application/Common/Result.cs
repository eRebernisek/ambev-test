using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.Application.Common
{
    public class Result<T>
    {
        public bool Succeeded { get; private set; }
        public T? Data { get; private set; }
        public List<string> Errors { get; private set; }

        private Result(bool succeeded, T? data, List<string> errors)
        {
            Succeeded = succeeded;
            Data = data;
            Errors = errors ?? new List<string>();
        }

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, data, new List<string>());
        }

        public static Result<T> Failure(List<string> errors)
        {
            return new Result<T>(false, default, errors ?? new List<string>());
        }

        public static Result<T> Failure(string error)
        {
            return new Result<T>(false, default, new List<string> { error ?? string.Empty });
        }
    }
} 