using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared.Result
{
    public class Result
    {

        protected readonly List<Error> _errors = [];
        public bool IsSuccess => _errors.Count == 0;// True
        public bool IsFailure => !IsSuccess; // False

        public IReadOnlyList<Error> Errors => _errors;

        // Ok - Success
        protected Result()
        {

        }
        // Fail with error
        protected Result(Error error)
        {
            _errors.Add(error);
        }
        // Fail with errors
        protected Result(List<Error> errors)
        {
            _errors.AddRange(errors);
        }


        public static Result Ok => new();
        public static Result Fail(Error error) => new(error);
        public static Result Fail(List<Error> errors) => new(errors);

    }

    public class Result<TValue> : Result
    {
        private readonly TValue _value;
        public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Cannot access the value of a failed result.");

        // Ok - Success with value
        private Result(TValue value) : base()
        {
            _value = value;
        }
        // Fail with error
        private Result(Error error) : base(error)
        {
            _value = default!;
        }
        // Fail with errors
        private Result(List<Error> errors) : base(errors)
        {
            _value = default!;
        }

        public static new Result<TValue> Ok(TValue value) => new(value);
        public static new Result<TValue> Fail(Error error) => new(error);
        public static new Result<TValue> Fail(List<Error> errors) => new(errors);


        public static implicit operator Result<TValue>(TValue value) => Ok(value);
        public static implicit operator Result<TValue>(Error error) => Fail(error);
        public static implicit operator Result<TValue>(List<Error> errors) => Fail(errors);


    }
}
