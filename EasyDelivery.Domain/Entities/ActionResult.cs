using System;
using System.Collections.Generic;
using System.Text;

namespace EasyDelivery.Domain.Entities
{
    public class TaskResult<T>
    {
        public bool Success { get; }
        public string Message { get; }
        public T Data { get; }

        protected TaskResult(bool success, T data, string message)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static TaskResult<T> Ok(T data, string message = "")
        {
            return new TaskResult<T>(true, data, message);
        }

        public static TaskResult<T> Fail(string message)
        {
            return new TaskResult<T>(false, default!, message);
        }

    }
}
