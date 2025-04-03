
CREATE TABLE public."Users"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "UserName" text COLLATE pg_catalog."default" NOT NULL,
    "NormalizedUserName" text COLLATE pg_catalog."default" NOT NULL,
    "Email" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Users_pkey" PRIMARY KEY ("Id")
);