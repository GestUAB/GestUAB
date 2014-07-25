// 
// Bootstrapper.cs
//  
// Author:
//       Tony Alexander Hild <tony_hild@yahoo.com>
// 
// Copyright (c) 2012 
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using Nancy.Scaffolding;
using GestUAB.Models;
using System.Security.Cryptography.X509Certificates;

namespace GestUAB
{
    using Nancy;
    using Nancy.Authentication.Forms;
    using Nancy.Bootstrapper;
    using Nancy.Diagnostics;
    using Nancy.Security;
    using Nancy.TinyIoc;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
//        const int CACHE_SECONDS = 30;
//        readonly Dictionary<string, Tuple<DateTime, Response, int>> cachedResponses = new Dictionary<string, Tuple<DateTime, Response, int>> ();

//        protected override NancyInternalConfiguration InternalConfiguration
//        {
//            get
//            {   
//                return NancyInternalConfiguration.WithOverrides(x => x.NancyModuleBuilder = typeof(RavenModuleBuilder)); 
//            }
//        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            GestUAB.Migrations.Schema.Update();
            Jsonp.Enable(pipelines);

            RegisterScaffoldingConfigContainer(container);







            // At request startup we modify the request pipelines to
            // include forms authentication - passing in our now request
            // scoped user name mapper.
            //
            // The pipelines passed in here are specific to this request,
            // so we can add/remove/update items in them as we please.
//            var formsAuthConfiguration =
//                new FormsAuthenticationConfiguration()
//                {
//                    RedirectUrl = "~/login",
//                    UserMapper = container.Resolve<IUserMapper>()
//                };

//            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);

//            var membershipConfig = new MembershipConfig() {
//                OnlineTimeWindow = 10,
//                Provider = new RavenDbMembershipProvider(DataAccess.Database.DocumentStore)
//            };
//            Membership.Enable(membershipConfig);

#if (DEBUG)
            StaticConfiguration.DisableErrorTraces = false;
            StaticConfiguration.EnableRequestTracing = true;
#endif
        }

        protected override Nancy.Diagnostics.DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get
            {
                return new DiagnosticsConfiguration { Password = @"teste"};
            }
        }

        static void RegisterScaffoldingConfigContainer(TinyIoCContainer container)
        {
            var configContainer = new ConfigContainer();

            configContainer.Register((Colaborador x) => x.Id)
                .Named("Código")
                    .Described("Código do colaborador.")
                    .WithVisibilityConfig(all: Visibility.Hidden);

            configContainer.Register((Colaborador x) => x.Instituicao)
                .Named("Instituição")
                    .Described("Nome da instituição de vínculo. Ex.: Unicentro - Universidade Estadual do Centro-Oeste.");

            configContainer.Register((Colaborador x) => x.Cpf)
                .Named("CPF")
                    .Described("CPF do colaborador. Ex.: xxx.xxx.xxx-xx.");

            configContainer.Register((Colaborador x) => x.Nome)
                .Named("Nome")
                    .Described("Nome do colaborador. Ex.: João da Silva.");

            configContainer.Register((Colaborador x) => x.Profissao)
                .Named("Profissão")
                    .Described("Profissão do colaborador. Ex.: Professor.");

            configContainer.Register((Colaborador x) => x.Sexo)
                .Named("Sexo")
                    .Described("Sexo do colaborador.")
                    .WithEnumAsSelect();

            configContainer.Register((Colaborador x) => x.DataNascimento)
                .Named("Data de nascimento")
                    .Described("Data de Nascimento. Ex.: 01/01/1967.");

            configContainer.Register((Colaborador x) => x.TipoDocumento)
                .Named("Tipo do documnento")
                    .Described("Tipo do documento de identificação do colaborador.")
                    .WithEnumAsSelect();

            configContainer.Register((Colaborador x) => x.Documento)
                .Named("Documnento")
                    .Described("Documento de identificação do colaborador.");

            configContainer.Register((Colaborador x) => x.OrgaoEmissor)
                .Named("Orgão emissor")
                    .Described("Orgão emissor do documento. Ex.: SSP-PR.");

            configContainer.Register((Colaborador x) => x.DataEmissao)
                .Named("Data de emissão")
                    .Described("Data de emissão do documento. Ex.: 01/01/1967.");

            configContainer.Register((Colaborador x) => x.UfNascimento)
                .Named("UF de nascimento")
                    .Described("Unidade da federação de nascimento do colaborador.")
                    .WithEnumAsSelect();

            configContainer.Register((Colaborador x) => x.MunicipioNascimento)
                .Named("Município de nascimento")
                    .Described("Município de nascimento do colaborador.");

            configContainer.Register((Colaborador x) => x.EstadoCivil)
                .Named("Estado civil")
                    .Described("Estado civil de nascimento do colaborador.")
                    .WithEnumAsSelect();

            configContainer.Register((Colaborador x) => x.NomeConjuge)
                .Named("Nome do cônjuge")
                    .Described("Nome do cônjuge do colaborador.");

            configContainer.Register((Colaborador x) => x.NomePai)
                .Named("Nome do pai")
                    .Described("Nome do pai do colaborador.");

            configContainer.Register((Colaborador x) => x.NomeMae)
                .Named("Nome da mãe")
                    .Described("Nome da mãe do colaborador.");

            configContainer.Register((Colaborador x) => x.TipoLogradouro)
                .Named("Tipo do logradouro")
                    .Described("Tipo do logradouro.");

            configContainer.Register((Colaborador x) => x.Logradouro)
                .Named("Logradouro")
                    .Described("Logradouro. Ex. Fulano de Tal...");

            configContainer.Register((Colaborador x) => x.Numero)
                .Named("Número")
                    .Described("Número do endereço.");

            configContainer.Register((Colaborador x) => x.Complemento)
                .Named("Complemento")
                    .Described("Complemento do endereço.");

            configContainer.Register((Colaborador x) => x.Cep)
                .Named("CEP")
                    .Described("Código de endereçamento postal (CEP). Ex.: xxxxx-xxx.");

            configContainer.Register((Colaborador x) => x.Bairro)
                .Named("Bairro")
                    .Described("Bairro de residência do colaborador.");

            configContainer.Register((Colaborador x) => x.Uf)
                .Named("UF")
                    .Described("UF (Estado) de residência do colaborador.");

            configContainer.Register((Colaborador x) => x.Municipio)
                .Named("Município")
                    .Described("Município de residência.");

            configContainer.Register((Colaborador x) => x.Telefone)
                .Named("Telefone")
                    .Described("Telefone de contato. Ex.: (xx) xxxx-xxxx.");

            configContainer.Register((Colaborador x) => x.Celular)
                .Named("Celular")
                    .Described("Celular de contato. Ex.: (xx) xxxx-xxxx.");

            configContainer.Register((Colaborador x) => x.Email)
                .Named("E-mail")
                    .Described("Email de contato do colaborador. Ex.: nome@unicentro.br.");

            configContainer.Register((Colaborador x) => x.Observacoes)
                .Named("Observações")
                    .Described("Observação sobre o colaborador.");

            TinyIoCContainer.Current.Register<ConfigContainer>(configContainer);
            TinyIoCContainer.Current.Register<ColaboradorValidator>("ColaboradorValidator");
        }
    }
}

