UPDATE AspNetUsers SET EmailConfirmed = 1;

INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
VALUES 
(NEWID(), 'administrator', 'ADMINISTRATOR', NEWID()),
(NEWID(), 'bibliotekar', 'BIBLIOTEKAR', NEWID()),
(NEWID(), 'clan', 'CLAN', NEWID());