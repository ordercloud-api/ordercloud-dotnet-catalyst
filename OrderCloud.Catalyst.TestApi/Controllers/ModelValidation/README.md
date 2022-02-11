## Model Validation  

When working with your own custom models, you can apply DataAnnotation attributes to enofrce certain rules when those models are part of PUT or POST calls.  You can also define custom error messages to provide helpful advice on how to troubleshoot these errors.  Catalyst offers several examples of the types of attributes that may be used.

### Required

Indicate that a data field value is required.  Empty values will trigger the custom error message provided.

```c#
		[Required(ErrorMessage = "This field is required, please try again.")]
		public string RequiredField { get; set; }
```

### String Length

Define a maximum string length, with the option to specify a minimum length.

```c#
		[StringLength(25, MinimumLength = 10, ErrorMessage = "This value must be at least 10 characters and no more than 25 characters.")]
		public string BoundedString { get; set; }
```

### Range

Define a low and high range of possible values for various data types, including ints, decimals, and dates.

```c#
		[Range(0.01, 100.00, ErrorMessage = "This value must be between {1} and {2}.")]
		public decimal BoundedDecimal { get; set; }

		[Range(1, 100, ErrorMessage = "This value must be between {1} and {2}.")]
		public int BoundedInteger { get; set; }
```

### Email Address

Ensure that a user's input comports to a typical format for email addresses.

```c#
		[EmailAddress(ErrorMessage = "The email address provided is not valid.")]
		public string Email { get; set; }
```

### Credit Card Number

Verify that a user is entering a credit card number in a format that is well-formed as a potentially acceptable card value.

```c#
		[CreditCard(ErrorMessage = "The credit card number provided is not valid.")]
		public string CreditCardNumber { get; set; }
```

### Regular Expressions

Define your own regular expressions to validate the value passed through is in the expected format.

```c#
		// Regex Example - Alphanumeric, no special characters or spaces
		[RegularExpression(@"^[a-zA-Z][a-zA-Z0-9]*$", ErrorMessage = "Invalid characters (special characters and spaces are not allowed).")]
		public string RegexExample { get; set; }
```