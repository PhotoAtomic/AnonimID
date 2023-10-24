using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonimId.Core
{

    public struct NoResult { }

    public interface IResult<TSuccess, TError>
    {
        bool IsSuccess { get; }

        TOut Match<TOut>(Func<TSuccess, TOut> success, Func<TError, TOut> failure);
    }

    public interface IResult<TSuccess> : IResult<TSuccess, Exception>
    {
    }

    public interface IResult : IResult<NoResult, Exception>
    {
    }

    public class Success : IResult
    {
        public bool IsSuccess => true;

        public Success()
        {
        }

        public override string ToString() => nameof(Success);

        public TOut Match<TOut>(Func<NoResult, TOut> success, Func<Exception, TOut> failure) => success(default);
        public TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure) => success();

    }

    public class Failure : IResult
    {
        public bool IsSuccess => false;

        private readonly Exception error;

        public Failure(Exception error)
        {
            this.error = error;
        }
        public override string ToString() => error.ToString();

        public TOut Match<TOut>(Func<NoResult, TOut> success, Func<Exception, TOut> failure) => failure(error);

        public TOut Match<TOut>(Func<TOut> success, Func<Exception, TOut> failure) => failure(error);


        public static implicit operator Exception(in Failure result) => result.error;

    }


    public struct Success<TSuccess> : IResult<TSuccess>
    {
        public bool IsSuccess => true;

        private readonly TSuccess data;
        public Success(TSuccess data)
        {
            this.data = data;
        }

        public override string ToString() => data?.ToString()!;

        public TOut Match<TOut>(Func<TSuccess, TOut> success, Func<Exception, TOut> failure) => success(data);


        public static implicit operator TSuccess(in Success<TSuccess> result) => result.data;
    }

    public struct Failure<TSuccess> : IResult<TSuccess>
    {
        public bool IsSuccess => false;

        private readonly Exception error;

        public Failure(Exception error)
        {
            this.error = error;
        }
        public override string ToString() => error.ToString();

        public TOut Match<TOut>(Func<TSuccess, TOut> success, Func<Exception, TOut> failure) => failure(error);


        public static implicit operator Exception(in Failure<TSuccess> result) => result.error;
    }
}
