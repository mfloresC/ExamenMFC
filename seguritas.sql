CREATE DATABASE Seguritas;
GO

USE Seguritas;
GO

CREATE TABLE scUsuario (
scUId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
scUNombre VARCHAR (50) NOT NULL,
scUUsuario VARCHAR (50) NOT NULL,
scUPassword VARCHAR(50) NOT NULL,
);

CREATE TABLE scCliente (
scCId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
scCNombre VARCHAR(50) NOT NULL,
scCFechaMod DATETIME NOT NULL
);

ALTER TABLE scCliente ADD CONSTRAINT scCNombre_U UNIQUE(scCNombre);
GO

CREATE TABLE scCobertura (
scCoId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
scCoNombre VARCHAR(50) NOT NULL,
scCobSuma VARCHAR(50) NOT NULL,
scCoFechaMod DATETIME NOT NULL,
scCoEstatus BIT NOT NULL
);
GO

CREATE TABLE scPlan (
scPId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
scPNombre VARCHAR(50) NOT NULL,
scPFechaMod DATETIME NOT NULL
);
GO

CREATE TABLE soClientePlan (
soCPId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
soCPCId INT NOT NULL,
soCPPId INT NOT NULL,
soCPEstatus BIT NOT NULL,
soCPFechaMod DATETIME NOT NULL
);

ALTER TABLE soClientePlan ADD CONSTRAINT FK_soClientePlan_CId FOREIGN KEY (soCPCId) REFERENCES scCliente(scCId);
ALTER TABLE soClientePlan ADD CONSTRAINT FK_soClientePlan_PId FOREIGN KEY (soCPPId) REFERENCES scPlan(scPId);
GO

CREATE TABLE soPlanCobertura (
soPCoId INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
soPCoPId INT NOT NULL,
soPCoCoId INT NOT NULL,
soPCoEstatus BIT NOT NULL,
soPCoFechaMod DATETIME NOT NULL
);

ALTER TABLE soPlanCobertura ADD CONSTRAINT FK_soPlanCobertura_PId FOREIGN KEY (soPCoPId) REFERENCES scPlan(scPId);
ALTER TABLE soPlanCobertura ADD CONSTRAINT FK_soPlanCobertura_CoId FOREIGN KEY (soPCoCoId) REFERENCES scCobertura(scCoId);
GO