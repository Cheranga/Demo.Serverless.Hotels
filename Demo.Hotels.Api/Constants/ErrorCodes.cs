namespace Demo.Hotels.Api.Constants
{
    public class ErrorCodes
    {
        public const string CannotGetCustomerData = nameof(CannotGetCustomerData);
        public const string InvalidCustomerDataReceived = nameof(InvalidCustomerDataReceived);
        public const string ApiError = nameof(ApiError);
    }

    public class ErrorMessages
    {
        public const string CannotGetCustomerData = "cannot retrieve customer data from the API";
        public const string InvalidCustomerDataReceived = "invalid data received from the API";
        public const string ApiError = "error occurred when calling the API.";
    }
}