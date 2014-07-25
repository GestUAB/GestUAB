using System;
using System.Configuration;
using GestUAB.DataAccess;
using GestUAB.Models;
using ServiceStack.OrmLite;
using System.Reflection;
using System.IO;

namespace GestUAB.Migrations
{
    public class Schema
    {
        public Schema()
        {

        }

        public static void Update()
        {
            using (var c = Database.OpenConnection())
            {
                c.ExecuteSql(System.IO.File.OpenText(Path.Combine("bin", "gestuab.sql")).ReadToEnd());
                Populate();
//                c.CreateTable (true, (new {__Table = "AnonymousTable", __Id = new {PrimaryKey = true},
//                    Id = Guid.NewGuid (), 
//                    __Field1 = new {Nullable = false},
//                    Field1 = 10, 
//                    __Field2 = new {MaxLength = 50},
//                    Field2 = "test", 
//                    Field3 = new int? () }));
            }
        }

        static void Populate()
        {
            ColaboradorManager.Insert(new Colaborador()
            {
                Bairro = "Bairro 1",
                Celular = "4291068640",
                Cep = "85012270",
                Complemento = "Apto. 2",
                Cpf = "34833938111",
                DataEmissao = new DateTime(1998, 10, 10),
                DataNascimento = new DateTime(1980, 11, 07),
                Documento = "82316574",
                Email = "tony_hild@yahoo.com",
                EstadoCivil = EstadoCivilType.Casado,
                Instituicao = "Unicentro",
                Logradouro = "Rua Presidente Getúlio Vargas",
                Municipio = "Guarapuava",
                MunicipioNascimento = "Guarapuava",
                Nome = "Tony Alexander Hild",
                NomeConjuge = "Graciane da Silva Hild",
                NomeMae = "Therezinha Scoroboate Hild",
                NomePai = "Antonio Hild",
                Numero = "744",
                Observacoes = "Teste",  
                OrgaoEmissor = "SSP-PR",
                Profissao = "Professor",
                Sexo = SexoType.Masculino,
                Telefone = "4230351380",
                TipoDocumento = DocumentoType.CarteiraIdentidade,
                Uf = UfType.PR,
                UfNascimento = UfType.PR
            }
            );

            ColaboradorManager.Insert(new Colaborador()
            {
                Bairro = "Bairro 2",
                Celular = "4291068640",
                Cep = "85012270",
                Complemento = "Apto. 2",
                Cpf = "31232863718",
                DataEmissao = new DateTime(1998, 10, 10),
                DataNascimento = new DateTime(1980, 11, 07),
                Documento = "82316574",
                Email = "tony_hild@yahoo.com",
                EstadoCivil = EstadoCivilType.Casado,
                Instituicao = "Unicentro",
                Logradouro = "Rua Presidente Getúlio Vargas",
                Municipio = "Guarapuava",
                MunicipioNascimento = "Guarapuava",
                Nome = "José da Silva",
                NomeConjuge = "Blabarina da Silva",
                NomeMae = "Therezinha da Silva",
                NomePai = "Antonio da Silva",
                Numero = "744",
                Observacoes = "Teste",  
                OrgaoEmissor = "SSP-PR",
                Profissao = "Professor",
                Sexo = SexoType.Masculino,
                Telefone = "4230351380",
                TipoDocumento = DocumentoType.CarteiraIdentidade,
                Uf = UfType.PR,
                UfNascimento = UfType.PR
            }
            );

            ColaboradorManager.Insert(new Colaborador()
            {
                Bairro = "Bairro 2",
                Celular = "4291068640",
                Cep = "85012270",
                Complemento = "Apto. 2",
                Cpf = "45763436610",
                DataEmissao = new DateTime(1998, 10, 10),
                DataNascimento = new DateTime(1980, 11, 07),
                Documento = "82316574",
                Email = "tony_hild@yahoo.com",
                EstadoCivil = EstadoCivilType.Casado,
                Instituicao = "Unicentro",
                Logradouro = "Rua Presidente Getúlio Vargas",
                Municipio = "Guarapuava",
                MunicipioNascimento = "Guarapuava",
                Nome = "Manoel da Silva",
                NomeConjuge = "Blabarina da Silva",
                NomeMae = "Therezinha da Silva",
                NomePai = "Antonio da Silva",
                Numero = "744",
                Observacoes = "Teste",  
                OrgaoEmissor = "SSP-PR",
                Profissao = "Professor",
                Sexo = SexoType.Masculino,
                Telefone = "4230351380",
                TipoDocumento = DocumentoType.CarteiraIdentidade,
                Uf = UfType.PR,
                UfNascimento = UfType.PR
            }
            );

            SetorManager.Insert(new Setor(){Nome = "Coordenação"});
            SetorManager.Insert(new Setor(){Nome = "Financeiro"});
            SetorManager.Insert(new Setor(){Nome = "Multidiciplinar"});
            var setor = new Setor(){Nome = "Especializações"};
            SetorManager.Insert(new Setor(){Nome = "Especializações"});

            CursoManager.Insert(new Curso() {Nome = "Mídias na Educação", Setor = setor, TipoCurso = CursoType.LatoSensu});
            CursoManager.Insert(new Curso() {Nome = "Gestão Escolar", Setor = setor, TipoCurso = CursoType.LatoSensu});
            CursoManager.Insert(new Curso() {Nome = "Gestão Pública Municipal", Setor = setor, TipoCurso = CursoType.LatoSensu});
        }
    }
}
