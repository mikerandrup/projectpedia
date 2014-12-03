CREATE TABLE Project
(
 rowid int PRIMARY KEY IDENTITY,
 projectid nvarchar(10) UNIQUE,
 headline nvarchar (60),
 byline nvarchar (300),
 details nvarchar (2048)
);

CREATE TABLE Collaborator
(
 rowid int PRIMARY KEY IDENTITY,
 personid nvarchar(10) UNIQUE,
 name nvarchar (60),
 primary_title nvarchar (60),
 biography nvarchar (1024),
 relationship nvarchar (1024),
 contact_linkedin nvarchar (120),
 contact_twitter nvarchar (60),
 contact_website nvarchar (60)
);

CREATE TABLE Skill
(
 rowid int PRIMARY KEY IDENTITY,
 skillid nvarchar(20) UNIQUE,
 skillname nvarchar(40),
 prominence int
);

CREATE TABLE rel_project2collaborator
(
 rowid int PRIMARY KEY IDENTITY,
 projectid nvarchar(10) REFERENCES Project(projectid),
 personid nvarchar(10) REFERENCES Collaborator(personid),
 roleheadline nvarchar(200),
 roledetails nvarchar(2048)
);

CREATE TABLE rel_skill2project
(
 rowid int PRIMARY KEY IDENTITY,
 skillid nvarchar(20) REFERENCES Skill(skillid),
 projectid nvarchar(10) REFERENCES Project(projectid)
);
