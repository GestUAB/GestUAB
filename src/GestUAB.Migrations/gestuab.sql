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
    [Profissao] VARCHAR(100),
    [Sexo] SMALLINT NOT NULL, -- CHECK ([Sexo] IN (1, 2)), -- 1 Feminino 2 Masculino 
    [DataNascimento] DATETIME NOT NULL,
    [Documento] VARCHAR(20) NOT NULL,
    [TipoDocumento] SMALLINT NOT NULL,
    [DataEmissao] DATETIME NOT NULL,
    [OrgaoEmissor] VARCHAR(100) NOT NULL,
    [UfNascimento] SMALLINT NOT NULL, -- CHECK ([UF] IN ("AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PR", "PB", "PA", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SE", "SP", "TO")),
    [MunicipioNascimento] VARCHAR(100) NOT NULL,
    [EstadoCivil] SMALLINT NOT NULL, -- CHECK ([EstadoCivil] IN (1, 2, 3, 4, 5, 6)), --1 "Solteiro", 2 "Casado", 3 "Separado", 4 "Divorciado", 5 "Viúvo", 6 "União Estável" 
    [NomeConjuge] VARCHAR(100),
    [NomePai] VARCHAR(100),
    [NomeMae] VARCHAR(100) NOT NULL,
    [TipoLogradouro] SMALLINT NOT NULL,
    [Logradouro] VARCHAR(100) NOT NULL,
    [Complemento] VARCHAR(100),
    [Numero] VARCHAR(6) NOT NULL,
    [Bairro] VARCHAR(100) NOT NULL,
    [Cep] VARCHAR(8) NOT NULL,
    [Uf] SMALLINT NOT NULL, --CHECK ([UF] IN ("AC", "AL", "AP", "AM", "BA", "CE", "DF", "ES", "GO", "MA", "MT", "MS", "MG", "PR", "PB", "PA", "PE", "PI", "RJ", "RN", "RS", "RO", "RR", "SC", "SE", "SP", "TO")),
    [Municipio] VARCHAR(100) NOT NULL,
    [Telefone] VARCHAR(14) NOT NULL, -- (xx) xxxx-xxxx  
    [Celular] VARCHAR(14),
    [Email] VARCHAR(50),
	[Observacoes] VARCHAR(500)
);

/*
CREATE TABLE IF NOT EXISTS [VinculoTutor]
(
    [Id] GUID PRIMARY KEY,  
    [ColaboradorId] GUID REFERENCES Colaborador(Id),
    [CursoId] GUID REFERENCES Curso(Id),
    [PoloId] GUID REFERENCES Polo(Id),
    [DataInicio] DATETIME NOT NULL,
    [DataFim] DATETIME NOT NULL,
    [NumeroBolsas] INTEGER NOT NULL,
);

CREATE TABLE IF NOT EXISTS [PagamentoBolsa]
(
    [Id] GUID PRIMARY KEY,  
    [VinculoBolsistaId] GUID REFERENCES VinculoBolsista(Id),
    [DataPagamento] DATETIME NOT NULL
);
*/

CREATE TABLE IF NOT EXISTS [Setor]
(
    [Id] GUID PRIMARY KEY,
    [Nome] VARCHAR(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS [Curso]
(
    [Id] GUID PRIMARY KEY,
    [SetorId] GUID REFERENCES Setor(Id),
    [Nome] VARCHAR(100) NOT NULL,
    [TipoCurso] SMALLINT
);

/*
CHECK ([TipoCurso] IN (0, 1, 2, 3, 4, 5, 6, 7, 8, 9)) --"1 Aperfeiçoamento", "2 Bacharelado", "3 Lato Sensu", "4 Licenciatura", "5 Extensão", "6 Sequencial", "7 Tecnólogo", "8 Mestrado", "9 Doutorado", "10 Outro"   CREATE TABLE IF NOT EXISTS [Vinculo]
(
    [Id] GUID PRIMARY KEY,  
    [ColaboradorId] GUID REFERENCES Colaborador(Id),
    [SetorId] GUID REFERENCES Setor(Id),
    [DataInicio] DATETIME NOT NULL,
    [DataFim] DATETIME NOT NULL,
    [Funcao] INTEGER NOT NULL CHECK ([Funcao] IN (1, 2, 3, 4, 5, 6, 7, 8)), 
    [PoloId] GUID REFERENCES Polo(Id),
    [BolsasPrevistas] INTEGER NOT NULL DEFAULT 0,
    [BolsasPagas] INTEGER NULL
    [Banco] VARCHAR(10) NULL,
    [Agencia] VARCHAR(10) NULL,
    [Conta] VARCHAR(10) NULL
);

CREATE TABLE IF NOT EXISTS [Polo]
(
    [Id] GUID PRIMARY KEY,
    [Nome] VARCHAR(100) NOT NULL
);
*/



COMMIT TRANSACTION;