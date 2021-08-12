CREATE TABLE FbolinxFileData (
    OID int IDENTITY(1,1) NOT NULL,
    FileName varchar(255) not null,
    FileData varbinary(max) not null,
    ContentType varchar(255),
    PRIMARY KEY (OID)
);
