## Error Handling  

Catalyst comes with a GlobalExceptionHandler that catches all runtime exceptions and converts them into an API error response with a standard format of
```jsonc
{
	"ErrorCode": "InvalidTieColor",
	"Message" : "A string describing how to fix your wardrobe malfunction"
	"Data" : {
		... // depends on the exception
	}
}
```

This is the same format as errors from the Ordercloud platform, which is helpful because your Frontend can share error handling code. 

Catalyst provides a number of out of the box exceptions in the file DerivedExceptions.cs that may be helpful such as UnAuthorizedException, NotFoundException, and InvalidPropertyException.

```c#
if (UserContext.UserType != "Supplier") {
	throw new UnAuthorizedException();
}
```

You can also extend the CatalystBaseException in order to deliver your own error messages.

```c#
    public class DivideByZeroException : CatalystBaseException
    {
        public DivideByZeroException() : base("DivideByZero", 400, "You have violated a fundamental mathmatical law.") { }
    }
```

Use CatalystBaseExceptions for expected error scenarios. In the case of unexpected error scenarios like those caused by a bug in your code, the GlobalExceptionHandler will return a standard 500 error response. 
