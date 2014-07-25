using System;
using Xunit;
using GestUAB.DataAccess;
using GestUAB.Migrations;
using GestUAB.Models;
using System.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using GestUAB.Tests;

namespace GestUAB.Tests
{
	public class ColaboradorDaoTests
	{
		public ColaboradorDaoTests ()
		{
			//Thread.CurrentThread.CurrentCulture = new CultureInfo ("en-US");
			if (System.IO.File.Exists("gestuab.sqlite")) {
				System.IO.File.Delete ("gestuab.sqlite");
			}
			Schema.Update ();
		}

		[Fact]
		public void Deve_Criar_Um_Colaborador_e_Ler_O_Mesmo() {
			var dao = new ColaboradorDao ();
			var obj1 = new Colaborador ();
			dao.Create (obj1);
            var obj2 = dao.Read<Colaborador> (obj1.Id);
			Assert.True (Compare.Equals<Colaborador>(obj1, obj2));
		}

		private Colaborador CreateRandom() {

			return new Colaborador () {
				Bairro = "Santa Cruz",
				Celular = "(42) 0000-0000",
				Cep = "85000-000",
				Complemento = "Apto. 2",
				Cpf = "000.000.000-00",
				DataNascimento = DateTime.Now,
				Logradouro = "Rua teste",
				Nome = "Colaborador teste",
				Numero = "15",
				Observacoes = "Teste",
				Documento = "0.000.000-0",
				Telefone = "(42) 0000-0000"
			};
		}
	}
}

