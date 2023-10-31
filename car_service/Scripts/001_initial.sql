CREATE TABLE IF NOT EXISTS "Cars" (
     "Id" serial NOT NULL PRIMARY KEY, 
     "Brand" text,
     "Model" text,
     "YearOfRelease" smallint,
     "VINcode" text
);

CREATE TABLE IF NOT EXISTS "Clients" (
    "Id" serial NOT NULL PRIMARY KEY,
    "Name" text,
    "Surname" text,
    "Adress" text,
    "PhoneNumber" text
);

CREATE TABLE IF NOT EXISTS "Orders" (
    "Id" serial NOT NULL PRIMARY KEY,
    "CarId" integer REFERENCES "Cars"("Id") ON DELETE CASCADE,
    "ClientId" integer REFERENCES "Clients"("Id") ON DELETE CASCADE,
    "DateTime" timestamp,
    "WorkDescription" text,
    "Status" smallint
);
