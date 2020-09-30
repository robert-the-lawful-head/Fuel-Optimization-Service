declare @DefaultConductorGroup int = 0

select @DefaultConductorGroup = g.OID
from [group] g
where g.[Group] = 'fbolinx'

delete from [user] where GroupID = @DefaultConductorGroup

delete from [group] where OID = @DefaultConductorGroup

delete from [user] where [username] = 'jessica@fuelerlinx.com'
delete from [user] where [username] = 'jaclyn@fuelerlinx.com'
delete from [user] where [username] = 'kevin@fuelerlinx.com'
delete from [user] where [username] = 'chau@fuelerlinx.com'
delete from [user] where [username] = 'mike@fuelerlinx.com'
delete from [user] where [username] = 'dmadaras@fuelerlinx.com'
delete from [user] where [username] = 'kathy@fuelerlinx.com'

insert into [group] ([Group], [Username], [Password], ISFBONetwork, Active)
values ('FBOLinx Conductor - Jessica', 'jessica@fuelerlinx.com', 'Filus123', 0, 1),
		('FBOLinx Conductor - Jackie', 'jaclyn@fuelerlinx.com', 'Filus123', 0, 1),
		('FBOLinx Conductor - Kevin', 'kevin@fuelerlinx.com', 'Filus123', 0, 1),
		('FBOLinx Conductor - Chau', 'chau@fuelerlinx.com', 'Filus123', 0, 1),
		('FBOLinx Conductor - Mike', 'mike@fuelerlinx.com', 'Filus123', 0, 1),
		('FBOLinx Conductor - Dave', 'dmadaras@fuelerlinx.com', 'Filus123', 0, 1),
		('FBOLinx Conductor - Kathy', 'kathy@fuelerlinx.com', 'Filus123', 0, 1)
