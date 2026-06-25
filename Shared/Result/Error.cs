using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Result
{
    public class Error
    {
        public string Code { get; }
        public string Description { get; }
        public ErrorType Type { get; }

        private Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        #region Static factory method to create Error

        public static Error Create(string Code = "General.Failure", string Description = "A general failure has occurred")
        {
            return new Error(Code, Description, ErrorType.Failure);
        }
        public static Error validation(string Code = "General.validation", string Description = "A general validation has occurred")
        {
            return new Error(Code, Description, ErrorType.validation);
        }
        public static Error NotFound(string Code = "General.NotFound", string Description = "A general NotFound has occurred")
        {
            return new Error(Code, Description, ErrorType.NotFound);
        }
        public static Error Unauthorized(string Code = "General.Unauthorized", string Description = "A general Unauthorized has occurred")
        {
            return new Error(Code, Description, ErrorType.Unauthorized);
        }
        public static Error Forbidden(string Code = "General.Forbidden", string Description = "A general Forbidden has occurred")
        {
            return new Error(Code, Description, ErrorType.Forbidden);
        }
        public static Error InvalidCrendentials(string Code = "General.InvalidCrendentials", string Description = "A general InvalidCrendentials has occurred")
        {
            return new Error(Code, Description, ErrorType.InvalidCrendentials);
        }
        public static Error Failure(string Code = "Failure.InvalidCrendentials", string Description = "A general Failure has occurred")
        {
            return new Error(Code, Description, ErrorType.Failure);
        }

        #endregion
    }
}
