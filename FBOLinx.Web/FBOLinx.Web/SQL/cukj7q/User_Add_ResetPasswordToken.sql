ALTER TABLE
	[User]
ADD
	ResetPasswordToken varchar(255) null,
	ResetPasswordTokenExpiration datetime2(7) null
