BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS [Config]
(
	[DatabaseVersion] VARCHAR(10)
);

INSERT INTO Config (DatabaseVersion) VALUES ('0.1a');

CREATE TABLE IF NOT EXISTS [Colaborador]
(
	[Id] GUID PRIMARY KEY,
    [Instituicao] VARCHAR(100) NOT NULL, -- Sigla + Nome 
    [Cpf] VARCHAR(11) NOT NULL,
    [Nome] VARCHAR(100) NOT NULL,
    [Profissao] VARCHAR(100) NOT NULL,
    [Sexo] INTEGER NOT NULL CHECK ([Sexo] IN (1, 2)), -- 1 Feminino 2 Masculino 
    [DataNascimento] DATETIME NOT NULL,
    [Documento] VARCHAR(20) NOT NULL,
    [TipoDocumento] VARCHAR(50) NOT NULL,
    [DataEmissaoDocumento] DATETIME NOT NULL,
    [OrgaoExpedidorDocumento] DATETIME NOT NULL,
    [UfNascimento] VARCHAR(2) NOT NULL CHECK ([UF] IN ("AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PR", "PB", "PA", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SE", "SP", "TO")),
    [MunicipioNascimento] VARCHAR(100) NOT NULL,
    [EstadoCivil] INTEGER NOT NULL CHECK ([EstadoCivil] IN (1, 2, 3, 4, 5, 6)), --1 "Solteiro", 2 "Casado", 3 "Separado", 4 "Divorciado", 5 "Viúvo", 6 "União Estável" 
    [NomeConjuge] VARCHAR(100),
    [NomePai] VARCHAR(100),
    [NomeMae] VARCHAR(100) NOT NULL,
    [Logradouro] VARCHAR(100) NOT NULL,
    [Complemento] VARCHAR(100),
    [Numero] VARCHAR(6) NOT NULL,
    [Bairro] VARCHAR(100) NOT NULL,
    [Cep] VARCHAR(8) NOT NULL,
    [Uf] VARCHAR(2) NOT NULL CHECK ([UF] IN ("AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PR", "PB", "PA", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SE", "SP", "TO")),
    [Municipio] VARCHAR(100) NOT NULL,
    [Telefone] VARCHAR(20) NOT NULL, --com DDD 
    [Celular] VARCHAR(20),
    [Email] VARCHAR(50),
    [AreaUltimoCursoSuperiorConcluido] VARCHAR(100),
    [UltimoCursoTitulacao] VARCHAR(100),
    [InstituicaoTitulacao] VARCHAR(100),
	[Observacoes] VARCHAR(500)
);


CREATE TABLE IF NOT EXISTS [Bolsista]
(
    [ColaboradorId] GUID REFERENCES Colaborador(Id),
    [Profissao] VARCHAR(100) NOT NULL,
    [Banco] VARCHAR(10) NOT NULL,
    [Agencia] VARCHAR(10) NOT NULL,
    [Conta] VARCHAR(10) NOT NULL
);

CREATE TABLE IF NOT EXISTS [VinculoBolsista]
(
    [ColaboradorId] GUID REFERENCES Colaborador(Id),
    [SetorId] GUID REFERENCES Setor(Id),
    [TipoCursoId] GUID REFERENCES TipoCurso(Id),
    [DataInicio] DATETIME NOT NULL,
    [DataFim] DATETIME NOT NULL,
    PRIMARY KEY ([ColaboradorId], [SetorId])
);

CREATE TABLE IF NOT EXISTS [VinculoTutor]
(
    [ColaboradorId] GUID REFERENCES Colaborador(Id),
    [CursoId] GUID REFERENCES Curso(Id),
    [PoloId] GUID REFERENCES Polo(Id),
    [DataInicio] DATETIME NOT NULL,
    [DataFim] DATETIME NOT NULL,
    [NumeroBolsas] INTEGER NOT NULL,
    PRIMARY KEY ([ColaboradorId], [CursoId], [PoloId])
);

CREATE TABLE IF NOT EXISTS [Curso]
(
    [Id] GUID PRIMARY KEY,
    [Nome] VARCHAR(100) NOT NULL,
    [TipoCurso] INTEGER NOT NULL CHECK ([Descricao] IN (1, 2, 3, 4, 5, 6, 7, 8, 9)) --"1 Aperfeiçoamento", "2 Bacharelado", "3 Lato Sensu", "4 Licenciatura", "5 Extensão", "6 Sequencial", "7 Tecnólogo", "8 Mestrado", "9 Doutorado" 
);

--SELECT lower(hex(randomblob(16)))
--http://www.guidgenerator.com/online-guid-generator.aspx
INSERT INTO TipoCurso (Id, Descricao) VALUES ("9a2db218-2b27-4284-b61d-084a7a99147b", "Aperfeiçoamento");
INSERT INTO TipoCurso (Id, Descricao) VALUES ("a2ae5467-3a88-4c62-a930-92a841c3a82e", "Bacharelado");
INSERT INTO TipoCurso (Id, Descricao) VALUES ("c0f649de-d8bc-443a-99d6-08f8e107c94f", "Lato Sensu");
INSERT INTO TipoCurso (Id, Descricao) VALUES ("bdfec130-9f23-4297-8e72-095b2b975cae", "Licenciatura");
INSERT INTO TipoCurso (Id, Descricao) VALUES ("c8382392-ae30-4aa6-8b39-c1e66e739b0e", "Extensão");
INSERT INTO TipoCurso (Id, Descricao) VALUES ("40ba5360-b1a5-46f9-941e-37fd83cd025b", "Sequencial");
INSERT INTO TipoCurso (Id, Descricao) VALUES ("b1cfabc2-159f-4aa6-a541-81180e0fba1f", "Tecnólogo");
INSERT INTO TipoCurso (Id, Descricao) VALUES ("01dde289-79a1-4a3f-b569-9cd81cf71a01", "Mestrado");
INSERT INTO TipoCurso (Id, Descricao) VALUES ("7f057cc4-b4d6-460e-ac1c-a547c38ba073", "Doutorado");

CREATE TABLE IF NOT EXISTS [VinculoCurso]
(
    [ColaboradorId] GUID REFERENCES Colaborador(Id),
    [CursoId] GUID REFERENCES Curso(Id),
    [FuncaoId] GUID REFERENCES Funcao(Id),
    [DataInicio] DATETIME NOT NULL,
    [DataFim] DATETIME NOT NULL,
    [NumeroBolsas] INTEGER NOT NULL,
    [Funcao] INTEGER NOT NULL,
    PRIMARY KEY ([ColaboradorId], [CursoId])
);

CREATE TABLE IF NOT EXISTS [Polo]
(
    [Id] GUID PRIMARY KEY,
    [Nome] VARCHAR(100) NOT NULL
);

CREATE TABLE IF NOT EXISTS [Setor]
(
    [Id] GUID PRIMARY KEY,
    [Nome] VARCHAR(50) NOT NULL
);

COMMIT TRANSACTION;