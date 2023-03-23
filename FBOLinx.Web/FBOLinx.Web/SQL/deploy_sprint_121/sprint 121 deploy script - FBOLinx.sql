use paragon_Test
GO
ALTER TABLE FuelReq
	ADD CustomerNotes varchar(MAX) null,
	PaymentMethod varchar(255) null;
GO
use paragon_Test
GO
ALTER TABLE FBOPreferences
	ADD OrderNotificationsEnabled Bit NULL;
GO
